using ClinicAPI.Models;

namespace ClinicAPI.Repositories.Interfaces
{
    public interface ILaboratorySupervisorRepository
    {
        public Task<LaboratorySupervisor?> GetLaboratorySupervisorById(int id);
        public Task<List<LaboratorySupervisor>> GetAllLaboratorySupervisors();
        public Task<LaboratorySupervisor> CreateLaboratorySupervisor(LaboratorySupervisor laboratorySupervisor);
        public Task<LaboratorySupervisor?> UpdateLaboratorySupervisor(LaboratorySupervisor laboratorySupervisor);
        public Task<bool> DeleteLaboratorySupervisor(int id);
    }
}
