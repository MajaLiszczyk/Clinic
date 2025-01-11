using ClinicAPI.Models;

namespace ClinicAPI.Repositories.Interfaces
{
    public interface ILaboratoryTestsGroupRepository
    {
        public Task<int> CreateLaboratoryTestsGroup(LaboratoryTestsGroup laboratoryTestsGroup);      
    }
}
