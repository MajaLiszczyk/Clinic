using ClinicAPI.Models;

namespace ClinicAPI.Repositories.Interfaces
{
    public interface IRegistrantRepository
    {
        public Task<Registrant?> GetRegistrantByIdAsync(int id);
        public Task CreateRegistrantAsync(Registrant registrant);
    }
}
