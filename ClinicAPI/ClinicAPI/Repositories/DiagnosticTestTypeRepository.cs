using ClinicAPI.DB;
using ClinicAPI.Models;
using ClinicAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace ClinicAPI.Repositories
{
    public class DiagnosticTestTypeRepository : IDiagnosticTestTypeRepository
    {
        private readonly ApplicationDBContext _context;
        public DiagnosticTestTypeRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<DiagnosticTestType?> GetDiagnosticTestTypeById(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                        new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                        TransactionScopeAsyncFlowOption.Enabled);
            DiagnosticTestType? testType = null;
            try
            {
                testType = await _context.DiagnosticTestType.Where(r => r.Id == id)
                            .FirstOrDefaultAsync(); //zwróci null, jesli brak wynikow

                scope.Complete();
            }
            catch (Exception)
            {
                //_logger.LogError(ex, "Error occurred while fetching patient with ID {Id}", id);
            }
            //return await Task.Run(() => patient);
            return testType;

        }
        
        public async Task<List<DiagnosticTestType>> GetAllDiagnosticTestTypes()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);  //KOD Z ESERVICE
            List<DiagnosticTestType> testTypes = new List<DiagnosticTestType>();
            try
            {
                testTypes = await _context.DiagnosticTestType.
                    ToListAsync(); //JESLI BRAK WYNIKOW- ZWROCI PUSTA LISTE
                scope.Complete();
            }
            catch (Exception) { }
            return testTypes;
        }
        
        public async Task<DiagnosticTestType> CreateDiagnosticTestType(DiagnosticTestType type)
        {
            await _context.AddAsync(type);
            await _context.SaveChangesAsync();
            return type;
        }
        
        public async Task<DiagnosticTestType?> UpdateDiagnosticTestType(DiagnosticTestType testType)
        {
            var _testType = _context.DiagnosticTestType.
               FirstOrDefault(p => p.Id == testType.Id);

            if (_testType == null)
            {
                return null;
            }
            try
            {
                _testType.Name = testType.Name;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                //wyjatek
            }
            return _testType;
        }
        
        public async Task<bool> DeleteDiagnosticTestType(int id)
        {
            var _testType = await _context.DiagnosticTestType.FindAsync(id);
            if (_testType == null) return false;

            _context.DiagnosticTestType.Remove(_testType);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
