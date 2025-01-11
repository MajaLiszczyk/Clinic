using AutoMapper;
using ClinicAPI.Models;
using ClinicAPI.Repositories;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Services.Interfaces;
using System.Transactions;

using ClinicAPI.Dtos;
using MediatR;


namespace ClinicAPI.Services
{
    public class LaboratoryTestTypeService : ILaboratoryTestTypeService
    {
        private readonly ILaboratoryTestTypeRepository _laboratoryTestTypeRepository;
        private readonly IMapper _mapper;

        public LaboratoryTestTypeService(ILaboratoryTestTypeRepository laboratoryTestTypeRepository, IMapper mapper)
        {
            _laboratoryTestTypeRepository = laboratoryTestTypeRepository;
            _mapper = mapper;

        }

        public async Task<LaboratoryTestType?> GetLaboratoryTestType(int id)
        {
            var testType = await _laboratoryTestTypeRepository.GetLaboratoryTestTypeById(id);
            return testType;

        }
        
        public async Task<List<LaboratoryTestType>> GetAllLaboratoryTestTypes()
        {
            if (_laboratoryTestTypeRepository == null)
            {
                throw new InvalidOperationException("Repository is null");
            }
            var testTypes = await _laboratoryTestTypeRepository.GetAllLaboratoryTestTypes();
            return testTypes;

        }
        
        public async Task<(bool Confirmed, string Response, LaboratoryTestType? patient)> CreateLaboratoryTestType(CreateLaboratoryTestTypeDto testType)
        {
            LaboratoryTestType _testType = new LaboratoryTestType
            {
                Name = testType.Name,
                IsAvailable = true
            };
            LaboratoryTestType? p = await _laboratoryTestTypeRepository.CreateLaboratoryTestType(_testType);
            if (p != null)
            {
                return await Task.FromResult((true, "Laboratory test type successfully created.", p));
            }
            else
            {
                LaboratoryTestType? k = null;
                return await Task.FromResult((false, "Laboratory test type was not created.", k));

            }

        }
        
        public async Task<(bool Confirmed, string Response)> UpdateLaboratoryTestType(UpdateLaboratoryTestTypeDto testType)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                           new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                           TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                if (await _laboratoryTestTypeRepository.IsLaboratoryTestTypeWithTheSameName(testType.Name))
                {
                    return (false, "Laboratory Test type with this name already exists.");
                } 
                LaboratoryTestType? _testType = await _laboratoryTestTypeRepository.GetLaboratoryTestTypeById(testType.Id);

                if (_testType == null)
                {
                    return await Task.FromResult((false, "LaboratoryTestType with given id does not exist."));
                }
                _testType.Name = testType.Name;
                var p = await _laboratoryTestTypeRepository.UpdateLaboratoryTestType(_testType);
                return await Task.FromResult((true, "LaboratoryTestType succesfully uptated"));

            }
            catch (Exception ex)
            {
                return (false, $"Error updating LaboratoryTestType: {ex.Message}");
            }


        }

        public async Task<(bool Confirmed, string Response)> TransferToArchive(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                          new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                          TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var _testType = await _laboratoryTestTypeRepository.GetLaboratoryTestTypeById(id);
                if (_testType == null)
                {
                    return await Task.FromResult((false, "Laboratory test type with given id does not exist."));
                }
                _testType.IsAvailable = false;
                var p = await _laboratoryTestTypeRepository.UpdateLaboratoryTestType(_testType);
                scope.Complete();
                return await Task.FromResult((true, "Laboratory test type transfered to archive"));
            }
            catch (Exception ex)
            {
                return (false, $"Error transfering to archive Laboratory test type: {ex.Message}");
            }

        }


        public async Task<(bool Confirmed, string Response)> DeleteLaboratoryTestType(int id)
        {
            var testType = await _laboratoryTestTypeRepository.GetLaboratoryTestTypeById(id);
            if (testType == null) return await Task.FromResult((false, "LaboratoryTestType with given id does not exist."));
            else
            {
                await _laboratoryTestTypeRepository.DeleteLaboratoryTestType(id);
                return await Task.FromResult((true, "LaboratoryTestType successfully deleted."));
            }

        }
    }
}
