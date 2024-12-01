using ClinicAPI.Dtos;

namespace ClinicAPI.Services.Interfaces
{
    public interface ILaboratorySupervisorService
    {
        public Task<ReturnLaboratorySupervisorDto?> GetLaboratorySupervisor(int id);
        public Task<List<ReturnLaboratorySupervisorDto>> GetAllLaboratorySupervisors();
        public Task<(bool Confirmed, string Response, ReturnLaboratorySupervisorDto? supervisor)> CreateLaboratorySupervisor(CreateLaboratorySupervisorDto request);
        public Task<(bool Confirmed, string Response)> UpdateLaboratorySupervisor(UpdateLaboratorySupervisorDto request);
        public Task<(bool Confirmed, string Response)> DeleteLaboratorySupervisor(int id);
    }
}
