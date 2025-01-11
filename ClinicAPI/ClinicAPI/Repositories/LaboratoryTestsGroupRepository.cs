using ClinicAPI.DB;
using ClinicAPI.Models;
using ClinicAPI.Repositories.Interfaces;
using System.Numerics;

namespace ClinicAPI.Repositories
{
    public class LaboratoryTestsGroupRepository : ILaboratoryTestsGroupRepository
    {
        private readonly ApplicationDBContext _context;
        public LaboratoryTestsGroupRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<int> CreateLaboratoryTestsGroup(LaboratoryTestsGroup laboratoryTestsGroup)
        {
            await _context.AddAsync(laboratoryTestsGroup);
            await _context.SaveChangesAsync();
            return laboratoryTestsGroup.Id;
        }

    }
}
