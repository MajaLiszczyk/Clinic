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

        //Zwraca wszystkie testy dla danej wizyty medycznej (dla jednej wizyty zawsze jest jedna grupa).
        public async Task<List<LaboratoryTest>> GetLaboratoryTestsByMedicalAppointmentId(int medicalAppointmentId)
        {
            // Pobierz ID grupy powiązanej z wizytą
            var groupId = await _context.LaboratoryTestsGroup
                .Where(group => group.MedicalAppointmentId == medicalAppointmentId)
                .Select(group => group.Id)
                .FirstOrDefaultAsync();

            if (groupId == 0) // Jeśli brak grupy, zwróć pustą listę
                return new List<LaboratoryTest>();

            // Pobierz testy laboratoryjne powiązane z grupą
            var tests = await _context.LaboratoryTest
                .Where(test => test.LaboratoryTestsGroupId == groupId)
                .ToListAsync();

            return tests;

            //podejście z join
            /*var tests = await (from LaboratoryTest in _context.LaboratoryTest
                               join group in _context.LaboratoryTestsGroup
                                   on test.LaboratoryTestsGroupId equals group.Id
                               where group.MedicalAppointmentId == medicalAppointmentId
                       select test).ToListAsync();

            return tests; */


            /*var tests = await _context.LaboratoryTest
                .Where(test => test.LaboratoryTestsGroup.MedicalAppointmentId == medicalAppointmentId)
                .Include(test => test.State) // Ładowanie stanu testu
                .ToListAsync();

            return tests;*/
        }

        //Zwraca listę testów dla danego pacjenta, pogrupowanych według wizyt (lub grup).
        //Zwracamy grupy jako kolekcję IGrouping<int, LaboratoryTest>, gdzie kluczem jest MedicalAppointmentId, a wartością lista testów.
        //public async Task<List<LaboratoryTest>> GetLaboratoryTestsByPatientId(int patientId)
        public async Task<List<IGrouping<int, LaboratoryTest>>> GetLaboratoryTestsByPatientId(int patientId)
        {
            // Pobierz ID wizyt pacjenta
            var medicalAppointmentIds = await _context.MedicalAppointment
                .Where(appointment => appointment.PatientId == patientId)
                .Select(appointment => appointment.Id)
                .ToListAsync();

            // Pobierz ID grup testów powiązanych z tymi wizytami
            var groupIds = await _context.LaboratoryTestsGroup
                .Where(group => medicalAppointmentIds.Contains(group.MedicalAppointmentId))
                .Select(group => group.Id)
                .ToListAsync();

            // Pobierz testy laboratoryjne powiązane z grupami
            var tests = await _context.LaboratoryTest
                .Where(test => groupIds.Contains(test.LaboratoryTestsGroupId))
                .ToListAsync();

            // Pogrupuj testy według ID wizyt
            var groupedTests = tests.GroupBy(test =>
            {
                var group = _context.LaboratoryTestsGroup.FirstOrDefault(g => g.Id == test.LaboratoryTestsGroupId);
                return group?.MedicalAppointmentId ?? 0;
            }).ToList();

            return groupedTests;

            /*var groupedTests = await _context.LaboratoryTest
                .Where(test => test.LaboratoryTestsGroup.MedicalAppointment.PatientId == patientId)
                .Include(test => test.State) // Ładowanie stanu testu
                .GroupBy(test => test.LaboratoryTestsGroup.MedicalAppointmentId)
                .ToListAsync();

            return groupedTests;*/
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
                //_laboratoryTest.date = laboratoryTest.date;
                _laboratoryTest.LaboratoryTestTypeId = laboratoryTest.LaboratoryTestTypeId;
                //_laboratoryTest.SupervisorId = laboratoryTest.SupervisorId;
                //_laboratoryTest.LaboratoryWorkerId = laboratoryTest.LaboratoryWorkerId;
                //_laboratoryTest.MedicalAppointmentId = laboratoryTest.MedicalAppointmentId;
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
