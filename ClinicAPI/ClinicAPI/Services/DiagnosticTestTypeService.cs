using AutoMapper;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Services.Interfaces;

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
            //return _mapper.Map<ReturnPatientDto>(patient);
            return testType;

        }
        
        public async Task<List<DiagnosticTestType>> GetAllDiagnosticTestTypes()
        {
            var testTypes = await _diagnosticTestTypeRepository.GetAllDiagnosticTestTypes();
            //return _mapper.Map<List<ReturnPatientDto>>(patients);
            return testTypes;
        }

        public async Task<List<DiagnosticTestType>> GetAllAvailableDiagnosticTestTypes()
        {
            var testTypes = await _diagnosticTestTypeRepository.GetAllAvailableDiagnosticTestTypes();
            //return _mapper.Map<List<ReturnPatientDto>>(patients);
            return testTypes;
        }

        public async Task<(bool Confirmed, string Response, DiagnosticTestType? patient)> CreateDiagnosticTestType(DiagnosticTestType testType)
        {
            DiagnosticTestType _testType = new DiagnosticTestType
            {
                Name = testType.Name,
            };
            DiagnosticTestType? p = await _diagnosticTestTypeRepository.CreateDiagnosticTestType(_testType);
            if (p != null)
            {
                //ReturnPatientDto r = _mapper.Map<ReturnPatientDto>(p);
                //return await Task.FromResult((true, "Patient successfully created.", r));
                return await Task.FromResult((true, "Diagnostic test type successfully created.", p));
            }
            else
            {
                DiagnosticTestType? k = null; //bez sensu tak obchodzić, da się inaczej?
                return await Task.FromResult((false, "Diagnostic test type was not created.", k));

            }
        }
        
        public async Task<(bool Confirmed, string Response)> UpdateDiagnosticTestType(DiagnosticTestType testType)
        {
            //DiagnosticTestType? _testType = await _diagnosticTestTypeRepository.GetDiagnosticTestTypeById(testType.Id);
            var _testType = await _diagnosticTestTypeRepository.GetDiagnosticTestTypeById(testType.Id);

            if (_testType == null)
            {
                return await Task.FromResult((false, "Patient with given id does not exist."));
            }
            else
            { 
                _testType.Name = testType.Name;
                //Patient r = _mapper.Map<Patient>(patient);
                var p = await _diagnosticTestTypeRepository.UpdateDiagnosticTestType(_testType);
                return await Task.FromResult((true, "Patient succesfully uptated"));
            }
        }

        public async Task<(bool Confirmed, string Response)> TransferToArchive(int id)
        {
            var _testType = await _diagnosticTestTypeRepository.GetDiagnosticTestTypeById(id);

            if (_testType == null)
            {
                return await Task.FromResult((false, "Patient with given id does not exist."));
            }
            else
            {
                _testType.IsAvailable = false;
                var p = await _diagnosticTestTypeRepository.UpdateDiagnosticTestType(_testType);
                return await Task.FromResult((true, "Patient transfered to archive"));
            }

        }


        public async Task<(bool Confirmed, string Response)> DeleteDiagnosticTestType(int id)
        {
            var testType = await _diagnosticTestTypeRepository.GetDiagnosticTestTypeById(id);
            if (testType == null) return await Task.FromResult((false, "DiagnosticTestType with given id does not exist."));
            else
            {
                await _diagnosticTestTypeRepository.DeleteDiagnosticTestType(id);
                return await Task.FromResult((true, "DiagnosticTestType successfully deleted."));
            }
        }
    }
}
