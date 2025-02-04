using AutoMapper;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Numerics;
using System.Transactions;

namespace ClinicAPI.Services
{
    public class RegistrantService : IRegistrantService
    {
        private readonly IRegistrantRepository _registantRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public RegistrantService(IRegistrantRepository registantRepository, UserManager<User> userManager, IMapper mapper)
        {
            _registantRepository = registantRepository;
            _userManager = userManager;
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

        public async Task<(bool Confirmed, string Response, ReturnRegistrantDto? registrant)> RegisterRegistrant(CreateRegisterRegistrantDto request)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                              new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                              TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                if (await _registantRepository.GetRegistrantWithTheSameNumber(request.RegistrantNumber))
                {
                    return (false, "Registrant with this laboratoryWorkerNumber already exists.", null);
                }

                var user = new User
                {
                    UserName = request.Email,
                    Email = request.Email
                };

                var createUserResult = await _userManager.CreateAsync(user, request.Password);
                if (!createUserResult.Succeeded)
                {
                    var errorMessages = createUserResult.Errors.Select(e => e.Description).ToList();
                    return (false, string.Join("; ", errorMessages), null);
                }

                var addToRoleResult = await _userManager.AddToRoleAsync(user, UserRole.Registrant);
                if (!addToRoleResult.Succeeded)
                {
                    return (false, "Failed to assign role to the user.", null);
                }

                var registrant = new Registrant
                {
                    UserId = user.Id,
                    Name = request.Name,
                    Surname = request.Surname,
                    RegistrantNumber = request.RegistrantNumber,
                };

                Registrant? p = await _registantRepository.CreateRegistrant(registrant);
                if (p == null)
                {
                    ReturnRegistrantDto? k = null;
                    return await Task.FromResult((false, "registrant was not created.", k));

                }
                ReturnRegistrantDto r = _mapper.Map<ReturnRegistrantDto>(p);
                scope.Complete();
                return await Task.FromResult((true, "registrant successfully registered.", r));
            }
            catch (Exception ex)
            {
                return (false, $"Error register in registrant: {ex.Message}", null);
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
