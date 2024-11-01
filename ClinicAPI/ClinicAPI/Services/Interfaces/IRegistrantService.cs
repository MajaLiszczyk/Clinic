using ClinicAPI.Dtos;

namespace ClinicAPI.Services.Interfaces
{
    public interface IRegistrantService
    {
        public Task<ReturnRegistrantDto?> GetRegistrantAsync(int id);
        public Task<List<ReturnRegistrantDto>> GetAllRegistrantsAsync();
        public Task<(bool Confirmed, string Response)> CreateRegistrantAsync(CreateRegistrantDto request);
        public Task<(bool Confirmed, string Response)> UpdateRegistrantAsync(UpdateRegistrantDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteRegistrantAsync(int id);
    }
}
