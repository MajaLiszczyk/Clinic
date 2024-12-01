using ClinicAPI.Dtos;

namespace ClinicAPI.Services.Interfaces
{
    public interface IDiagnosticTestService
    {
        public Task<ReturnDiagnosticTestDto?> GetDiagnosticTest(int id);
        public Task<List<ReturnDiagnosticTestDto>> GetAllDiagnosticTests();
        public Task<(bool Confirmed, string Response, ReturnDiagnosticTestDto? diagnosticTest)> CreateDiagnosticTest(CreateDiagnosticTestDto request);
        public Task<(bool Confirmed, string Response)> UpdateDiagnosticTest(UpdateDiagnosticTestDto request);
        public Task<(bool Confirmed, string Response)> DeleteDiagnosticTest(int id);
    }
}
