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
    public class LaboratorySupervisorService : ILaboratorySupervisorService
    {
        private readonly ILaboratorySupervisorRepository _laboratorySupervisorRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;



        public LaboratorySupervisorService(ILaboratorySupervisorRepository laboratorySupervisorRepository, UserManager<User> userManager, IMapper mapper)
        {
            _laboratorySupervisorRepository = laboratorySupervisorRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ReturnLaboratorySupervisorDto?> GetLaboratorySupervisor(int id)
        {
            var laboratorySupervisor = await _laboratorySupervisorRepository.GetLaboratorySupervisorById(id);
            return _mapper.Map<ReturnLaboratorySupervisorDto>(laboratorySupervisor);
        }

        public async Task<List<ReturnLaboratorySupervisorDto>> GetAllLaboratorySupervisors()
        {
            var laboratorySupervisor = await _laboratorySupervisorRepository.GetAllLaboratorySupervisors();
            return _mapper.Map<List<ReturnLaboratorySupervisorDto>>(laboratorySupervisor);
        }

        public async Task<List<ReturnLaboratorySupervisorDto>> GetAllAvailableLaboratorySupervisors()
        {
            var laboratorySupervisors = await _laboratorySupervisorRepository.GetAllAvailableLAboratorySupervisors();
            return _mapper.Map<List<ReturnLaboratorySupervisorDto>>(laboratorySupervisors);
        }

        public async Task<(bool Confirmed, string Response, ReturnLaboratorySupervisorDto? supervisor)> CreateLaboratorySupervisor(CreateLaboratorySupervisorDto laboratorySupervisor)
        {
            LaboratorySupervisor _laboratorySupervisor = new LaboratorySupervisor()
            {
                Name = laboratorySupervisor.Name,
                Surname = laboratorySupervisor.Surname,
            };
            LaboratorySupervisor? p = await _laboratorySupervisorRepository.CreateLaboratorySupervisor(_laboratorySupervisor);
            if (p != null)
            {
                ReturnLaboratorySupervisorDto r = _mapper.Map<ReturnLaboratorySupervisorDto>(p);
                return await Task.FromResult((true, "Patient successfully created.", r));
            }
            else
            {
                ReturnLaboratorySupervisorDto? k = null;
                return await Task.FromResult((false, "Patient was not created.", k));
            }
        }

        public async Task<(bool Confirmed, string Response, ReturnLaboratorySupervisorDto? laboratorySupervisor)> RegisterLaboratorySupervisor(CreateRegisterLaboratorySupervisorDto request)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                               new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                               TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                if (await _laboratorySupervisorRepository.GetLaboratorySupervisorWithTheSameNumber(request.LaboratorySupervisorNumber))
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
                    //ReturnLaboratoryWorkerDto? k = null;
                    return (false, "Failed to assign role to the user.", null);
                }

                var laboratorySupervisor = new LaboratorySupervisor
                {
                    UserId = user.Id,
                    Name = request.Name,
                    Surname = request.Surname,
                    LaboratorySupervisorNumber = request.LaboratorySupervisorNumber,
                };

                LaboratorySupervisor? p = await _laboratorySupervisorRepository.CreateLaboratorySupervisor(laboratorySupervisor);
                if (p == null)
                {
                    ReturnLaboratorySupervisorDto? k = null;
                    return await Task.FromResult((false, "laboratorySupervisor was not created.", k));

                }
                ReturnLaboratorySupervisorDto r = _mapper.Map<ReturnLaboratorySupervisorDto>(p);
                scope.Complete();
                return await Task.FromResult((true, "laboratorySupervisor successfully registered.", r));
            }
            catch (Exception ex)
            {
                return (false, $"Error registerin laboratorySupervisor: {ex.Message}", null);
            }
        }




        public async Task<(bool Confirmed, string Response)> UpdateLaboratorySupervisor(UpdateLaboratorySupervisorDto laboratorySupervisor)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                               new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                               TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                /*if (await _laboratorySupervisorRepository.GetLaboratorySupervisorWithTheSameNumber(laboratorySupervisor.))
                {
                    return (false, "Patient with this PESEL already exists.");
                } */

                LaboratorySupervisor? _laboratorySupervisor = await _laboratorySupervisorRepository.GetLaboratorySupervisorById(laboratorySupervisor.Id);

                if (_laboratorySupervisor == null)
                {
                    return await Task.FromResult((false, "Patient with given id does not exist."));
                }
                _laboratorySupervisor.LaboratorySupervisorNumber = laboratorySupervisor.LaboratorySupervisorNumber;
                _laboratorySupervisor.Surname = laboratorySupervisor.Surname;
                _laboratorySupervisor.Name = laboratorySupervisor.Name;

                var p = await _laboratorySupervisorRepository.UpdateLaboratorySupervisor(_laboratorySupervisor);
                scope.Complete();
                ReturnLaboratorySupervisorDto r = _mapper.Map<ReturnLaboratorySupervisorDto>(_laboratorySupervisor);
                return await Task.FromResult((true, "Patient succesfully uptated"));
            }
            catch (Exception ex)
            {
                return (false, $"Error updating DiagnosticTest: {ex.Message}");
            }
           
        }
        
        public async Task<(bool Confirmed, string Response)> DeleteLaboratorySupervisor(int id)
        {
            var laboratorySupervisor = await _laboratorySupervisorRepository.GetLaboratorySupervisorById(id);
            if (laboratorySupervisor == null) return await Task.FromResult((false, "Patient with given id does not exist."));
            else
            {
                await _laboratorySupervisorRepository.DeleteLaboratorySupervisor(id);
                return await Task.FromResult((true, "Patient successfully deleted."));
            }
        }

        public async Task<(bool Confirmed, string Response)> TransferToArchive(int id)
        {

            using var scope = new TransactionScope(TransactionScopeOption.Required,
                               new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                               TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var _laboratorySupervisor = await _laboratorySupervisorRepository.GetLaboratorySupervisorById(id);

                if (_laboratorySupervisor == null)
                {
                    return await Task.FromResult((false, "_laboratorySupervisor with given id does not exist."));
                }
                if (!await _laboratorySupervisorRepository.CanArchiveLaboratorySupervisor(id))
                {
                    return await Task.FromResult((false, "Can nor archive _laboratorySupervisor with appointments."));
                }
                _laboratorySupervisor.IsAvailable = false;
                var p = await _laboratorySupervisorRepository.UpdateLaboratorySupervisor(_laboratorySupervisor);
                scope.Complete();
                return await Task.FromResult((true, "_laboratorySupervisor succesfully uptated"));
            }
            catch (Exception ex)
            {
                return (false, $"Error updating _laboratorySupervisor: {ex.Message}");
            }
        }




    }
}
