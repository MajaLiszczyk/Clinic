using ClinicAPI.Models;

namespace ClinicAPI.Repositories.Interfaces
{
    public interface ILaboratoryWorkerRepository
    {
        public Task<LaboratoryWorker?> GetLaboratoryWorkerById(int id);
        public Task<List<LaboratoryWorker>> GetAllLaboratoryWorkers();
        public Task<LaboratoryWorker> CreateLaboratoryWorker(LaboratoryWorker patient);
        public Task<LaboratoryWorker?> UpdateLaboratoryWorker(LaboratoryWorker patient);
        public Task<bool> DeleteLaboratoryWorker(int id);
    }
}
