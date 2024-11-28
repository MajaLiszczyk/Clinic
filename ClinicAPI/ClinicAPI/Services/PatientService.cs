using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Services.Interfaces;

namespace ClinicAPI.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;

        }


       // public Task<ReturnPatientDto?> GetPatientAsync(int id)
        public async Task<Patient?> GetPatientAsync(int id)
        {
            var patient = await _patientRepository.GetPatientByIdAsync(id);
            return patient;

        }
        /*public Task<List<ReturnPatientDto>> GetAllPatientsAsync()
        {

        }*/
        //public Task<(bool Confirmed, string Response, Patient? patient)> CreatePatientAsync(CreatePatientDto patient)
        public async Task<(bool Confirmed, string Response, Patient? patient)> CreatePatientAsync(Patient patient)
        {
            var _patient = new Patient
            {
                Pesel = patient.Pesel,
                Name = patient.Name,
                Surname = patient.Surname,
            };
            await _patientRepository.CreatePatientAsync(_patient);

            return await Task.FromResult((true, "Patient successfully created.", patient));

        }
        /*public Task<(bool Confirmed, string Response)> UpdatePatientAsync(UpdatePatientDto request, int id)
        {

        }
        public Task<(bool Confirmed, string Response)> DeletePatientAsync(int id)
        {

        }*/
    }
}
