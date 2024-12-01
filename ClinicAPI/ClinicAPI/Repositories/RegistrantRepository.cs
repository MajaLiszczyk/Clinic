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
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                        new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                        TransactionScopeAsyncFlowOption.Enabled);
            Registrant? registrant = null;
            try
            {
                registrant = await _context.Registrant.Where(r => r.Id == id)
                            .FirstOrDefaultAsync();
                scope.Complete();
            }
            catch (Exception){}
            return registrant;
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
            try
            {
                _registrant.Name = registrant.Name;
                _registrant.Surname = registrant.Surname;

                _context.SaveChanges();

            }
            catch (Exception ex){}
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
