using ClinicAPI.Dtos;

namespace ClinicAPI.Services.Interfaces
{
    public interface IAdminService
    {
        public Task<ReturnAdminDto?> GetAdminAsync(int id);
        public Task<List<ReturnAdminDto>> GetAllAdminsAsync();
        public Task<(bool Confirmed, string Response)> CreateAdminAsync(CreateAdminDto request);
        public Task<(bool Confirmed, string Response)> UpdateAdminAsync(UpdateAdminDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteAdminAsync(int id);
    }
}
