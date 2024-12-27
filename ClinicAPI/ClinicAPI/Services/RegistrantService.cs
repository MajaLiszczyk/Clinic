using AutoMapper;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Services.Interfaces;
using System.Numerics;

namespace ClinicAPI.Services
{
    public class RegistrantService : IRegistrantService
    {
        private readonly IRegistrantRepository _registantRepository;
        private readonly IMapper _mapper;

        public RegistrantService(IRegistrantRepository registantRepository, IMapper mapper)
        {
            _registantRepository = registantRepository;
            _mapper = mapper;

        }


        // public Task<ReturnPatientDto?> GetPatientAsync(int id)
        public async Task<ReturnRegistrantDto?> GetRegistrant(int id)
        {
            var registrant = await _registantRepository.GetRegistrantById(id);
            return _mapper.Map<ReturnRegistrantDto>(registrant);
        }
        public async Task<List<ReturnRegistrantDto>> GetAllRegistrants()
        {
            var registrants = await _registantRepository.GetAllRegistrants();
            return _mapper.Map<List<ReturnRegistrantDto>>(registrants);

            //return _mapper.Map<List<ReturnMessageDto>>(messages);

        }

        public async Task<(bool Confirmed, string Response, ReturnRegistrantDto? registrant)> CreateRegistrant(CreateRegistrantDto registrant)
        {
            Registrant _registrant = new Registrant
            {
                Name = registrant.Name,
                Surname = registrant.Surname,
            };
            Registrant? p = await _registantRepository.CreateRegistrant(_registrant);
            if (p != null)
            {
                ReturnRegistrantDto r = _mapper.Map<ReturnRegistrantDto>(p);
                return await Task.FromResult((true, "Registrant successfully created.", r));
            }
            else
            {
                ReturnRegistrantDto? k = null;
                return await Task.FromResult((false, "Registrant was not created.", k));

            }


        }
        public async Task<(bool Confirmed, string Response)> UpdateRegistrant(UpdateRegistrantDto registrant)
        {
            Registrant? _registrant = await _registantRepository.GetRegistrantById(registrant.Id);

            if (_registrant == null)
            {
                return await Task.FromResult((false, "Registrant with given id does not exist."));
            }
            else
            {
                Registrant r = _mapper.Map<Registrant>(registrant);
                var p = await _registantRepository.UpdateRegistrant(_registrant);
                return await Task.FromResult((true, "Registrant succesfully uptated"));
            }
        }

        public async Task<(bool Confirmed, string Response)> TransferToArchive(int id)
        {
            Registrant? _registrant = await _registantRepository.GetRegistrantById(id);

            if (_registrant == null)
            {
                return await Task.FromResult((false, "Registrant with given id does not exist."));
            }
            else
            {
                _registrant.IsAvailable = false;
                var p = await _registantRepository.UpdateRegistrant(_registrant);
                return await Task.FromResult((true, "Registrant succesfully uptated"));
            }
        }

        

        public async Task<(bool Confirmed, string Response)> DeleteRegistrant(int id)
        {
            var registrant = await _registantRepository.GetRegistrantById(id);
            if (registrant == null) return await Task.FromResult((false, "Patient with given id does not exist."));
            else
            {
                await _registantRepository.DeleteRegistrant(id);
                return await Task.FromResult((true, "Patient successfully deleted."));
            }
        }
    }
}
