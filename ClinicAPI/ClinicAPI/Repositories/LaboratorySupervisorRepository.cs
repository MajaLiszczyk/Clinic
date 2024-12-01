using ClinicAPI.DB;
using ClinicAPI.Models;
using ClinicAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace ClinicAPI.Repositories
{
    public class LaboratorySupervisorRepository : ILaboratorySupervisorRepository
    {
        private readonly ApplicationDBContext _context;
        public LaboratorySupervisorRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<LaboratorySupervisor?> GetLaboratorySupervisorById(int id)
        {

            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                       new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                       TransactionScopeAsyncFlowOption.Enabled);
            LaboratorySupervisor? laboratorySupervisor = null;
            try
            {
                laboratorySupervisor = await _context.LaboratorySupervisor.Where(r => r.Id == id)
                            .FirstOrDefaultAsync();

                scope.Complete();
            }
            catch (Exception) { }
            return laboratorySupervisor;
        }

        public async Task<List<LaboratorySupervisor>> GetAllLaboratorySupervisors()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                  new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                  TransactionScopeAsyncFlowOption.Enabled);
            List<LaboratorySupervisor> laboratorySupervisors = new List<LaboratorySupervisor>();
            try
            {
                laboratorySupervisors = await _context.LaboratorySupervisor.
                    ToListAsync();
                scope.Complete();
            }
            catch (Exception) { }
            return laboratorySupervisors;
        }

        public async Task<LaboratorySupervisor> CreateLaboratorySupervisor(LaboratorySupervisor laboratorySupervisor)
        {
            await _context.AddAsync(laboratorySupervisor);
            await _context.SaveChangesAsync();
            return laboratorySupervisor;
        }

        public async Task<LaboratorySupervisor?> UpdateLaboratorySupervisor(LaboratorySupervisor laboratorySupervisor)
        {
            var _laboratorySupervisor = _context.LaboratorySupervisor.
                FirstOrDefault(p => p.Id == laboratorySupervisor.Id);

            if (_laboratorySupervisor == null)
            {
                return null;
            }
            try
            {
                _laboratorySupervisor.Surname = laboratorySupervisor.Surname;
                _laboratorySupervisor.Name = laboratorySupervisor.Name;

                _context.SaveChanges();

            }
            catch (Exception ex) { }
            return _laboratorySupervisor;
        }

        public async Task<bool> DeleteLaboratorySupervisor(int id)
        {
            var _laboratorySupervisor = await _context.LaboratorySupervisor.FindAsync(id);
            if (_laboratorySupervisor == null) return false;

            _context.LaboratorySupervisor.Remove(_laboratorySupervisor);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
