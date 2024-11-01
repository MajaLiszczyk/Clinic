using ClinicAPI.Dtos;

namespace ClinicAPI.Services.Interfaces
{
    public interface IPatientService
    {
        public Task<ReturnPatientDto?> GetPatientAsync(int id);
        public Task<List<ReturnPatientDto>> GetAllPatientsAsync();
        public Task<(bool Confirmed, string Response)> CreatePatientAsync(CreatePatientDto request);
        public Task<(bool Confirmed, string Response)> UpdatePatientAsync(UpdatePatientDto request, int id);
        public Task<(bool Confirmed, string Response)> DeletePatientAsync(int id);
    }
}
