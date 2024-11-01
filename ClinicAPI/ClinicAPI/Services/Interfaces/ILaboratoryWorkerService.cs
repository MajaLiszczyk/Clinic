using ClinicAPI.Dtos;

namespace ClinicAPI.Services.Interfaces
{
    public interface ILaboratoryWorkerService
    {
        public Task<ReturnLaboratoryWorkerDto?> GetLaboratoryWorkerAsync(int id);
        public Task<List<ReturnLaboratoryWorkerDto>> GetAllLaboratoryWorkersAsync();
        public Task<(bool Confirmed, string Response)> CreateLaboratoryWorkerAsync(CreateLaboratoryWorkerDto request);
        public Task<(bool Confirmed, string Response)> UpdateLaboratoryWorkerAsync(UpdateLaboratoryWorkerDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteLaboratoryWorkerAsync(int id);
    }
}
