using ClinicAPI.Dtos;
using ClinicAPI.Models;

namespace ClinicAPI.Repositories.Interfaces
{
    public interface ILaboratoryTestRepository
    {
        public Task<LaboratoryTest?> GetLaboratoryTestById(int id);
        //public Task<List<IGrouping<int, LaboratoryTest>>> GetLaboratoryTestsByPatientId(int patientId);
        //public Task<List<LaboratoryTest>> GetLaboratoryTestsByMedicalAppointmentId(int medicalAppointmentId);
        public Task<List<ReturnLaboratoryTestDto>> GetLaboratoryTestsByMedicalAppointmentId(int medicalAppointmentId);
        public Task<List<ReturnGroupWithLaboratoryTestsDto>> GetComissionedLaboratoryTestsWithGroupByPatientId(int patientId);
        public Task<List<LaboratoryTest>> GetAllLaboratoryTests();
        public Task<List<LaboratoryTest>> GetLaboratoryTestsByLabAppId(int laboratoryAppointmentId);
        public Task<LaboratoryTest> CreateLaboratoryTest(LaboratoryTest patient);
        public Task<LaboratoryTest?> UpdateLaboratoryTest(LaboratoryTest patient);
        public Task<List<LaboratoryTest>> ChangeLaboratoryTestsStateByLabAppId(int laboratoryAppointmentId, LaboratoryTestState testState);
        public Task<bool> DeleteLaboratoryTest(int id);


    }
}
