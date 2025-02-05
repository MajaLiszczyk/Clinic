using ClinicAPI.Dtos;

namespace ClinicAPI.Services.Interfaces
{
    public interface ILaboratorySupervisorService
    {
        public Task<ReturnLaboratorySupervisorDto?> GetLaboratorySupervisor(int id);
        public Task<List<ReturnLaboratorySupervisorDto>> GetAllLaboratorySupervisors();
        public Task<List<ReturnLaboratorySupervisorDto>> GetAllAvailableLaboratorySupervisors();
        public Task<(bool Confirmed, string Response, ReturnLaboratorySupervisorDto? supervisor)> CreateLaboratorySupervisor(CreateLaboratorySupervisorDto request);
        public Task<(bool Confirmed, string Response, ReturnLaboratorySupervisorDto? laboratorySupervisor)> RegisterLaboratorySupervisor(CreateRegisterLaboratorySupervisorDto request);
        public Task<(bool Confirmed, string Response)> UpdateLaboratorySupervisor(UpdateLaboratorySupervisorDto request);
        public Task<(bool Confirmed, string Response)> DeleteLaboratorySupervisor(int id);
        public Task<(bool Confirmed, string Response)> TransferToArchive(int id);
    }
}
