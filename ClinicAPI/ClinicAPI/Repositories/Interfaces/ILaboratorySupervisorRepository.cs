using ClinicAPI.Models;

namespace ClinicAPI.Repositories.Interfaces
{
    public interface ILaboratorySupervisorRepository
    {
        public Task<LaboratorySupervisor?> GetLaboratorySupervisorById(int id);
        public Task<List<LaboratorySupervisor>> GetAllLaboratorySupervisors();
        public Task<List<LaboratorySupervisor>> GetAllAvailableLAboratorySupervisors();

        public Task<bool> GetLaboratorySupervisorWithTheSameNumber(string number);
        public Task<LaboratorySupervisor> CreateLaboratorySupervisor(LaboratorySupervisor laboratorySupervisor);
        public Task<LaboratorySupervisor?> UpdateLaboratorySupervisor(LaboratorySupervisor laboratorySupervisor);
        public Task<bool> DeleteLaboratorySupervisor(int id);
        public Task<bool> CanArchiveLaboratorySupervisor(int laboratorySupervisorId);

    }
}
