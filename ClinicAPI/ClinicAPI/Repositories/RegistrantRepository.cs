using ClinicAPI.DB;
using ClinicAPI.Models;
using ClinicAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicAPI.Repositories
{
    public class RegistrantRepository : IRegistrantRepository
    {
        private readonly ApplicationDBContext _context;
        public RegistrantRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Registrant?> GetRegistrantByIdAsync(int id)
        {
            Registrant? registrant = null;
            try
            {
                registrant = await _context.Registrant.Where(r => r.Id == id)
                            .FirstOrDefaultAsync();

                //scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => registrant);
        }

        public async Task CreateRegistrantAsync(Registrant registrant)
        {
            await _context.AddAsync(registrant);
            await _context.SaveChangesAsync();
        }




    }
}
