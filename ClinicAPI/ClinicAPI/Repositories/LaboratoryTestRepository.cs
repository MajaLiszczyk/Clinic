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
        //public async Task<List<LaboratoryTest>> GetLaboratoryTestsByMedicalAppointmentId(int medicalAppointmentId)
        public async Task<List<ReturnLaboratoryTestDto>> GetLaboratoryTestsByMedicalAppointmentId(int medicalAppointmentId)
        {
            /*public int Id { get; set; }
public int MedicalAppointmentId { get; set; }
public DateTime date { get; set; }
//public LaboratoryTestType laboratoryTestType { get; set; }
public int LaboratoryTestTypeId { get; set; }
public int LaboratoryTestTypeName { get; set; }
public int LaboratoryWorkerId { get; set; }
public int SupervisorId { get; set; }
public string DoctorNote { get; set; } */

            // Pobierz ID grupy powiązanej z wizytą
            // Jeśli brak grupy, zwróć pustą listę
            // Pobierz testy laboratoryjne powiązane z grupą

            var laboratoryTests = await _context.LaboratoryTest
           .Join(
               _context.LaboratoryTestsGroup,
               labTest => labTest.LaboratoryTestsGroupId,
               labGroup => labGroup.Id,
               (labTest, labGroup) => new { labTest, labGroup }
           )
           .Where(joined => joined.labGroup.MedicalAppointmentId == medicalAppointmentId)
           .Join(
               _context.LaboratoryTestType,
               joined => joined.labTest.LaboratoryTestTypeId,
               testType => testType.Id,
               (joined, testType) => new { joined.labTest, joined.labGroup, testType }
           )
           .GroupJoin(
               _context.LaboratoryAppointment,
               joined => joined.labGroup.LaboratoryAppointmentId,
               labApp => labApp.Id,
               (joined, labAppGroup) => new { joined, labApp = labAppGroup.FirstOrDefault() }
           )
           .Select(result => new ReturnLaboratoryTestDto
           {
               Id = result.joined.labTest.Id,
               MedicalAppointmentId = result.joined.labGroup.MedicalAppointmentId,
               Date = result.labApp != null ? result.labApp.DateTime : DateTime.MinValue,
               LaboratoryTestTypeId = result.joined.labTest.LaboratoryTestTypeId,
               LaboratoryTestTypeName = result.joined.testType.Name,
               LaboratoryWorkerId = result.labApp != null ? result.labApp.LaboratoryWorkerId : 0,
               SupervisorId = result.labApp != null ? result.labApp.SupervisorId : 0,
               DoctorNote = result.joined.labTest.DoctorNote
           })
           .ToListAsync();

            return laboratoryTests;


            /*var tests = await (from labTest in _context.LaboratoryTest
                                   join labGroup in _context.LaboratoryTestsGroup
                                   on labTest.LaboratoryTestsGroupId equals labGroup.Id
                                   where labGroup.MedicalAppointmentId == medicalAppointmentId
                                   join testType in _context.LaboratoryTestType
                                   on labTest.LaboratoryTestTypeId equals testType.Id
                                   join labApp in _context.LaboratoryAppointment
                                   on labGroup.LaboratoryAppointmentId equals labApp.Id
                                   select new ReturnLaboratoryTestDto
                                   {
                                       Id = labTest.Id,
                                       MedicalAppointmentId = medicalAppointmentId,
                                       Date = labApp.DateTime,
                                       LaboratoryTestTypeId = labTest.LaboratoryTestTypeId,
                                       LaboratoryTestTypeName = testType.Name,
                                       LaboratoryWorkerId = labApp.LaboratoryWorkerId,
                                       SupervisorId = labApp.SupervisorId,
                                       DoctorNote = labTest.DoctorNote

                                       //groupId = testGroup.Key,
                                       //laboratoryTests = testGroup.ToList()
                                   }).ToListAsync(); */



            //select labTest).ToListAsync();

            //return tests;




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

        public async Task<List<LaboratoryTest>> GetLaboratoryTestsByLabAppId(int laboratoryAppointmentId)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                  new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                  TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var incompleteTests = await (from labApp in _context.LaboratoryAppointment
                                             where labApp.Id == laboratoryAppointmentId
                                             join labGroup in _context.LaboratoryTestsGroup
                                                 on labApp.Id equals labGroup.LaboratoryAppointmentId
                                             join labTest in _context.LaboratoryTest
                                                 on labGroup.Id equals labTest.LaboratoryTestsGroupId
                                             where labTest.Result == null
                                             select labTest)
                                            .ToListAsync();

                scope.Complete();
                return incompleteTests;
            }
            catch (Exception)
            {
                scope.Dispose();
                throw;
            }
        }



        public async Task<LaboratoryTest> CreateLaboratoryTest(LaboratoryTest laboratoryTest)
        {
            await _context.AddAsync(laboratoryTest);
            await _context.SaveChangesAsync();
            return laboratoryTest;
        }
        public async Task<LaboratoryTest?> UpdateLaboratoryTest(LaboratoryTest laboratoryTest)
        {

            _context.LaboratoryTest.Update(laboratoryTest);
            await _context.SaveChangesAsync();
            return laboratoryTest;

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
