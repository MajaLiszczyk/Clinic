using ClinicAPI.DB;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace ClinicAPI.Repositories
{
    public class MedicalSpecialisationRepository : IMedicalSpecialisationRepository
    {
        private readonly ApplicationDBContext _context;
        public MedicalSpecialisationRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<MedicalSpecialisation?> GetMedicalSpecialisationById(int id)
        {

            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                       new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                       TransactionScopeAsyncFlowOption.Enabled);
            MedicalSpecialisation? medicalSpecialisation = null;
            try
            {
                medicalSpecialisation = await _context.MedicalSpecialisation.Where(r => r.Id == id)
                            .FirstOrDefaultAsync();

                scope.Complete();
            }
            catch (Exception) { }
            return medicalSpecialisation;
        }

        
        public async Task<List<MedicalSpecialisation>> GetAllMedicalSpecialisations()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                 new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                 TransactionScopeAsyncFlowOption.Enabled);
            List<MedicalSpecialisation> medicalSpecialisations = new List<MedicalSpecialisation>();
            try
            {
                medicalSpecialisations = await _context.MedicalSpecialisation.
                    ToListAsync();
                scope.Complete();
            }
            catch (Exception) { }
            return medicalSpecialisations;
        }
        
        public async Task<MedicalSpecialisation> CreateMedicalSpecialisation(MedicalSpecialisation medicalSpecialisation)
        {
            await _context.AddAsync(medicalSpecialisation);
            await _context.SaveChangesAsync();
            return medicalSpecialisation;
        }
        
        public async Task<MedicalSpecialisation?> UpdateMedicalSpecialisation(MedicalSpecialisation medicalSpecialisation)
        {
            var _medicalSpecialisation = _context.MedicalSpecialisation.
                FirstOrDefault(p => p.Id == medicalSpecialisation.Id);

            if (_medicalSpecialisation == null)
            {
                return null;
            }
            try
            {
                _medicalSpecialisation.Name = medicalSpecialisation.Name;
                _context.SaveChanges();
            }
            catch (Exception ex) { }
            return _medicalSpecialisation;
        }
        
        public async Task<bool> DeleteMedicalSpecialisation(int id)
        {
            var _medicalSpecialisation = await _context.MedicalSpecialisation.FindAsync(id);
            if (_medicalSpecialisation == null) return false;

            _context.MedicalSpecialisation.Remove(_medicalSpecialisation);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
