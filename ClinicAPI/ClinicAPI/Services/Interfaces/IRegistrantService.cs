using ClinicAPI.Dtos;
using ClinicAPI.Models;

namespace ClinicAPI.Services.Interfaces
{
    public interface IRegistrantService
    {
        public Task<ReturnRegistrantDto?> GetRegistrant(int id);
        public Task<List<ReturnRegistrantDto>> GetAllRegistrants();
        public Task<(bool Confirmed, string Response, ReturnRegistrantDto? registrant)> CreateRegistrant(CreateRegistrantDto registrat);
        public Task<(bool Confirmed, string Response, ReturnRegistrantDto? registrant)> RegisterRegistrant(CreateRegisterRegistrantDto request);
        public Task<(bool Confirmed, string Response)> UpdateRegistrant(UpdateRegistrantDto request);
        public Task<(bool Confirmed, string Response)> TransferToArchive(int id);
        public Task<(bool Confirmed, string Response)> DeleteRegistrant(int id);
    }
}
