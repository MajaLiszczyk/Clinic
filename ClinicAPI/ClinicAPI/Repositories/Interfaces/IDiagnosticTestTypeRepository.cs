using ClinicAPI.Models;

namespace ClinicAPI.Repositories.Interfaces
{
    public interface IDiagnosticTestTypeRepository
    {
        public Task<DiagnosticTestType?> GetDiagnosticTestTypeById(int id);
        public Task<bool> IsDiagnosticTestTypeWithTheSameName(string name);
        public Task<List<DiagnosticTestType>> GetAllDiagnosticTestTypes();
        public Task<List<DiagnosticTestType>> GetAllAvailableDiagnosticTestTypes();
        public Task<DiagnosticTestType> CreateDiagnosticTestType(DiagnosticTestType type);
        public Task<DiagnosticTestType?> UpdateDiagnosticTestType(DiagnosticTestType type);
        public Task<bool> DeleteDiagnosticTestType(int id);
        public Task<bool> IsUsedInTests(int id);
    }
}
