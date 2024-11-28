using ClinicAPI.Dtos;
using ClinicAPI.Models;

namespace ClinicAPI.Services.Interfaces
{
    public interface IPatientService
    {
        //public Task<ReturnPatientDto?> GetPatientAsync(int id);
        public Task<Patient?> GetPatientAsync(int id);
        //public Task<List<ReturnPatientDto>> GetAllPatientsAsync();
        public Task<(bool Confirmed, string Response, Patient? patient)> CreatePatientAsync(Patient patient);
        //public Task<(bool Confirmed, string Response)> UpdatePatientAsync(UpdatePatientDto request, int id);
        //public Task<(bool Confirmed, string Response)> DeletePatientAsync(int id);
    }
}
