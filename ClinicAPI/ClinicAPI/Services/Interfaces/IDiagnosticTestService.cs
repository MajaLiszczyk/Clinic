using ClinicAPI.Dtos;

namespace ClinicAPI.Services.Interfaces
{
    public interface IDiagnosticTestService
    {
        public Task<ReturnDiagnosticTestDto?> GetDiagnosticTestAsync(int id);
        public Task<List<ReturnDiagnosticTestDto>> GetAllDiagnosticTestsAsync();
        public Task<(bool Confirmed, string Response)> CreateDiagnosticTestAsync(CreateDiagnosticTestDto request);
        public Task<(bool Confirmed, string Response)> UpdateDiagnosticTestAsync(UpdateDiagnosticTestDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteDiagnosticTestAsync(int id);
    }
}
