using AutoMapper;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Services.Interfaces;
using MediatR;
using System.Transactions;

namespace ClinicAPI.Services
{
    public class DiagnosticTestTypeService : IDiagnosticTestTypeService
    {
        private readonly IDiagnosticTestTypeRepository _diagnosticTestTypeRepository;
        private readonly IMapper _mapper;

        public DiagnosticTestTypeService(IDiagnosticTestTypeRepository diagnosticTestTypeRepository, IMapper mapper)
        {
            _diagnosticTestTypeRepository = diagnosticTestTypeRepository;
            _mapper = mapper;
        }

        public async Task<DiagnosticTestType?> GetDiagnosticTestType(int id)
        {
            var testType = await _diagnosticTestTypeRepository.GetDiagnosticTestTypeById(id);
            return testType;
        }
        
        public async Task<List<DiagnosticTestType>> GetAllDiagnosticTestTypes()
        {
            var testTypes = await _diagnosticTestTypeRepository.GetAllDiagnosticTestTypes();
            return testTypes;
        }

        public async Task<List<DiagnosticTestType>> GetAllAvailableDiagnosticTestTypes()
        {
            var testTypes = await _diagnosticTestTypeRepository.GetAllAvailableDiagnosticTestTypes();
            return testTypes;
        }

        public async Task<(bool Confirmed, string Response, DiagnosticTestType? patient)> CreateDiagnosticTestType(CreateDiagnosticTestTypeDto testType)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                           new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                           TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                if (await _diagnosticTestTypeRepository.IsDiagnosticTestTypeWithTheSameName(testType.Name))
                {
                    return (false, "Diagnostic Test type with this name already exists.", null);
                }

                DiagnosticTestType _testType = new DiagnosticTestType
                {
                    Name = testType.Name,
                    IsAvailable = true
                };
                DiagnosticTestType? p = await _diagnosticTestTypeRepository.CreateDiagnosticTestType(_testType);
                if (p == null)
                {
                    DiagnosticTestType? k = null;
                    return await Task.FromResult((false, "Diagnostic test type was not created.", k));
                }
                scope.Complete();
                return await Task.FromResult((true, "Diagnostic test type successfully created.", p));

            }
            catch (Exception ex)
            {
                return (false, $"Error creating DiagnosticTestType: {ex.Message}", null);
            }
        }
        
        public async Task<(bool Confirmed, string Response)> UpdateDiagnosticTestType(UpdateDiagnosticTestTypeDto testType)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                           new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                           TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                if (await _diagnosticTestTypeRepository.IsDiagnosticTestTypeWithTheSameName(testType.Name))
                {
                    return (false, "Diagnostic Test type with this name already exists.");
                }
                var _testType = await _diagnosticTestTypeRepository.GetDiagnosticTestTypeById(testType.Id);

                if (_testType == null)
                {
                    return await Task.FromResult((false, "Patient with given id does not exist."));
                }
                _testType.Name = testType.Name;
                var p = await _diagnosticTestTypeRepository.UpdateDiagnosticTestType(_testType);
                scope.Complete();
                return await Task.FromResult((true, "Patient succesfully uptated"));
            }
            catch (Exception ex)
            {
                return (false, $"Error updating DiagnosticTestType: {ex.Message}");
            }
        }

        public async Task<(bool Confirmed, string Response)> TransferToArchive(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                           new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                           TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var _testType = await _diagnosticTestTypeRepository.GetDiagnosticTestTypeById(id);
                if (_testType == null)
                {
                    return await Task.FromResult((false, "Patient with given id does not exist."));
                }
                _testType.IsAvailable = false;
                var p = await _diagnosticTestTypeRepository.UpdateDiagnosticTestType(_testType);
                scope.Complete();
                return await Task.FromResult((true, "Patient transfered to archive"));          
            }
            catch (Exception ex)
            {
                return (false, $"Error transfering to archive DiagnosticTestType: {ex.Message}");
            }
        }

        public async Task<(bool Confirmed, string Response)> DeleteDiagnosticTestType(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                           new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                           TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var testType = await _diagnosticTestTypeRepository.GetDiagnosticTestTypeById(id);
                if (testType == null)
                {
                    return await Task.FromResult((false, "DiagnosticTestType with given id does not exist."));
                }
                if (await _diagnosticTestTypeRepository.IsUsedInTests(id))
                {
                    return await Task.FromResult((false, "Can not delete DiagnosticTestType in use."));
                }
                await _diagnosticTestTypeRepository.DeleteDiagnosticTestType(id);
                scope.Complete();
                return await Task.FromResult((true, "DiagnosticTestType successfully deleted."));              
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting DiagnosticTestType: {ex.Message}");
            }
        }
    }
}
