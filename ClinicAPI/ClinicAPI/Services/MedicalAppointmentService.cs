﻿using AutoMapper;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Services.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace ClinicAPI.Services
{
    public class MedicalAppointmentService : IMedicalAppointmentService
    {
        private readonly IMedicalAppointmentRepository _medicalAppointmentRepository;
        private readonly IMapper _mapper;

        public MedicalAppointmentService(IMedicalAppointmentRepository medicalAppointmentRepository, IMapper mapper)
        {
            _medicalAppointmentRepository = medicalAppointmentRepository;
            _mapper = mapper;
        }

        public async Task<MedicalAppointment?> GetMedicalAppointment(int id)
        {
            var medicalAppointment = await _medicalAppointmentRepository.GetMedicalAppointmentById(id);
            return medicalAppointment;
        }


        public async Task<ReturnMedicalAppointmentPatientDto?> GetMedicalAppointmentByIdWithPatient(int id)
        {
            var medicalAppointment = await _medicalAppointmentRepository.GetMedicalAppointmentByIdWithPatient(id);
            return medicalAppointment;
        }       

        public async Task<List<ReturnMedicalAppointmentPatientDoctorDto>> GetAllMedicalAppointmentsPatientsDoctors()
        {
            var medicalAppointments = await _medicalAppointmentRepository.GetAllMedicalAppointmentsPatientsDoctors();
            return medicalAppointments;
        }

        public async Task<List<ReturnMedicalAppointmentDto>> GetAllMedicalAppointments()
        {
            var medicalAppointments = await _medicalAppointmentRepository.GetAllMedicalAppointments();
            return _mapper.Map<List<ReturnMedicalAppointmentDto>>(medicalAppointments);
        }      

        public async Task<List<ReturnMedicalAppointmentDoctorDto>> GetMedicalAppointmentsBySpecialisation(int id)
        {
            var medicalAppointments = await _medicalAppointmentRepository.GetMedicalAppointmentsBySpecialisation(id);
            return medicalAppointments;
        }

        public async Task<MedicalAppointmentsOfPatient> GetMedicalAppointmentsByDoctorId(int id)
        {
            MedicalAppointmentsOfPatient allAppointments = new MedicalAppointmentsOfPatient(); //zmienic nazwe
            var medicalAppointments = await _medicalAppointmentRepository.GetMedicalAppointmentsByDoctorId(id);
            foreach (ReturnMedicalAppointmentPatientDoctorDto medicalAppointment in medicalAppointments)
            {
                if((bool)medicalAppointment.IsFinished || (bool)medicalAppointment.IsCancelled)
                {
                    allAppointments.pastMedicalAppointments.Add(medicalAppointment);
                }
                else
                {
                    allAppointments.futureMedicalAppointments.Add(medicalAppointment);
                }
            }
            return allAppointments;
        }

        public async Task<List<ReturnMedicalAppointmentPatientDoctorDto>> GetFutureMedicalAppointmentsByPatientOrUserId(int id, string userId, string role)
        {
            if (role == UserRole.Patient)
            {
                var medicalAppointments = await _medicalAppointmentRepository.GetFutureMedicalAppointmentsByPatientUserId(userId);
                if (medicalAppointments == null)
                {
                    throw new KeyNotFoundException("Nie znaleziono wizyt lekarskich.");
                }
                return medicalAppointments;
            }
            else if (role == UserRole.Registrant)
            {
                var medicalAppointments = await _medicalAppointmentRepository.GetFutureMedicalAppointmentsByPatientId(id);
                if (medicalAppointments == null)
                {
                    throw new KeyNotFoundException("Nie znaleziono wizyt lekarskich.");
                }
                return medicalAppointments;
            }
            else
            {
                throw new UnauthorizedAccessException("Nieznana rola użytkownika.");               
            }
        }

        public async Task<List<ReturnMedicalAppointmentPatientDoctorDto>> GetPastMedicalAppointmentsByPatientOrUserId(int id, string userId, string role)
        {
            if (role == UserRole.Patient)
            {
                var medicalAppointments = await _medicalAppointmentRepository.GetPastMedicalAppointmentsByPatientUserId(userId);
                if (medicalAppointments == null)
                {
                    throw new KeyNotFoundException("Nie znaleziono wizyt lekarskich.");
                }
                return medicalAppointments;
            }
            else if (role == UserRole.Registrant)
            {
                var medicalAppointments = await _medicalAppointmentRepository.GetPastMedicalAppointmentsByPatientId(id);
                if (medicalAppointments == null)
                {
                    throw new KeyNotFoundException("Nie znaleziono wizyt lekarskich.");
                }
                return medicalAppointments;
            }
            else
            {
                throw new UnauthorizedAccessException("Nieznana rola użytkownika.");
            }
        }

        public async Task<MedicalAppointmentsOfPatient> GetMedicalAppointmentsByPatientId(int id)
        {
            MedicalAppointmentsOfPatient allAppointments = new MedicalAppointmentsOfPatient();
            var medicalAppointments = await _medicalAppointmentRepository.GetMedicalAppointmentsByPatientId(id);
            foreach (ReturnMedicalAppointmentPatientDoctorDto medicalAppointment in medicalAppointments)
            {
                if ((bool)medicalAppointment.IsFinished)
                {
                    allAppointments.pastMedicalAppointments.Add(medicalAppointment);
                }
                else
                {
                    allAppointments.futureMedicalAppointments.Add(medicalAppointment);
                }
            }
            return allAppointments;
        }

        public async Task<(bool Confirmed, string Response, ReturnMedicalAppointmentDto? medAppointment)> CreateMedicalAppointment(CreateMedicalAppointmentDto medicalAp)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                               new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted },
                               TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var appointmentDate = new DateTime(medicalAp.Date.Year, medicalAp.Date.Month, medicalAp.Date.Day, medicalAp.Time.Hour, medicalAp.Time.Minute, 0);
                var todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                if (appointmentDate < todayDate)
                {
                    return (false, "The appointment date cannot be in the past.", null);
                }
                MedicalAppointment _medicalAppointment = new MedicalAppointment
                {
                    DateTime = new DateTime(medicalAp.Date.Year, medicalAp.Date.Month, medicalAp.Date.Day, medicalAp.Time.Hour, medicalAp.Time.Minute, 0),
                    PatientId = 0,
                    DoctorId = medicalAp.DoctorId,
                    Interview = "domyslny interview",
                    Diagnosis = "domyslne diagnosis",
                    IsCancelled = false,
                    IsFinished = false,
                    CancellingComment = ""
                };
                MedicalAppointment? p = await _medicalAppointmentRepository.CreateMedicalAppointment(_medicalAppointment);
                if (p == null)
                {
                    ReturnMedicalAppointmentDto? k = null;
                    return await Task.FromResult((false, "medicalAppointment was not created.", k));
                }
                ReturnMedicalAppointmentDto r = _mapper.Map<ReturnMedicalAppointmentDto>(p);
                scope.Complete();
                return await Task.FromResult((true, "medicalAppointment successfully created.", r));
            }
            catch (Exception ex)
            {
                return (false, $"Error cerating appointmnent: {ex.Message}", null);
            }
        }
        
        public async Task<(bool Confirmed, string Response)> UpdatePatientCancel(ReturnMedicalAppointmentPatientDoctorDto medicalAppointment)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var _medicalAppointment = await _medicalAppointmentRepository.GetMedicalAppointmentById(medicalAppointment.Id);

                if (_medicalAppointment == null)
                {
                    return await Task.FromResult((false, "medicalAppointment with given id does not exist."));
                }
                _medicalAppointment.DateTime = medicalAppointment.DateTime;
                _medicalAppointment.PatientId = medicalAppointment.PatientId;
                _medicalAppointment.Interview = medicalAppointment.Interview;
                _medicalAppointment.Diagnosis = medicalAppointment.Diagnosis;
                _medicalAppointment.DoctorId = medicalAppointment.DoctorId;
                _medicalAppointment.IsFinished = medicalAppointment.IsFinished;
                _medicalAppointment.IsCancelled = medicalAppointment.IsCancelled;
                _medicalAppointment.CancellingComment = medicalAppointment.CancellingComment;
                var p = await _medicalAppointmentRepository.UpdateMedicalAppointment(_medicalAppointment);
                scope.Complete();
                return await Task.FromResult((true, "medicalAppointment succesfully uptated"));
            }
            catch (Exception ex)
            {
                return (false, $"Error updating appointmnent: {ex.Message}");
            }
        }

        public async Task<(bool Confirmed, string Response)> UpdateMedicalAppointment(UpdateMedicalAppointmentDto medicalAppointment)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var _medicalAppointment = await _medicalAppointmentRepository.GetMedicalAppointmentById(medicalAppointment.Id);

                if (_medicalAppointment == null)
                {
                    return await Task.FromResult((false, "medicalAppointment with given id does not exist."));
                }
                _medicalAppointment.DateTime = medicalAppointment.DateTime;
                _medicalAppointment.PatientId = medicalAppointment.PatientId;
                _medicalAppointment.Interview = medicalAppointment.Interview;
                _medicalAppointment.Diagnosis = medicalAppointment.Diagnosis;
                _medicalAppointment.DoctorId = medicalAppointment.DoctorId;
                _medicalAppointment.IsFinished = medicalAppointment.IsFinished;
                _medicalAppointment.IsCancelled = medicalAppointment.IsCancelled;
                _medicalAppointment.CancellingComment = medicalAppointment.CancellingComment;
                var p = await _medicalAppointmentRepository.UpdateMedicalAppointment(_medicalAppointment);
                scope.Complete();
                return await Task.FromResult((true, "medicalAppointment succesfully uptated"));
            }
            catch (Exception ex)
            {
                return (false, $"Error updating appointmnent: {ex.Message}");
            }
        }

        public async Task<(bool Confirmed, string Response)> DeleteMedicalAppointment(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                               new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                               TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var medicalAppointment = await _medicalAppointmentRepository.GetMedicalAppointmentById(id);
                if (medicalAppointment == null)
                {
                    return await Task.FromResult((false, "medicalAppointment with given id does not exist."));
                }
                await _medicalAppointmentRepository.DeleteMedicalAppointment(id);
                scope.Complete();
                return await Task.FromResult((true, "medicalAppointment successfully deleted."));
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting appointmnent: {ex.Message}");
            }
        }
    }
}
