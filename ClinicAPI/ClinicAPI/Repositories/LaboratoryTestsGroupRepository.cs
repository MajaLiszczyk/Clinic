using ClinicAPI.DB;
using ClinicAPI.Models;
using ClinicAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Transactions;

namespace ClinicAPI.Repositories
{
    public class LaboratoryTestsGroupRepository : ILaboratoryTestsGroupRepository
    {
        private readonly ApplicationDBContext _context;
        public LaboratoryTestsGroupRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<LaboratoryTestsGroup?> getGroupByLabAppId(int id)
        {
            return await _context.LaboratoryTestsGroup.Where(r => r.LaboratoryAppointmentId == id).FirstOrDefaultAsync();
        }

        public async Task<int> CreateLaboratoryTestsGroup(LaboratoryTestsGroup laboratoryTestsGroup)
        {
            await _context.AddAsync(laboratoryTestsGroup);
            await _context.SaveChangesAsync();
            return laboratoryTestsGroup.Id;
        }

        public async Task<int> UpdateLaboratoryTestsGroup(LaboratoryTestsGroup laboratoryTestsGroup)
        {

            _context.LaboratoryTestsGroup.Update(laboratoryTestsGroup);
            await _context.SaveChangesAsync();
            return laboratoryTestsGroup.Id;
        }

        public async Task<LaboratoryTestsGroup?> GetTestsGroupById(int id)
        {
            return await _context.LaboratoryTestsGroup.Where(r => r.Id == id).FirstOrDefaultAsync();
        }      
    }
}
