using ClinicAPI.Dtos;
using ClinicAPI.Models;

namespace ClinicAPI.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        public Task<Patient?> GetPatientById(int id);
        public Task<Patient?> GetPatientByUserId(string id);
        public Task<bool> GetPatientWithTheSamePesel(string pesel);
        public Task<List<Patient>> GetAllPatients(); 
        public Task<List<Patient>> GetAllAvailablePatients(); 
        public Task<Patient> CreatePatient(Patient patient);
        //public Task<Patient> RegisterPatient(Patient patient);
        public Task<Patient?> UpdatePatient(Patient patient);
        public Task<bool> DeletePatient(int id);

    }
}
