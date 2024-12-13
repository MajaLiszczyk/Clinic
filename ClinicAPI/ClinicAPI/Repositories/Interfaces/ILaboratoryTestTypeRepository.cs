using ClinicAPI.Models;

namespace ClinicAPI.Repositories.Interfaces
{
    public interface ILaboratoryTestTypeRepository
    {
        public Task<LaboratoryTestType?> GetLaboratoryTestTypeById(int id);
        public Task<List<LaboratoryTestType>> GetAllLaboratoryTestTypes();
        public Task<LaboratoryTestType> CreateLaboratoryTestType(LaboratoryTestType type);
        public Task<LaboratoryTestType?> UpdateLaboratoryTestType(LaboratoryTestType type);
        public Task<bool> DeleteLaboratoryTestType(int id);
    }
}
