using ClinicAPI.Dtos;
using ClinicAPI.Models;

namespace ClinicAPI.Services.Interfaces
{
    public interface IDiagnosticTestTypeService
    {
        public Task<DiagnosticTestType?> GetDiagnosticTestType(int id);
        public Task<List<DiagnosticTestType>> GetAllDiagnosticTestTypes();
        public Task<List<DiagnosticTestType>> GetAllAvailableDiagnosticTestTypes();
        public Task<(bool Confirmed, string Response, DiagnosticTestType? patient)> CreateDiagnosticTestType(DiagnosticTestType testType);
        public Task<(bool Confirmed, string Response)> UpdateDiagnosticTestType(DiagnosticTestType testType);
        public Task<(bool Confirmed, string Response)> TransferToArchive(int id);
        public Task<(bool Confirmed, string Response)> DeleteDiagnosticTestType(int id);
    }
}
