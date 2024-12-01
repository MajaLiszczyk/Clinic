using ClinicAPI.Dtos;

namespace ClinicAPI.Services.Interfaces
{
    public interface IDoctorService
    {
        public Task<ReturnDoctorDto?> GetDoctor(int id);
        public Task<List<ReturnDoctorDto>> GetAllDoctors();
        public Task<(bool Confirmed, string Response, ReturnDoctorDto? doctor)> CreateDoctor(CreateDoctorDto request);
        public Task<(bool Confirmed, string Response)> UpdateDoctor(UpdateDoctorDto request);
        public Task<(bool Confirmed, string Response)> DeleteDoctor(int id);
    }
}
