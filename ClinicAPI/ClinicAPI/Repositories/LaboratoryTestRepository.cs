using ClinicAPI.DB;
using ClinicAPI.Models;
using ClinicAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace ClinicAPI.Repositories
{
    public class LaboratoryTestRepository : ILaboratoryTestRepository
    {
        private readonly ApplicationDBContext _context;
        public LaboratoryTestRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<LaboratoryTest?> GetLaboratoryTestById(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                        new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                        TransactionScopeAsyncFlowOption.Enabled);
            LaboratoryTest? laboratoryTest = null;
            try
            {
                laboratoryTest = await _context.LaboratoryTest.Where(r => r.Id == id)
                            .FirstOrDefaultAsync(); //zwróci null, jesli brak wynikow

                scope.Complete();
            }
            catch (Exception){}
            return laboratoryTest;

        }
        public async Task<List<LaboratoryTest>> GetAllLaboratoryTests()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled); 
            List<LaboratoryTest> laboratoryTests = new List<LaboratoryTest>();
            try
            {
                laboratoryTests = await _context.LaboratoryTest.
                    ToListAsync(); 
                scope.Complete();
            }
            catch (Exception) { }
            return laboratoryTests;
        }
        public async Task<LaboratoryTest> CreateLaboratoryTest(LaboratoryTest laboratoryTest)
        {
            await _context.AddAsync(laboratoryTest);
            await _context.SaveChangesAsync();
            return laboratoryTest;
        }
        public async Task<LaboratoryTest?> UpdateLaboratoryTest(LaboratoryTest laboratoryTest)
        {
            LaboratoryTest _laboratoryTest = _context.LaboratoryTest.
               FirstOrDefault(p => p.Id == laboratoryTest.Id);

            if (_laboratoryTest == null)
            {
                return null;
                //brak pacjenta
            }
            try
            {
                _laboratoryTest.date = laboratoryTest.date;
                _laboratoryTest.LaboratoryTestTypeId = laboratoryTest.LaboratoryTestTypeId;
                _laboratoryTest.SupervisorId = laboratoryTest.SupervisorId;
                _laboratoryTest.LaboratoryWorkerId = laboratoryTest.LaboratoryWorkerId;
                _laboratoryTest.MedicalAppointmentId = laboratoryTest.MedicalAppointmentId;
                _laboratoryTest.DoctorNote = laboratoryTest.DoctorNote;

                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                //wyjatek
            }
            return _laboratoryTest;
        }
        public async Task<bool> DeleteLaboratoryTest(int id)
        {
            var _laboratoryTest = await _context.LaboratoryTest.FindAsync(id);
            if (_laboratoryTest == null) return false;

            _context.LaboratoryTest.Remove(_laboratoryTest);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
