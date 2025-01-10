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

        public async Task<bool> IsSpecilityWithTheSameName(string name)
        {
            if (_context.MedicalSpecialisation.Any(p => p.Name == name))
            {
                return true;
            }
            return false;
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

        public async Task<List<MedicalSpecialisation>> GetAllAvailableMedicalSpecialisations()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                 new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                 TransactionScopeAsyncFlowOption.Enabled);
            List<MedicalSpecialisation> medicalSpecialisations = new List<MedicalSpecialisation>();
            try
            {
                medicalSpecialisations = await _context.MedicalSpecialisation.Where(r => r.IsAvailable == true).
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
            /*var _medicalSpecialisation = _context.MedicalSpecialisation.
                FirstOrDefault(p => p.Id == medicalSpecialisation.Id);

            if (_medicalSpecialisation == null)
            {
                return null;
            }*/
            try
            {
                _context.MedicalSpecialisation.Update(medicalSpecialisation);
                await _context.SaveChangesAsync();

                /*_medicalSpecialisation.Name = medicalSpecialisation.Name;
                _context.SaveChanges(); */
            }
            catch (Exception ex)
            { // Obsłuż wyjątek
                throw new Exception("An error occurred while updating the medical specialisation.", ex);
            }
            return medicalSpecialisation;
        }
        
        public async Task<bool> DeleteMedicalSpecialisation(int id)
        {
            var _medicalSpecialisation = await _context.MedicalSpecialisation.FindAsync(id);
            if (_medicalSpecialisation == null) return false;

            _context.MedicalSpecialisation.Remove(_medicalSpecialisation);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsLinkedToDoctor(int specialisationId)
        {

            return await _context.Doctor.AnyAsync(d => d.MedicalSpecialisations.Any(s => s.Id == specialisationId));

        }

        public async Task<bool> CanArchiveSpecialisation(int specialisationId)
        {
            // Sprawdzenie, czy istnieje wpis w tabeli wspólnej z lekarzem, który jest dostępny
            return !await _context.Doctor
                .Where(d => d.IsAvailable == true)
                .AnyAsync(d => d.MedicalSpecialisations.Any(ms => ms.Id == specialisationId));
        }



    }
}
