using ClinicAPI.Models;

namespace ClinicAPI.Repositories.Interfaces
{
    public interface ILaboratoryTestRepository
    {
        public Task<LaboratoryTest?> GetLaboratoryTestById(int id);
        public Task<List<LaboratoryTest>> GetAllLaboratoryTests();
        public Task<LaboratoryTest> CreateLaboratoryTest(LaboratoryTest patient);
        public Task<LaboratoryTest?> UpdateLaboratoryTest(LaboratoryTest patient);
        public Task<bool> DeleteLaboratoryTest(int id);


    }
}
