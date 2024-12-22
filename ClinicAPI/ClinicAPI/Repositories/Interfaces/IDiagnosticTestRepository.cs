using ClinicAPI.Dtos;
using ClinicAPI.Models;

namespace ClinicAPI.Repositories.Interfaces
{
    public interface IDiagnosticTestRepository
    {
        public Task<DiagnosticTest?> GetDiagnosticTestById(int id);
        public Task<List<DiagnosticTest>> GetAllDiagnosticTests();
        public Task<List<ReturnDiagnosticTestDto>> GetByMedicalAppointmentId(int id);
        public Task<DiagnosticTest> CreateDiagnosticTest(DiagnosticTest patient);
        public Task<DiagnosticTest?> UpdateDiagnosticTest(DiagnosticTest patient);
        public Task<bool> DeleteDiagnosticTest(int id);
    }
}
