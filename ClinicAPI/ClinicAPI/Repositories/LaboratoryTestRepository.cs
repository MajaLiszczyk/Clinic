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
                            .FirstOrDefaultAsync();

                scope.Complete();
            }
            catch (Exception){}
            return laboratoryTest;
        }

        public async Task<List<ReturnLaboratoryTestDto>> GetLaboratoryTestsByMedicalAppointmentId(int medicalAppointmentId)
        {
            var laboratoryTests = await (from labTest in _context.LaboratoryTest
                                         join labGroup in _context.LaboratoryTestsGroup
                                         on labTest.LaboratoryTestsGroupId equals labGroup.Id
                                         where labGroup.MedicalAppointmentId == medicalAppointmentId
                                         join testType in _context.LaboratoryTestType
                                         on labTest.LaboratoryTestTypeId equals testType.Id
                                         join labApp in _context.LaboratoryAppointment
                                         on labGroup.LaboratoryAppointmentId equals labApp.Id into labAppGroup
                                         from labApp in labAppGroup.DefaultIfEmpty()
                                         select new ReturnLaboratoryTestDto
                                         {
                                             Id = labTest.Id,
                                             MedicalAppointmentId = labGroup.MedicalAppointmentId,
                                             Date = labApp != null ? labApp.DateTime : DateTime.MinValue,
                                             LaboratoryTestTypeId = labTest.LaboratoryTestTypeId,
                                             LaboratoryTestTypeName = testType.Name,
                                             LaboratoryWorkerId = labApp != null ? labApp.LaboratoryWorkerId : 0,
                                             SupervisorId = labApp != null ? labApp.SupervisorId : 0,
                                             DoctorNote = labTest.DoctorNote,
                                             Result = labTest.Result
                                         }).ToListAsync();
            return laboratoryTests;         
        }

        //lsta zleconych badań "w grupach" u pacjenta
        public async Task<List<ReturnGroupWithLaboratoryTestsDto>> GetComissionedLaboratoryTestsWithGroupByPatientId(int patientId)
        {
            var groupedTests = await (from labTest in _context.LaboratoryTest
                                      join labGroup in _context.LaboratoryTestsGroup
                                          on labTest.LaboratoryTestsGroupId equals labGroup.Id
                                      join testType in _context.LaboratoryTestType
                                          on labTest.LaboratoryTestTypeId equals testType.Id
                                      join appointment in _context.MedicalAppointment
                                          on labGroup.MedicalAppointmentId equals appointment.Id
                                      where labGroup.LaboratoryAppointmentId == null
                                            && appointment.PatientId == patientId
                                      group new { labTest, testType } by labGroup.Id into testGroup
                                      select new ReturnGroupWithLaboratoryTestsDto
                                      {
                                          groupId = testGroup.Key,
                                          laboratoryTests = testGroup.Select(t => new ReturnLaboratoryTestWithTypeName
                                          {
                                              Id = t.labTest.Id,
                                              LaboratoryTestsGroupId = t.labTest.LaboratoryTestsGroupId,
                                              State = t.labTest.State,
                                              LaboratoryTestTypeId = t.labTest.LaboratoryTestTypeId,
                                              LaboratoryTestTypeName = t.testType.Name,
                                              Result = t.labTest.Result,
                                              DoctorNote = t.labTest.DoctorNote,
                                              RejectComment = t.labTest.RejectComment
                                          }).ToList()
                                      }).ToListAsync();
            return groupedTests;          
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

        public async Task<List<LaboratoryTest>> ChangeLaboratoryTestsStateByLabAppId(int laboratoryAppointmentId, LaboratoryTestState testState)
        {
                var laboratoryTests = await (from labApp in _context.LaboratoryAppointment
                                             where labApp.Id == laboratoryAppointmentId
                                             join labGroup in _context.LaboratoryTestsGroup
                                                 on labApp.Id equals labGroup.LaboratoryAppointmentId
                                             join labTest in _context.LaboratoryTest
                                                 on labGroup.Id equals labTest.LaboratoryTestsGroupId
                                             select labTest)
                                            .ToListAsync();

                foreach (var test in laboratoryTests)
                {   
                    if(test.State != LaboratoryTestState.Accepted)
                    {
                        test.State = testState;
                    }
                }

                await _context.SaveChangesAsync();
                return laboratoryTests;
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
