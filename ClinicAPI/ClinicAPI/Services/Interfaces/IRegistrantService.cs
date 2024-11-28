using ClinicAPI.Dtos;
using ClinicAPI.Models;

namespace ClinicAPI.Services.Interfaces
{
    public interface IRegistrantService
    {
        public Task<Registrant?> GetRegistrantAsync(int id);
        public Task<(bool Confirmed, string Response, Registrant? registrant)> CreateRegistrantAsync(Registrant registrat);
        /*public Task<List<ReturnRegistrantDto>> GetAllRegistrantsAsync();
        public Task<(bool Confirmed, string Response)> CreateRegistrantAsync(CreateRegistrantDto request);
        public Task<(bool Confirmed, string Response)> UpdateRegistrantAsync(UpdateRegistrantDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteRegistrantAsync(int id);*/
    }
}
