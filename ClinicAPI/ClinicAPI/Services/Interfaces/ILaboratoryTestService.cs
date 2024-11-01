using ClinicAPI.Dtos;

namespace ClinicAPI.Services.Interfaces
{
    public interface ILaboratoryTestService
    {
        public Task<ReturnLaboratoryTestDto?> GetLaboratoryTestAsync(int id);
        public Task<List<ReturnLaboratoryTestDto>> GetAllLaboratoryTestsAsync();
        public Task<(bool Confirmed, string Response)> CreateLaboratoryTestAsync(CreateLaboratoryTestDto request);
        public Task<(bool Confirmed, string Response)> UpdateLaboratoryTestAsync(UpdateLaboratoryTestDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteLaboratoryTestAsync(int id);
    }
}
