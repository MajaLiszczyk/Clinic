using ClinicAPI.Models;

namespace ClinicAPI.Repositories.Interfaces
{
    public interface ILaboratoryTestsGroupRepository
    {
        public Task<int> CreateLaboratoryTestsGroup(LaboratoryTestsGroup laboratoryTestsGroup);
        public Task<LaboratoryTestsGroup> getGroupByLabAppId(int id);
        public Task<int> UpdateLaboratoryTestsGroup(LaboratoryTestsGroup laboratoryTestsGroup);
        public Task<LaboratoryTestsGroup?> GetTestsGroupById(int id);
    }
}
