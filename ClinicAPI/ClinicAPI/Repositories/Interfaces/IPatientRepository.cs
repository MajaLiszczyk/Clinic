using ClinicAPI.Models;

namespace ClinicAPI.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        public Task<Patient?> GetPatientByIdAsync(int id);
        public Task CreatePatientAsync(Patient patient);
    }
}
