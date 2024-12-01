using ClinicAPI.Dtos;
using ClinicAPI.Models;

namespace ClinicAPI.Services.Interfaces
{
    public interface IPatientService
    {
        //public Task<ReturnPatientDto?> GetPatientAsync(int id);
        public Task<ReturnPatientDto?> GetPatient(int id);
        //public Task<List<ReturnPatientDto>> GetAllPatientsAsync();
        public Task<List<ReturnPatientDto>> GetAllPatients();
        public Task<(bool Confirmed, string Response, ReturnPatientDto? patient)> CreatePatient(CreatePatientDto patient);
        //public Task<(bool Confirmed, string Response)> UpdatePatientAsync(UpdatePatientDto request, int id);
        public Task<(bool Confirmed, string Response)> UpdatePatient(UpdatePatientDto patient);
        public Task<(bool Confirmed, string Response)> DeletePatient(int id);
    }
}
