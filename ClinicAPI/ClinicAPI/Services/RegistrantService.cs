using AutoMapper;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Services.Interfaces;
using System.Numerics;
using System.Transactions;

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

        public async Task<ReturnRegistrantDto?> GetRegistrant(int id)
        {
            var registrant = await _registantRepository.GetRegistrantById(id);
            return _mapper.Map<ReturnRegistrantDto>(registrant);
        }
        public async Task<List<ReturnRegistrantDto>> GetAllRegistrants()
        {
            var registrants = await _registantRepository.GetAllRegistrants();
            return _mapper.Map<List<ReturnRegistrantDto>>(registrants);

        }

        public async Task<(bool Confirmed, string Response, ReturnRegistrantDto? registrant)> CreateRegistrant(CreateRegistrantDto registrant)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                               new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                               TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                Registrant _registrant = new Registrant
                {
                    Name = registrant.Name,
                    Surname = registrant.Surname,
                };
                Registrant? p = await _registantRepository.CreateRegistrant(_registrant);
                if (p == null)
                {
                    ReturnRegistrantDto? k = null;
                    return await Task.FromResult((false, "Registrant was not created.", k));
                }
                ReturnRegistrantDto r = _mapper.Map<ReturnRegistrantDto>(p);
                scope.Complete();
                return await Task.FromResult((true, "Registrant successfully created.", r));

            }
            catch (Exception ex)
            {
                return (false, $"Error updating DiagnosticTest: {ex.Message}", null);
            }
            


        }
        public async Task<(bool Confirmed, string Response)> UpdateRegistrant(UpdateRegistrantDto registrant)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                               new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                               TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                Registrant? _registrant = await _registantRepository.GetRegistrantById(registrant.Id);

                if (_registrant == null)
                {
                    return await Task.FromResult((false, "Registrant with given id does not exist."));
                }
                Registrant r = _mapper.Map<Registrant>(registrant);
                var p = await _registantRepository.UpdateRegistrant(_registrant);
                scope.Complete();
                return await Task.FromResult((true, "Registrant succesfully uptated"));
            }
            catch (Exception ex)
            {
                return (false, $"Error updating DiagnosticTest: {ex.Message}");
            }
            
        }

        public async Task<(bool Confirmed, string Response)> TransferToArchive(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                               new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                               TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                Registrant? _registrant = await _registantRepository.GetRegistrantById(id);

                if (_registrant == null)
                {
                    return await Task.FromResult((false, "Registrant with given id does not exist."));
                }
                _registrant.IsAvailable = false;
                var p = await _registantRepository.UpdateRegistrant(_registrant);
                scope.Complete();
                return await Task.FromResult((true, "Registrant succesfully uptated"));
            }
            catch (Exception ex)
            {
                return (false, $"Error updating DiagnosticTest: {ex.Message}");
            }
           
        }

        

        public async Task<(bool Confirmed, string Response)> DeleteRegistrant(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                               new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                               TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var registrant = await _registantRepository.GetRegistrantById(id);
                if (registrant == null)
                {
                    return await Task.FromResult((false, "Patient with given id does not exist."));
                }
                await _registantRepository.DeleteRegistrant(id);
                scope.Complete();
                return await Task.FromResult((true, "Patient successfully deleted."));
            }
            catch (Exception ex)
            {
                return (false, $"Error updating DiagnosticTest: {ex.Message}");
            }
            
        }
    }
}
