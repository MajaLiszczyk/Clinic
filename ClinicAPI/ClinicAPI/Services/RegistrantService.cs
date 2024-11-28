using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Services.Interfaces;

namespace ClinicAPI.Services
{
    public class RegistrantService : IRegistrantService
    {
        private readonly IRegistrantRepository _registrantRepository;
        public RegistrantService(IRegistrantRepository registrantRepository)
        {
            _registrantRepository = registrantRepository;
            
        }

        //public Task<ReturnRegistrantDto?> GetRegistrantAsync(int id)
        public async Task<Registrant?> GetRegistrantAsync(int id)
        {
            var registrant = await _registrantRepository.GetRegistrantByIdAsync(id);
            return registrant;
            //return _mapper.Map<ReturnMessageDto>(message);
        }
        public async Task<(bool Confirmed, string Response, Registrant? registrant)> CreateRegistrantAsync(Registrant registrant)
        {
            var _registrant = new Registrant
            {
                Name = registrant.Name,
                Surname = registrant.Surname,
            };
            await _registrantRepository.CreateRegistrantAsync(_registrant);

            return await Task.FromResult((true, "Registrant successfully created.", registrant));
        }

        /*public Task<List<ReturnRegistrantDto>> GetAllRegistrantsAsync()
        {

        }
        public Task<(bool Confirmed, string Response)> CreateRegistrantAsync(CreateRegistrantDto request)
        {

        }
        public Task<(bool Confirmed, string Response)> UpdateRegistrantAsync(UpdateRegistrantDto request, int id)
        {

        }
        public Task<(bool Confirmed, string Response)> DeleteRegistrantAsync(int id)
        {

        }*/
    }
}
