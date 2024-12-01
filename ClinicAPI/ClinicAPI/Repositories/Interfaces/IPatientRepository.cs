using ClinicAPI.Dtos;
using ClinicAPI.Models;

namespace ClinicAPI.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        public Task<Patient?> GetPatientById(int id);
        public Task<List<Patient>> GetAllPatients(); 
        public Task<Patient> CreatePatient(Patient patient);
        public Task<Patient?> UpdatePatient(Patient patient);
        public Task<bool> DeletePatient(int id);

    }
}
