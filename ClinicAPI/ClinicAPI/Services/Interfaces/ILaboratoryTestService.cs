using ClinicAPI.Dtos;

namespace ClinicAPI.Services.Interfaces
{
    public interface ILaboratoryTestService
    {
        public Task<ReturnLaboratoryTestDto?> GetLaboratoryTest(int id);
        public Task<List<ReturnLaboratoryTestDto>> GetAllLaboratoryTests();
        //public Task<List<ReturnDoctorDto>> GetAllAvailableDoctors();
        public Task<(bool Confirmed, string Response, ReturnLaboratoryTestDto? laboratoryTest)> CreateLaboratoryTest(CreateLaboratoryTestDto request);
        public Task<(bool Confirmed, string Response)> UpdateLaboratoryTest(UpdateLaboratoryTestDto request);
        public Task<(bool Confirmed, string Response)> DeleteLaboratoryTest(int id);
    }
}
