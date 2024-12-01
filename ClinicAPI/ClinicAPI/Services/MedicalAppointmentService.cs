﻿using AutoMapper;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Services.Interfaces;

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
        public async Task<ReturnMedicalAppointmentDto?> GetMedicalAppointment(int id)
        {
            var medicalAppointment = await _medicalAppointmentRepository.GetMedicalAppointmentById(id);
            return _mapper.Map<ReturnMedicalAppointmentDto>(medicalAppointment);
        }
        
        public async Task<List<ReturnMedicalAppointmentDto>> GetAllMedicalAppointments()
        {
            var medicalAppointments = await _medicalAppointmentRepository.GetAllMedicalAppointments();
            return _mapper.Map<List<ReturnMedicalAppointmentDto>>(medicalAppointments);
        }
        
        public async Task<(bool Confirmed, string Response, ReturnMedicalAppointmentDto? medAppointment)> CreateMedicalAppointment(CreateMedicalAppointmentDto medicalAppointment)
        {
            MedicalAppointment _medicalAppointment = new MedicalAppointment
            {
                dateTime = medicalAppointment.dateTime,
                PatientId = medicalAppointment.PatientId,
                DoctorId = medicalAppointment.DoctorId,
                Interview = medicalAppointment.Interview,
                Diagnosis = medicalAppointment.Diagnosis,
                DiseaseUnit = medicalAppointment.DiseaseUnit
            };
            MedicalAppointment? p = await _medicalAppointmentRepository.CreateMedicalAppointment(_medicalAppointment);
            if (p != null)
            {
                ReturnMedicalAppointmentDto r = _mapper.Map<ReturnMedicalAppointmentDto>(p);
                return await Task.FromResult((true, "medicalAppointment successfully created.", r));
            }
            else
            {
                ReturnMedicalAppointmentDto? k = null;
                return await Task.FromResult((false, "medicalAppointment was not created.", k));
            }
        }
        
        public async Task<(bool Confirmed, string Response)> UpdateMedicalAppointment(UpdateMedicalAppointmentDto medicalAppointment)
        {
            MedicalAppointment? _medicalAppointment = await _medicalAppointmentRepository.GetMedicalAppointmentById(medicalAppointment.Id);

            if (_medicalAppointment == null)
            {
                return await Task.FromResult((false, "medicalAppointment with given id does not exist."));
            }
            else
            {
                MedicalAppointment r = _mapper.Map<MedicalAppointment>(medicalAppointment);
                var p = await _medicalAppointmentRepository.UpdateMedicalAppointment(r);
                return await Task.FromResult((true, "medicalAppointment succesfully uptated"));
            }
        }
        
        public async Task<(bool Confirmed, string Response)> DeleteMedicalAppointment(int id)
        {
            var medicalAppointment = await _medicalAppointmentRepository.GetMedicalAppointmentById(id);
            if (medicalAppointment == null) return await Task.FromResult((false, "medicalAppointment with given id does not exist."));
            else
            {
                await _medicalAppointmentRepository.DeleteMedicalAppointment(id);
                return await Task.FromResult((true, "medicalAppointment successfully deleted."));
            }
        }
    }
}
