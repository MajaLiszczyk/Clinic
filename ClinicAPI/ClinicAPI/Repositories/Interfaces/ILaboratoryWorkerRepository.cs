using ClinicAPI.Models;

namespace ClinicAPI.Repositories.Interfaces
{
    public interface ILaboratoryWorkerRepository
    {
        public Task<LaboratoryWorker?> GetLaboratoryWorkerById(int id);
        public Task<LaboratoryWorker?> GetLaboratoryWorkerByUserId(string userId);
        public Task<List<LaboratoryWorker>> GetAllLaboratoryWorkers();
        public Task<List<LaboratoryWorker>> GetAllAvailableLaboratoryWorkers();
        public Task<LaboratoryWorker> CreateLaboratoryWorker(LaboratoryWorker patient);
        public Task<bool>GetLaboratoryWorkerWithTheSameNumber(string number);
        public Task<LaboratoryWorker?> UpdateLaboratoryWorker(LaboratoryWorker patient);
        public Task<bool> DeleteLaboratoryWorker(int id);
        public Task<bool> CanArchiveLaboratoryWorker(int laboratoryWorkerId);

    }
}
