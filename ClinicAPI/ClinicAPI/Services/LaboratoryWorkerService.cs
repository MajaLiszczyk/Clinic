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
    public class LaboratoryWorkerService : ILaboratoryWorkerService
    {
        private readonly ILaboratoryWorkerRepository _laboratoryWorkerRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public LaboratoryWorkerService(ILaboratoryWorkerRepository laboratoryWorkerRepository, UserManager<User> userManager, IMapper mapper)
        {
            _laboratoryWorkerRepository = laboratoryWorkerRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ReturnLaboratoryWorkerDto?> GetLaboratoryWorker(int id)
        {
            var laboratoryWorker = await _laboratoryWorkerRepository.GetLaboratoryWorkerById(id);
            return _mapper.Map<ReturnLaboratoryWorkerDto>(laboratoryWorker);
        }

        public async Task<List<ReturnLaboratoryWorkerDto>> GetAllLaboratoryWorkers()
        {
            var laboratoryWorkers = await _laboratoryWorkerRepository.GetAllLaboratoryWorkers();
            return _mapper.Map<List<ReturnLaboratoryWorkerDto>>(laboratoryWorkers);
        }

        public async Task<List<ReturnLaboratoryWorkerDto>> GetAllAvailableLaboratoryWorkers()
        {
            var laboratoryWorkers = await _laboratoryWorkerRepository.GetAllAvailableLaboratoryWorkers();
            return _mapper.Map<List<ReturnLaboratoryWorkerDto>>(laboratoryWorkers);
        }

        public async Task<(bool Confirmed, string Response, ReturnLaboratoryWorkerDto? laboratoryWorker)> CreateLaboratoryWorker(CreateLaboratoryWorkerDto laboratoryWorker)
        {
            LaboratoryWorker _laboratoryWorker = new LaboratoryWorker
            {
                Name = laboratoryWorker.Name,
                Surname = laboratoryWorker.Surname,
            };
            LaboratoryWorker? p = await _laboratoryWorkerRepository.CreateLaboratoryWorker(_laboratoryWorker);
            if (p != null)
            {
                ReturnLaboratoryWorkerDto r = _mapper.Map<ReturnLaboratoryWorkerDto>(p);
                return await Task.FromResult((true, "Patient successfully created.", r));
            }
            else
            {
                ReturnLaboratoryWorkerDto? k = null;
                return await Task.FromResult((false, "Patient was not created.", k));
            }
        }

        public async Task<(bool Confirmed, string Response, ReturnLaboratoryWorkerDto? laboratoryWorker)> RegisterLaboratoryWorker(CreateRegisterLaboratoryWorkerDto request)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                               new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                               TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                if (await _laboratoryWorkerRepository.GetLaboratoryWorkerWithTheSameNumber(request.LaboratoryWorkerNumber))
                {
                    return (false, "laboratoryWorker with this laboratoryWorkerNumber already exists.", null);
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

                var addToRoleResult = await _userManager.AddToRoleAsync(user, UserRole.LaboratoryWorker);
                if (!addToRoleResult.Succeeded)
                {
                    return (false, "Failed to assign role to the user.", null);
                }

                var laboratoryWorker = new LaboratoryWorker
                {
                    UserId = user.Id,      
                    Name = request.Name,
                    Surname = request.Surname,
                    LaboratoryWorkerNumber = request.LaboratoryWorkerNumber,
                };

                LaboratoryWorker? p = await _laboratoryWorkerRepository.CreateLaboratoryWorker(laboratoryWorker);
                if (p == null)
                {
                    ReturnLaboratoryWorkerDto? k = null;
                    return await Task.FromResult((false, "LaboratoryWorker was not created.", k));

                }
                ReturnLaboratoryWorkerDto r = _mapper.Map<ReturnLaboratoryWorkerDto>(p);
                scope.Complete();
                return await Task.FromResult((true, "LaboratoryWorker successfully registered.", r));
            }
            catch (Exception ex)
            {
                return (false, $"Error registerin LaboratoryWorker: {ex.Message}", null);
            }
        }


        public async Task<(bool Confirmed, string Response)> UpdateLaboratoryWorker(UpdateLaboratoryWorkerDto laboratoryWorker)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                               new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                               TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                LaboratoryWorker? _laboratoryWorker = await _laboratoryWorkerRepository.GetLaboratoryWorkerById(laboratoryWorker.Id);

                if (_laboratoryWorker == null)
                {
                    return await Task.FromResult((false, "Patient with given id does not exist."));
                }
                _laboratoryWorker.LaboratoryWorkerNumber = laboratoryWorker.LaboratoryWorkerNumber;
                _laboratoryWorker.Surname = laboratoryWorker.Surname;
                _laboratoryWorker.Name = laboratoryWorker.Name;

                var p = await _laboratoryWorkerRepository.UpdateLaboratoryWorker(_laboratoryWorker);
                scope.Complete();
                ReturnLaboratoryWorkerDto r = _mapper.Map<ReturnLaboratoryWorkerDto>(_laboratoryWorker);
                return await Task.FromResult((true, "Patient succesfully uptated"));
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
                var _laboratoryWorker = await _laboratoryWorkerRepository.GetLaboratoryWorkerById(id);

                if (_laboratoryWorker == null)
                {
                    return await Task.FromResult((false, "Patient with given id does not exist."));
                }
                if (!await _laboratoryWorkerRepository.CanArchiveLaboratoryWorker(id))
                {
                    return await Task.FromResult((false, "Can nor archive Patient with appointments."));
                }
                _laboratoryWorker.IsAvailable = false;
                var p = await _laboratoryWorkerRepository.UpdateLaboratoryWorker(_laboratoryWorker);
                scope.Complete();
                return await Task.FromResult((true, "Patient succesfully uptated"));
            }
            catch (Exception ex)
            {
                return (false, $"Error updating DiagnosticTest: {ex.Message}");
            }
        }

        public async Task<(bool Confirmed, string Response)> DeleteLaboratoryWorker(int id)
        {
            var laboratoryWorker = await _laboratoryWorkerRepository.GetLaboratoryWorkerById(id);
            if (laboratoryWorker == null) return await Task.FromResult((false, "Patient with given id does not exist."));
            else
            {
                await _laboratoryWorkerRepository.DeleteLaboratoryWorker(id);
                return await Task.FromResult((true, "Patient successfully deleted."));
            }
        }
    }
}
