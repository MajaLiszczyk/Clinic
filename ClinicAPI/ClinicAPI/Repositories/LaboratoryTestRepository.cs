using ClinicAPI.DB;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.RegularExpressions;
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
            // Jeśli brak grupy, zwróć pustą listę
            // Pobierz testy laboratoryjne powiązane z grupą
            var tests = await (from labTest in _context.LaboratoryTest
                               join labGroup in _context.LaboratoryTestsGroup
                               on labTest.LaboratoryTestsGroupId equals labGroup.Id
                               where labGroup.MedicalAppointmentId == medicalAppointmentId
                       select labTest).ToListAsync();

            return tests;




            /*var groupId = await _context.LaboratoryTestsGroup
                .Where(group => group.MedicalAppointmentId == medicalAppointmentId)
                .Select(group => group.Id)
                .FirstOrDefaultAsync();

            if (groupId == 0) 
                return new List<LaboratoryTest>();

            var tests = await _context.LaboratoryTest
                .Where(test => test.LaboratoryTestsGroupId == groupId)
                .ToListAsync();

            return tests;*/
        }

        //lsta zleconych badań "w grupach" u pacjenta
        public async Task<List<ReturnGroupWithLaboratoryTestsDto>> GetComissionedLaboratoryTestsWithGroupByPatientId(int patientId)
        {
            var groupedTests = await (from labTest in _context.LaboratoryTest
                                      join labGroup in _context.LaboratoryTestsGroup
                                      on labTest.LaboratoryTestsGroupId equals labGroup.Id
                                      where labGroup.LaboratoryAppointmentId == null   //czy == 0 ?
                                      join appointment in _context.MedicalAppointment
                                      on labGroup.MedicalAppointmentId equals appointment.Id
                                      where labGroup.LaboratoryAppointmentId == null
                                            && appointment.PatientId == patientId
                                      group labTest by labGroup.Id into testGroup
                                      select new ReturnGroupWithLaboratoryTestsDto
                                      {
                                          groupId = testGroup.Key,
                                          laboratoryTests = testGroup.ToList()
                                      }).ToListAsync();

            return groupedTests;
        }


        //Wynikiem jest lista grup(IGrouping<int, LaboratoryTest>), gdzie:
        //Kluczem(Key) grupy jest MedicalAppointmentId.
        //Wartości grupy to lista LaboratoryTest powiązanych z tą wizytą.
        //DO WYWALENIA? UŻYWAM ?
        public async Task<List<IGrouping<int, LaboratoryTest>>> GetLaboratoryTestsByPatientIdGroupByMedApp(int patientId)
        {
            //grupowanie po medicalAppointment:

            var groupedTests = await (from labTest in _context.LaboratoryTest
                                      join labGroup in _context.LaboratoryTestsGroup
                                      on labTest.LaboratoryTestsGroupId equals labGroup.Id
                                      join appointment in _context.MedicalAppointment
                                      on labGroup.MedicalAppointmentId equals appointment.Id
                                      where appointment.PatientId == patientId
                              group labTest by labGroup.MedicalAppointmentId into testGroup
                              select testGroup).ToListAsync();

            return groupedTests;

            //grupowanie po testGroup:
            /*var groupedTests = await (from test in _context.LaboratoryTest
                                      join group in _context.LaboratoryTestsGroup
                                      on test.LaboratoryTestsGroupId equals group.Id
                                      join appointment in _context.MedicalAppointment
                                      on group.MedicalAppointmentId equals appointment.Id
                                      where appointment.PatientId == patientId
                              group test by group.Id into testGroup
                              select testGroup).ToListAsync();
            return groupedTests;*/

            /*var medicalAppointmentIds = await _context.MedicalAppointment
                .Where(appointment => appointment.PatientId == patientId)
                .Select(appointment => appointment.Id)
                .ToListAsync();
            var groupIds = await _context.LaboratoryTestsGroup
                .Where(group => medicalAppointmentIds.Contains(group.MedicalAppointmentId))
                .Select(group => group.Id)
                .ToListAsync();
            var tests = await _context.LaboratoryTest
                .Where(test => groupIds.Contains(test.LaboratoryTestsGroupId))
                .ToListAsync();
            var groupedTests = tests.GroupBy(test =>
            {
                var group = _context.LaboratoryTestsGroup.FirstOrDefault(g => g.Id == test.LaboratoryTestsGroupId);
                return group?.MedicalAppointmentId ?? 0;
            }).ToList();
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
