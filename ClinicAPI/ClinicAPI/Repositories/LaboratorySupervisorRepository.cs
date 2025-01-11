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


        public async Task<bool> GetLaboratorySupervisorWithTheSameNumber(string number)
        {
            if (_context.LaboratorySupervisor.Any(p => p.LaboratorySupervisorNumber == number))
            {
                return true;
            }
            return false;
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

        public async Task<List<LaboratorySupervisor>> GetAllAvailableLAboratorySupervisors()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            List<LaboratorySupervisor> laboratorySupervisors = new List<LaboratorySupervisor>();
            try
            {
                laboratorySupervisors = await _context.LaboratorySupervisor.Where(r => r.IsAvailable == true)
                    .ToListAsync();
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
            _context.LaboratorySupervisor.Update(laboratorySupervisor);
            await _context.SaveChangesAsync();
            return laboratorySupervisor;          
        }

        public async Task<bool> DeleteLaboratorySupervisor(int id)
        {
            var _laboratorySupervisor = await _context.LaboratorySupervisor.FindAsync(id);
            if (_laboratorySupervisor == null) return false;

            _context.LaboratorySupervisor.Remove(_laboratorySupervisor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CanArchiveLaboratorySupervisor(int laboratorySupervisorId)
        {
            // Sprawdzenie, czy laboratorySupervisor ma jakąkolwiek wizytę, która nie jest Cancelled ani Finished
            return !await _context.LaboratoryAppointment.AnyAsync(ma =>
                ma.SupervisorId == laboratorySupervisorId &&
                (ma.State != LaboratoryAppointmentState.Cancelled &&
                ma.State != LaboratoryAppointmentState.Finished));
        }

    }
}
