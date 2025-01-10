using ClinicAPI.Dtos;
using ClinicAPI.Models;

namespace ClinicAPI.Services.Interfaces
{
    public interface IPatientService
    {
        public Task<ReturnPatientDto?> GetPatient(int id);
        public Task<ReturnPatientDto?> GetPatientByUserId(string id);
        public Task<List<ReturnPatientDto>> GetAllPatients();
        public Task<List<ReturnPatientDto>> GetAllAvailablePatients();
        public Task<(bool Confirmed, string Response, ReturnPatientDto? patient)> CreatePatient(CreatePatientDto patient);
        public Task<(bool Confirmed, string Response, ReturnPatientDto? patient)> RegisterPatient(CreateRegisterPatientDto patient);
        public Task<(bool Confirmed, string Response)> UpdatePatient(UpdatePatientDto patient);
        public Task<(bool Confirmed, string Response)> TransferToArchive(int id);
        public Task<(bool Confirmed, string Response)> DeletePatient(int id);
    }
}
