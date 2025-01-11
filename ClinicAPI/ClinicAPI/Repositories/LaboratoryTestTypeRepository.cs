using ClinicAPI.DB;
using ClinicAPI.Models;
using ClinicAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace ClinicAPI.Repositories
{
    public class LaboratoryTestTypeRepository : ILaboratoryTestTypeRepository
    {
        private readonly ApplicationDBContext _context;
        public LaboratoryTestTypeRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<LaboratoryTestType?> GetLaboratoryTestTypeById(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                       new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                       TransactionScopeAsyncFlowOption.Enabled);
            LaboratoryTestType? testType = null;
            try
            {
                testType = await _context.LaboratoryTestType.Where(r => r.Id == id)
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
        
        public async Task<List<LaboratoryTestType>> GetAllLaboratoryTestTypes()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);  
            List<LaboratoryTestType> testTypes = new List<LaboratoryTestType>();
            try
            {
                testTypes = await _context.LaboratoryTestType.
                    ToListAsync(); //JESLI BRAK WYNIKOW- ZWROCI PUSTA LISTE
                scope.Complete();
            }
            catch (Exception) { }
            return testTypes;

        }

        public async Task<bool> IsLaboratoryTestTypeWithTheSameName(string name)
        {
            if (_context.LaboratoryTestType.Any(p => p.Name == name))
            {
                return true;
            }
            return false;
        }

        public async Task<LaboratoryTestType> CreateLaboratoryTestType(LaboratoryTestType type)
        {
            await _context.AddAsync(type);
            await _context.SaveChangesAsync();
            return type;
        }
        
        public async Task<LaboratoryTestType?> UpdateLaboratoryTestType(LaboratoryTestType testType)
        {
            _context.LaboratoryTestType.Update(testType);
            await _context.SaveChangesAsync();
            return testType;
        }
        
        public async Task<bool> DeleteLaboratoryTestType(int id)
        {
            var _testType = await _context.LaboratoryTestType.FindAsync(id);
            if (_testType == null) return false;

            _context.LaboratoryTestType.Remove(_testType);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
