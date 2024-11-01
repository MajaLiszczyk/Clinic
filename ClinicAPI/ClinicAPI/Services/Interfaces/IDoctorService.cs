using ClinicAPI.Dtos;

namespace ClinicAPI.Services.Interfaces
{
    public interface IDoctorService
    {
        public Task<ReturnDoctorDto?> GetDoctorAsync(int id);
        public Task<List<ReturnDoctorDto>> GetAllDoctorsAsync();
        public Task<(bool Confirmed, string Response)> CreateDoctorAsync(CreateDoctorDto request);
        public Task<(bool Confirmed, string Response)> UpdateDoctorAsync(UpdateDoctorDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteDoctorAsync(int id);
    }
}
