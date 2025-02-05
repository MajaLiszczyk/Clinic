using ClinicAPI.Dtos;
using ClinicAPI.Models;

namespace ClinicAPI.Services.Interfaces
{
    public interface ILaboratoryTestService
    {
        public Task<ReturnLaboratoryTestDto?> GetLaboratoryTest(int id);
        public Task<List<ReturnLaboratoryTestDto>> GetLaboratoryTestsByMedicalAppointmentId(int id);
        public Task<List<ReturnLaboratoryTestDto>> GetAllLaboratoryTests();
        public Task<List<LaboratoryTest>> GetLaboratoryTestsByLabAppId(int laboratoryAppointmentId);
        public Task<List<ReturnGroupWithLaboratoryTestsDto>> GetComissionedLaboratoryTestsWithGroupByPatientId(int id);
        public Task<(bool Confirmed, string Response, ReturnLaboratoryTestDto? laboratoryTest)> CreateLaboratoryTest(CreateLaboratoryTestDto request);
        public Task<(bool Confirmed, string Response)> UpdateLaboratoryTest(UpdateLaboratoryTestDto request);
        public Task<(bool Confirmed, string Response)> SaveLaboratoryTestResult(int id, string resultValue);
        public Task<(bool Confirmed, string Response)> AcceptLaboratoryTestResult(int id);
        public Task<(bool Confirmed, string Response)> RejectLaboratoryTestResult(int id, string rejectComment);
        public Task<(bool Confirmed, string Response)> DeleteLaboratoryTest(int id);
    }
}
