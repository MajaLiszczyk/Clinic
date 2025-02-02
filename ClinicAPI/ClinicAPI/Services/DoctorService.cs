using AutoMapper;
using ClinicAPI.DB;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Transactions;

namespace ClinicAPI.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMedicalAppointmentRepository _medicalAppointmentRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationDBContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IMedicalSpecialisationService _medicalSpecialisationService;



        public DoctorService(IDoctorRepository doctorRepository, IMapper mapper, ApplicationDBContext applicationDBContext
                            , UserManager<User> userManager, IMedicalSpecialisationService medicalSpecialisationService
                            , IMedicalAppointmentRepository medicalAppointmentRepository)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
            _dbContext = applicationDBContext;
            _userManager = userManager;
            _medicalSpecialisationService = medicalSpecialisationService;
            _medicalAppointmentRepository = medicalAppointmentRepository;
        }
        public async Task<ReturnDoctorDto?> GetDoctor(int id)
        {
            var doctor = await _doctorRepository.GetDoctorById(id);
            return _mapper.Map<ReturnDoctorDto>(doctor);
        }

        public async Task<List<ReturnDoctorDto>> GetAllDoctors()
        {
            var doctors = await _doctorRepository.GetAllDoctors();
            return _mapper.Map<List<ReturnDoctorDto>>(doctors);
        }

        public async Task<List<ReturnDoctorDto>> GetAllAvailableDoctors()
        {
            var doctors = await _doctorRepository.GetAllAvailableDoctors();
            return _mapper.Map<List<ReturnDoctorDto>>(doctors);
        }

        public async Task<List<DoctorWithSpecialisations>> GetDoctorsWithSpecialisations()
        {
            var doctors = await _doctorRepository.GetDoctorsWithSpecialisations();
            return doctors;
        }


        public async Task<(bool Confirmed, string Response, ReturnDoctorDto? doctor)> CreateDoctor(CreateDoctorDto request, ICollection<MedicalSpecialisation> medicalSpecialisations)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                               new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                               TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                Doctor _doctor = new Doctor
                {
                    Name = request.Name,
                    Surname = request.Surname,
                    DoctorNumber = request.DoctorNumber,
                    MedicalSpecialisations = medicalSpecialisations
                };
                Doctor? p = await _doctorRepository.CreateDoctor(_doctor);
                if (p == null)
                {
                    ReturnDoctorDto? k = null;
                    return await Task.FromResult((false, "doctor was not created.", k)); 
                }
                ReturnDoctorDto r = _mapper.Map<ReturnDoctorDto>(p);
                scope.Complete();
                return await Task.FromResult((true, "doctor successfully created.", r));

            }
            catch (Exception ex)
            {
                return (false, $"Error creating doctor: {ex.Message}", null);
            }


        }

        public async Task<(bool Confirmed, string Response, ReturnDoctorDto? doctor)> CreateDoctorWithSpecialisations(CreateDoctorDto request)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                ICollection<int> medicalSpecialisationsIds = request.MedicalSpecialisationsIds;
                ICollection<MedicalSpecialisation> medicalSpecialisations = new List<MedicalSpecialisation>();
                MedicalSpecialisation specialisation;
                foreach (int id in medicalSpecialisationsIds)
                {
                    specialisation = await _medicalSpecialisationService.GetRawSpecialisation(id);
                    medicalSpecialisations.Add(specialisation);
                }
                return await CreateDoctor(request, medicalSpecialisations);
            }
            catch (Exception ex)
            {
                return (false, $"Error creating doctor: {ex.Message}", null);
            }
        }


        public async Task<(bool Confirmed, string Response, ReturnDoctorDto? doctor)> RegisterDoctor(CreateRegisterDoctorDto request)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                               new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                               TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                if (await _doctorRepository.GetDoctorWithTheSameNumber(request.DoctorNumber))
                {
                    return (false, "Doctor with this PWZ number already exists.", null);
                }
                // Tworzenie użytkownika
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

                // Przypisanie roli Doctor do użytkownika
                var addToRoleResult = await _userManager.AddToRoleAsync(user, UserRole.Doctor);
                if (!addToRoleResult.Succeeded)
                {
                    return (false, "Failed to assign role to the user.", null);
                }

                ICollection<int> medicalSpecialisationsIds = request.MedicalSpecialisationsIds;
                ICollection<MedicalSpecialisation> medicalSpecialisations = new List<MedicalSpecialisation>();
                MedicalSpecialisation specialisation;
                foreach (int id in medicalSpecialisationsIds)
                {
                    specialisation = await _medicalSpecialisationService.GetRawSpecialisation(id);
                    medicalSpecialisations.Add(specialisation);
                }
                var doctor = new Doctor
                {
                    UserId = user.Id,
                    Name = request.Name,
                    Surname = request.Surname,
                    DoctorNumber = request.DoctorNumber,
                    MedicalSpecialisations = medicalSpecialisations
                };

                Doctor? d = await _doctorRepository.CreateDoctor(doctor);
                if (d == null)
                {
                    ReturnDoctorDto? k = null;
                    return await Task.FromResult((false, "Doctor was not created.", k));

                }
                ReturnDoctorDto r = _mapper.Map<ReturnDoctorDto>(d);
                scope.Complete();
                return await Task.FromResult((true, "Doctor successfully registered.", r));

            }
            catch (Exception ex)
            {
                return (false, $"Error registering doctor: {ex.Message}", null);
            }
        }



        public async Task<(bool Confirmed, string Response)> UpdateDoctor(UpdateDoctorDto doctor, ICollection<MedicalSpecialisation> medicalSpecialisations)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                if (await _doctorRepository.GetDoctorWithTheSameNumber(doctor.DoctorNumber))
                {
                    return (false, "Doctor with this PWZ number already exists.");
                }
                var _doctor = await _doctorRepository.GetDoctorById(doctor.Id);

                if (_doctor == null)
                {
                    return await Task.FromResult((false, "doctor with given id does not exist."));
                }
                _doctor.Name = doctor.Name;
                _doctor.Surname = doctor.Surname;
                _doctor.DoctorNumber = doctor.DoctorNumber;
                _doctor.MedicalSpecialisations = medicalSpecialisations;

                //Doctor r = _mapper.Map<Doctor>(doctor);
                var p = await _doctorRepository.UpdateDoctor(_doctor);
                scope.Complete();
                return await Task.FromResult((true, "doctor succesfully uptated"));

            }
            catch (Exception ex)
            {
                return (false, $"Error registering doctor: {ex.Message}");
            }

        }

        public async Task<(bool Confirmed, string Response)> UpdateDoctorWithSpecialisations(UpdateDoctorDto request)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                ICollection<int> medicalSpecialisationsIds = request.MedicalSpecialisationsIds;
                ICollection<MedicalSpecialisation> medicalSpecialisations = new List<MedicalSpecialisation>();
                MedicalSpecialisation specialisation;
                foreach (int id in medicalSpecialisationsIds)
                {
                    specialisation = await _medicalSpecialisationService.GetRawSpecialisation(id);
                    medicalSpecialisations.Add(specialisation);
                }
                return await UpdateDoctor(request, medicalSpecialisations);
            }
            catch (Exception ex)
            {
                return (false, $"Error registering doctor: {ex.Message}");
            }
        }


        public async Task<(bool Confirmed, string Response)> TransferToArchive(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                           new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                           TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var _doctor = await _doctorRepository.GetDoctorById(id);
                if (_doctor == null)
                {
                    return await Task.FromResult((false, "doctor with given id does not exist."));
                }
                if (!await _doctorRepository.CanArchiveDoctor(id))
                {
                    return await Task.FromResult((false, "Can nor archive doctor with appointments.")); // Nie można zarchiwizować lekarza
                }
                _doctor.IsAvailable = false;

                //Doctor r = _mapper.Map<Doctor>(doctor);
                var p = await _doctorRepository.UpdateDoctor(_doctor);
                scope.Complete();
                return await Task.FromResult((true, "doctor succesfully uptated"));
            }
            catch (Exception ex)
            {
                return (false, $"Error transfering to archive doctor: {ex.Message}");
            }
        }


        public async Task<(bool Confirmed, string Response)> DeleteDoctor(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                               new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                               TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var doctor = await _doctorRepository.GetDoctorById(id);
                if (doctor == null)
                {
                    return await Task.FromResult((false, "doctor with given id does not exist."));
                }
                if (await _medicalAppointmentRepository.HasDoctorMedicalAppointments(id))
                {
                    return await Task.FromResult((false, "can not delete doctor with appointments."));
                }
                await _doctorRepository.DeleteDoctor(id);
                scope.Complete();
                return await Task.FromResult((true, "doctor successfully deleted."));
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting doctor: {ex.Message}");
            }

        }
    }
}
