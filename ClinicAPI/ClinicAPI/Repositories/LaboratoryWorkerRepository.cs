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

        public async Task<LaboratoryWorker?> GetLaboratoryWorkerByUserId(string userId)
        {
            var laboratoryWorker = await _context.LaboratoryWorker.Where(r => r.UserId == userId)
                            .FirstOrDefaultAsync();
            return laboratoryWorker;
        }

        public async Task<bool> GetLaboratoryWorkerWithTheSameNumber(string number)
        {
            if (_context.LaboratoryWorker.Any(p => p.LaboratoryWorkerNumber == number))
            {
                return true;
            }
            return false;
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

        public async Task<List<LaboratoryWorker>> GetAllAvailableLaboratoryWorkers()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            List<LaboratoryWorker> laboratoryWorkers = new List<LaboratoryWorker>();
            try
            {
                laboratoryWorkers = await _context.LaboratoryWorker.Where(r => r.IsAvailable == true)
                    .ToListAsync();
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
            _context.LaboratoryWorker.Update(laboratoryWorker);
            await _context.SaveChangesAsync();
            return laboratoryWorker;
        }

        public async Task<bool> DeleteLaboratoryWorker(int id)
        {
            var _laboratoryWorker = await _context.LaboratoryWorker.FindAsync(id);
            if (_laboratoryWorker == null) return false;

            _context.LaboratoryWorker.Remove(_laboratoryWorker);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CanArchiveLaboratoryWorker(int laboratoryWorkerId)
        {
            return !await _context.LaboratoryAppointment.AnyAsync(ma =>
                ma.LaboratoryWorkerId == laboratoryWorkerId &&
                (ma.State != LaboratoryAppointmentState.Cancelled &&
                ma.State != LaboratoryAppointmentState.Finished));
        }
    }
}
