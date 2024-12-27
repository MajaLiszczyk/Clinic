using AutoMapper;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Services.Interfaces;
using System.Numerics;

namespace ClinicAPI.Services
{
    //SERWIS powinien przygotować dane w postaci odpowiedniej dla kontrolera (dto) - mapuje encje odebrane z repo na dto. 
    //Obsługuje sytuacje, w których wynik zapytania jest null, ale nie powinien jeszcze zwracać kodów HTTP (to rola kontrolera).
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public PatientService(IPatientRepository patientRepository, IMapper mapper)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;

        }


       // public Task<ReturnPatientDto?> GetPatientAsync(int id)
        public async Task<ReturnPatientDto?> GetPatient(int id)
        {
            var patient = await _patientRepository.GetPatientById(id);
            return _mapper.Map<ReturnPatientDto>(patient);
        }

        public async Task<List<ReturnPatientDto>> GetAllPatients()
        {
            var patients = await _patientRepository.GetAllPatients();
            return _mapper.Map<List<ReturnPatientDto>>(patients);

            //return _mapper.Map<List<ReturnMessageDto>>(messages);

        }
        //public Task<(bool Confirmed, string Response, Patient? patient)> CreatePatientAsync(CreatePatientDto patient)
        //TO DO : PRZY TWRZENIEU PESEL NIE MOZE SIE POWTORZYC W BAZIE
        public async Task<(bool Confirmed, string Response, ReturnPatientDto? patient)> CreatePatient(CreatePatientDto patient) 
        {
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
        public async Task<(bool Confirmed, string Response)> UpdatePatient(UpdatePatientDto patient)
        {
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
