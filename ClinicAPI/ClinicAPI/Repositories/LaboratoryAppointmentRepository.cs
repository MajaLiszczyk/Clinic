using ClinicAPI.DB;
using ClinicAPI.Models;
using ClinicAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace ClinicAPI.Repositories
{
    public class LaboratoryAppointmentRepository : ILaboratoryAppointmentRepository
    {
        private readonly ApplicationDBContext _context;
        public LaboratoryAppointmentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<LaboratoryAppointment?> GetLaboratoryAppointmentById(int id)
        {
            return await _context.LaboratoryAppointment.Where(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<LaboratoryAppointment>> GetAllLaboratoryAppointments()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            List<LaboratoryAppointment> laboratoryAppointments = new List<LaboratoryAppointment>();
            try
            {
                laboratoryAppointments = await _context.LaboratoryAppointment.
                    ToListAsync();
                scope.Complete();
            }
            catch (Exception) { }
            return laboratoryAppointments;
        }


        public async Task<LaboratoryAppointment> CreateLaboratoryAppointment(LaboratoryAppointment laboratoryAppointment)
        {
            await _context.AddAsync(laboratoryAppointment);
            laboratoryAppointment.DateTime = laboratoryAppointment.DateTime.ToUniversalTime();
            await _context.SaveChangesAsync();

            return laboratoryAppointment;

        }

        public async Task<LaboratoryAppointment?> UpdateLaboratoryAppointment(LaboratoryAppointment laboratoryAppointment)
        {
            _context.LaboratoryAppointment.Update(laboratoryAppointment);
            await _context.SaveChangesAsync();
            return laboratoryAppointment;
        }
    }
}
