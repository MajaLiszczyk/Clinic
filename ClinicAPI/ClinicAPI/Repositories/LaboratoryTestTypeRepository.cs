using ClinicAPI.DB;
using ClinicAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace ClinicAPI.Repositories
{
    public class LaboratoryTestTypeRepository
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
                                                   TransactionScopeAsyncFlowOption.Enabled);  //KOD Z ESERVICE
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
        
        public async Task<LaboratoryTestType> CreateLaboratoryTestType(LaboratoryTestType type)
        {
            await _context.AddAsync(type);
            await _context.SaveChangesAsync();
            return type;
        }
        
        public async Task<LaboratoryTestType?> UpdateLaboratoryTestType(LaboratoryTestType testType)
        {
            var _testType = _context.LaboratoryTestType.
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
