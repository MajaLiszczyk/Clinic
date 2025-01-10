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

namespace ClinicAPI.Services
{
    //SERWIS powinien przygotować dane w postaci odpowiedniej dla kontrolera (dto) - mapuje encje odebrane z repo na dto. 
    //Obsługuje sytuacje, w których wynik zapytania jest null, ale nie powinien jeszcze zwracać kodów HTTP (to rola kontrolera).
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationDBContext _dbContext;
        private readonly UserManager<User> _userManager;

        public PatientService(IPatientRepository patientRepository, IMapper mapper, ApplicationDBContext dbContext, UserManager<User> userManager)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
            _dbContext = dbContext;
            _userManager = userManager;
        }


       // public Task<ReturnPatientDto?> GetPatientAsync(int id)
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

        
        //public Task<(bool Confirmed, string Response, Patient? patient)> CreatePatientAsync(CreatePatientDto patient)
        //TO DO : PRZY TWRZENIEU PESEL NIE MOZE SIE POWTORZYC W BAZIE
        public async Task<(bool Confirmed, string Response, ReturnPatientDto? patient)> CreatePatient(CreatePatientDto patient) 
        {
            if (_dbContext.Patient.Any(p => p.Pesel == patient.Pesel))
            {
                ReturnPatientDto? k = null; //BARDZO ZŁA PRAKTYKA??
                return (false, "Patient with this PESEL already exists.", k);
                //return BadRequest(new { Message = "Patient with this PESEL already exists" });
            }
            Patient _patient = new Patient
            {
                Pesel = patient.Pesel,
                Name = patient.Name,
                Surname = patient.Surname,
                PatientNumber = "domyslnyNumer",
            };
            Patient? p = await _patientRepository.CreatePatient(_patient);
            if (p != null) {
                ReturnPatientDto r = _mapper.Map<ReturnPatientDto>(p);
                return await Task.FromResult((true, "Patient successfully created.", r));
            }
            else
            {
                ReturnPatientDto? k = null; //bez sensu tak obchodzić, da się inaczej?
                return await Task.FromResult((false, "Patient was not created.", k));

            }
        }

        public async Task<(bool Confirmed, string Response, ReturnPatientDto? patient)> RegisterPatient(CreateRegisterPatientDto request)
        {
            //if (_dbContext.Patient.Any(p => p.Pesel == request.Pesel))
            if (await _patientRepository.GetPatientWithTheSamePesel(request.Pesel))
            {
                //ReturnPatientDto? k = null; //BARDZO ZŁA PRAKTYKA??
                return (false, "Patient with this PESEL already exists.", null);
                //return BadRequest(new { Message = "Patient with this PESEL already exists" });
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

            // Przypisanie roli Patient do użytkownika
            var addToRoleResult = await _userManager.AddToRoleAsync(user, UserRole.Patient);
            if (!addToRoleResult.Succeeded)
            {
                ReturnPatientDto? k = null;
                return (false, "Failed to assign role to the user.", k);  //WYSTARCZY PEWNIE ZAMIAST K DAC BEZPOSREDNIO NULL?
            }

            // Tworzenie encji Patient i powiązanie z User
            var patient = new Patient
            {
                UserId = user.Id,
                Pesel = request.Pesel,
                Name = request.Name,
                Surname = request.Surname,
                PatientNumber = "domyslnyNumer"
            };

            Patient? p = await _patientRepository.CreatePatient(patient);
            if (p != null)
            {
                ReturnPatientDto r = _mapper.Map<ReturnPatientDto>(p);
                return await Task.FromResult((true, "Patient successfully registered.", r));
            }

            else //DA SIĘ INACZEJ OBEJŚĆ?
            {
                ReturnPatientDto? k = null; //bez sensu tak obchodzić, da się inaczej?
                return await Task.FromResult((false, "Patient was not created.", k));

            }
        }



        public async Task<(bool Confirmed, string Response)> UpdatePatient(UpdatePatientDto patient)
        {
            if (await _patientRepository.GetPatientWithTheSamePesel(patient.Pesel))
            {
                //ReturnPatientDto? k = null; //BARDZO ZŁA PRAKTYKA??
                return (false, "Patient with this PESEL already exists.");
                //return BadRequest(new { Message = "Patient with this PESEL already exists" });
            }
            //Patient? _patient = await _patientRepository.GetPatientById(patient.Id);   
            var _patient = await _patientRepository.GetPatientById(patient.Id);   

            if (_patient == null) {
                return await Task.FromResult((false, "Patient with given id does not exist."));
            }
            else{
                _patient.Name = patient.Name;
                _patient.Surname = patient.Surname;
                _patient.Pesel = patient.Pesel;

                //Patient r = _mapper.Map<Patient>(patient);
                var p = await _patientRepository.UpdatePatient(_patient);
                return await Task.FromResult((true, "Patient succesfully uptated"));
            }
        }

        public async Task<(bool Confirmed, string Response)> TransferToArchive(int id)
        {
            //Patient? _patient = await _patientRepository.GetPatientById(patient.Id);   
            var _patient = await _patientRepository.GetPatientById(id);

            if (_patient == null)
            {
                return await Task.FromResult((false, "Patient with given id does not exist."));
            }
            else
            {
                _patient.IsAvailable = false;
                //Patient r = _mapper.Map<Patient>(patient);
                var p = await _patientRepository.UpdatePatient(_patient);
                return await Task.FromResult((true, "Patient succesfully uptated"));
            }
        }



        public async Task<(bool Confirmed, string Response)> DeletePatient(int id)
        {
            var patient = await _patientRepository.GetPatientById(id);
            if (patient == null) return await Task.FromResult((false, "Patient with given id does not exist."));
            else
            {
                await _patientRepository.DeletePatient(id);
                return await Task.FromResult((true, "Patient successfully deleted."));
            }
        }
    }
}
