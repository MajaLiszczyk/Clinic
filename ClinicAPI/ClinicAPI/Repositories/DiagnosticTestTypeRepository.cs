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
            DiagnosticTestType? testType = null;
            try
            {
                testType = await _context.DiagnosticTestType.Where(r => r.Id == id)
                            .FirstOrDefaultAsync(); //zwróci null, jesli brak wynikow
            }
            catch (Exception)
            {
                //_logger.LogError(ex, "Error occurred while fetching patient with ID {Id}", id);
            }
            return testType;
        }

        public async Task<bool> IsDiagnosticTestTypeWithTheSameName(string name)
        {
            if (_context.DiagnosticTestType.Any(p => p.Name == name))
            {
                return true;
            }
            return false;
        }


        public async Task<List<DiagnosticTestType>> GetAllDiagnosticTestTypes()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);  
            List<DiagnosticTestType> testTypes = new List<DiagnosticTestType>();
            try
            {
                testTypes = await _context.DiagnosticTestType.
                    ToListAsync(); 
                scope.Complete();
            }
            catch (Exception) { }
            return testTypes;
        }

        public async Task<List<DiagnosticTestType>> GetAllAvailableDiagnosticTestTypes()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled); 
            List<DiagnosticTestType> testTypes = new List<DiagnosticTestType>();
            try
            {
                testTypes = await _context.DiagnosticTestType.Where(r => r.IsAvailable == true).
                    ToListAsync(); 
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
            try
            {
                _context.DiagnosticTestType.Update(testType);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the medical specialisation.", ex);
            }
            return testType;
        }
        
        public async Task<bool> DeleteDiagnosticTestType(int id)
        {
            var _testType = await _context.DiagnosticTestType.FindAsync(id);
            if (_testType == null) return false;

            _context.DiagnosticTestType.Remove(_testType);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsUsedInTests(int testTypeId)
        {
            return await _context.DiagnosticTest.AnyAsync(t => t.DiagnosticTestTypeId == testTypeId);
        }

    }
}
