using AutoMapper;
using ClinicAPI.DB;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Transactions;

namespace ClinicAPI.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMedicalAppointmentRepository _medicalAppointmentRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationDBContext _dbContext;
        private readonly UserManager<User> _userManager;

        public PatientService(IPatientRepository patientRepository, IMapper mapper, ApplicationDBContext dbContext
                              , UserManager<User> userManager, IMedicalAppointmentRepository medicalAppointmentRepository)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
            _dbContext = dbContext;
            _userManager = userManager;
            _medicalAppointmentRepository = medicalAppointmentRepository;
        }


        public async Task<ReturnPatientDto?> GetPatient(int id)
        {
            var patient = await _patientRepository.GetPatientById(id);
            return _mapper.Map<ReturnPatientDto>(patient);
        }

        public async Task<ReturnPatientDto?> GetPatientByUserId(string id)
        {
            var patient = await _patientRepository.GetPatientByUserId(id);
            return _mapper.Map<ReturnPatientDto>(patient);
        }

        public async Task<List<ReturnPatientDto>> GetAllPatients()
        {
            var patients = await _patientRepository.GetAllPatients();
            return _mapper.Map<List<ReturnPatientDto>>(patients);

            //return _mapper.Map<List<ReturnMessageDto>>(messages);

        }

        public async Task<List<ReturnPatientDto>> GetAllAvailablePatients()
        {
            var patients = await _patientRepository.GetAllAvailablePatients();
            return _mapper.Map<List<ReturnPatientDto>>(patients);
        }

        
        public async Task<(bool Confirmed, string Response, ReturnPatientDto? patient)> CreatePatient(CreatePatientDto patient) 
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                               new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                               TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                if (_dbContext.Patient.Any(p => p.Pesel == patient.Pesel))
                {
                    ReturnPatientDto? k = null;
                    return (false, "Patient with this PESEL already exists.", k);
                }
                Patient _patient = new Patient
                {
                    Pesel = patient.Pesel,
                    Name = patient.Name,
                    Surname = patient.Surname,
                    PatientNumber = "domyslnyNumer",
                };
                Patient? p = await _patientRepository.CreatePatient(_patient);
                if (p == null)
                {
                    ReturnPatientDto? k = null;
                    return await Task.FromResult((false, "Patient was not created.", k));

                }
                ReturnPatientDto r = _mapper.Map<ReturnPatientDto>(p);
                scope.Complete();
                return await Task.FromResult((true, "Patient successfully created.", r));
            }
            catch (Exception ex)
            {
                return (false, $"Error updating DiagnosticTest: {ex.Message}", null);
            }
            
        }

        public async Task<(bool Confirmed, string Response, ReturnPatientDto? patient)> RegisterPatient(CreateRegisterPatientDto request)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                               new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                               TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                if (await _patientRepository.GetPatientWithTheSamePesel(request.Pesel))
                {
                    return (false, "Patient with this PESEL already exists.", null);
                }

                var user = new User
                {
                    UserName = request.Email,
                    Email = request.Email
                };

                var createUserResult = await _userManager.CreateAsync(user, request.Password);
                if (!createUserResult.Succeeded)
                {
                    var errorMessages = createUserResult.Errors.Select(e => e.Description).ToList();
                    return (false, string.Join("; ", errorMessages), null);
                }

                var addToRoleResult = await _userManager.AddToRoleAsync(user, UserRole.Patient);
                if (!addToRoleResult.Succeeded)
                {
                    ReturnPatientDto? k = null;
                    return (false, "Failed to assign role to the user.", k);
                }

                var patient = new Patient
                {
                    UserId = user.Id,
                    Pesel = request.Pesel,
                    Name = request.Name,
                    Surname = request.Surname,
                    PatientNumber = "domyslnyNumer"
                };

                Patient? p = await _patientRepository.CreatePatient(patient);
                if (p == null)
                {
                    ReturnPatientDto? k = null;
                    return await Task.FromResult((false, "Patient was not created.", k));
                   
                }
                ReturnPatientDto r = _mapper.Map<ReturnPatientDto>(p);
                scope.Complete();
                return await Task.FromResult((true, "Patient successfully registered.", r));
            }
            catch (Exception ex)
            {
                return (false, $"Error updating DiagnosticTest: {ex.Message}", null);
            }
        }



        public async Task<(bool Confirmed, string Response)> UpdatePatient(UpdatePatientDto patient)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                               new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                               TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                if (await _patientRepository.GetPatientWithTheSamePesel(patient.Pesel))
                {
                    return (false, "Patient with this PESEL already exists.");
                }
                var _patient = await _patientRepository.GetPatientById(patient.Id);

                if (_patient == null)
                {
                    return await Task.FromResult((false, "Patient with given id does not exist."));
                }
                _patient.Name = patient.Name;
                _patient.Surname = patient.Surname;
                _patient.Pesel = patient.Pesel;

                var p = await _patientRepository.UpdatePatient(_patient);
                scope.Complete();
                return await Task.FromResult((true, "Patient succesfully uptated"));
            }
            catch (Exception ex)
            {
                return (false, $"Error updating DiagnosticTest: {ex.Message}");
            }
            
        }

        public async Task<(bool Confirmed, string Response)> TransferToArchive(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                               new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                               TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var _patient = await _patientRepository.GetPatientById(id);

                if (_patient == null)
                {
                    return await Task.FromResult((false, "Patient with given id does not exist."));
                }
                if (!await _patientRepository.CanArchivePatient(id))
                {
                    return await Task.FromResult((false, "Can nor archive Patient with appointments."));
                }
                _patient.IsAvailable = false;
                var p = await _patientRepository.UpdatePatient(_patient);
                scope.Complete();
                return await Task.FromResult((true, "Patient succesfully uptated"));
            }
            catch (Exception ex)
            {
                return (false, $"Error updating DiagnosticTest: {ex.Message}");
            }
           
        }



        public async Task<(bool Confirmed, string Response)> DeletePatient(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                               new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                               TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var patient = await _patientRepository.GetPatientById(id);
                if (patient == null)
                {
                    return await Task.FromResult((false, "Patient with given id does not exist."));
                }
                //sprawdzenie czy mozna usunac pacjenta (czy ma jakąkolwiek przypisaną wizytę)
                if (await _medicalAppointmentRepository.HasPatientMedicalAppointments(id))
                {
                    return await Task.FromResult((false, "Can not delete patient with appointments."));
                }
                await _patientRepository.DeletePatient(id);
                scope.Complete();
                return await Task.FromResult((true, "Patient successfully deleted."));
            }
            catch (Exception ex)
            {
                return (false, $"Error updating DiagnosticTest: {ex.Message}");
            }
           
        }
    }
}
