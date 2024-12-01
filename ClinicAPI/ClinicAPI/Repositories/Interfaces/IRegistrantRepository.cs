using ClinicAPI.Models;

namespace ClinicAPI.Repositories.Interfaces
{
    public interface IRegistrantRepository
    {
        public Task<Registrant?> GetRegistrantById(int id);
        public Task<List<Registrant>> GetAllRegistrants();
        public Task<Registrant> CreateRegistrant(Registrant registrant);
        public Task<Registrant?> UpdateRegistrant(Registrant registrant);
        public Task<bool> DeleteRegistrant(int id);
    }
}
