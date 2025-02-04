using ClinicAPI.DB;
using ClinicAPI.Models;
using ClinicAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace ClinicAPI.Repositories
{
    public class RegistrantRepository : IRegistrantRepository
    {
        private readonly ApplicationDBContext _context;
        public RegistrantRepository(ApplicationDBContext context)
        {
            _context = context;
        }



        public async Task<Registrant?> GetRegistrantById(int id)
        {
            Registrant? registrant = null;
                registrant = await _context.Registrant.Where(r => r.Id == id)
                            .FirstOrDefaultAsync();
            return registrant;
        }

        public async Task<bool> GetRegistrantWithTheSameNumber(string number)
        {
            if (_context.Registrant.Any(p => p.RegistrantNumber == number))
            {
                return true;
            }
            return false;
        }

        public async Task<List<Registrant>> GetAllRegistrants()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);
            List<Registrant> registrants = new List<Registrant>();
            try
            {
                registrants = await _context.Registrant.
                    ToListAsync(); 
                scope.Complete();
            }
            catch (Exception) { }
            return registrants;
        }

        public async Task<Registrant> CreateRegistrant(Registrant registrant)
        {
            await _context.AddAsync(registrant);
            await _context.SaveChangesAsync();
            return registrant;
        }

        public async Task<Registrant?> UpdateRegistrant(Registrant registrant)
        {
            Registrant _registrant = _context.Registrant.
               FirstOrDefault(p => p.Id == registrant.Id);

            if (_registrant == null)
            {
                return null;
            }
                _registrant.Name = registrant.Name;
                _registrant.Surname = registrant.Surname;

                _context.SaveChanges();
            return _registrant;
        }

        public async Task<bool> DeleteRegistrant(int id)
        {
            var _registrant = await _context.Registrant.FindAsync(id);
            if (_registrant == null) return false;

            _context.Registrant.Remove(_registrant);
            await _context.SaveChangesAsync();
            return true;
        }

        
    }
}
