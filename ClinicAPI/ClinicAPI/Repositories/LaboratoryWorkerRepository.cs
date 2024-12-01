using ClinicAPI.DB;
using ClinicAPI.Models;
using ClinicAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace ClinicAPI.Repositories
{
    public class LaboratoryWorkerRepository : ILaboratoryWorkerRepository
    {
        private readonly ApplicationDBContext _context;
        public LaboratoryWorkerRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<LaboratoryWorker?> GetLaboratoryWorkerById(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                       new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                       TransactionScopeAsyncFlowOption.Enabled);
            LaboratoryWorker? laboratoryWorker = null;
            try
            {
                laboratoryWorker = await _context.LaboratoryWorker.Where(r => r.Id == id)
                            .FirstOrDefaultAsync();

                scope.Complete();
            }
            catch (Exception){}
            return laboratoryWorker;
        }

        public async Task<List<LaboratoryWorker>> GetAllLaboratoryWorkers()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            List<LaboratoryWorker> laboratoryWorkers = new List<LaboratoryWorker>();
            try
            {
                laboratoryWorkers = await _context.LaboratoryWorker.
                    ToListAsync();
                scope.Complete();
            }
            catch (Exception) { }
            return laboratoryWorkers;
        }

        public async Task<LaboratoryWorker> CreateLaboratoryWorker(LaboratoryWorker laboratoryWorker)
        {
            await _context.AddAsync(laboratoryWorker);
            await _context.SaveChangesAsync();
            return laboratoryWorker;
        }

        public async Task<LaboratoryWorker?> UpdateLaboratoryWorker(LaboratoryWorker laboratoryWorker)
        {
            var _laboratoryWorker = _context.LaboratoryWorker.
                FirstOrDefault(p => p.Id == laboratoryWorker.Id);

            if (_laboratoryWorker == null)
            {
                return null;
            }
            try
            {
                _laboratoryWorker.Surname = laboratoryWorker.Surname;
                _laboratoryWorker.Name = laboratoryWorker.Name;

                _context.SaveChanges();

            }
            catch (Exception ex){}
            return _laboratoryWorker;
        }

        public async Task<bool> DeleteLaboratoryWorker(int id)
        {
            var _laboratoryWorker = await _context.LaboratoryWorker.FindAsync(id);
            if (_laboratoryWorker == null) return false;

            _context.LaboratoryWorker.Remove(_laboratoryWorker);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
