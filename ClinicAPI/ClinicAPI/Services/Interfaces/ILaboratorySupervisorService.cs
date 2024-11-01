using ClinicAPI.Dtos;

namespace ClinicAPI.Services.Interfaces
{
    public interface ILaboratorySupervisorService
    {
        public Task<ReturnLaboratorySupervisorDto?> GetLaboratorySupervisorAsync(int id);
        public Task<List<ReturnLaboratorySupervisorDto>> GetAllLaboratorySupervisorsAsync();
        public Task<(bool Confirmed, string Response)> CreateLaboratorySupervisorAsync(CreateLaboratorySupervisorDto request);
        public Task<(bool Confirmed, string Response)> UpdateLaboratorySupervisorAsync(UpdateLaboratorySupervisorDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteLaboratorySupervisorAsync(int id);
    }
}
