using AutoMapper;
using ClinicAPI.Models;
using ClinicAPI.Repositories;
using ClinicAPI.Repositories.Interfaces;

namespace ClinicAPI.Services
{
    public class LaboratoryTestTypeService
    {
        private readonly ILaboratoryTestTypeRepository _laboratoryTestTypeRepository;
        private readonly IMapper _mapper;

        public LaboratoryTestTypeService(ILaboratoryTestTypeRepository laboratoryTestTypeRepository, IMapper mapper)
        {
            _laboratoryTestTypeRepository = _laboratoryTestTypeRepository;
            _mapper = mapper;

        }

        public async Task<LaboratoryTestType?> GetLaboratoryTestType(int id)
        {
            var testType = await _laboratoryTestTypeRepository.GetLaboratoryTestTypeById(id);
            return testType;

        }
        
        public async Task<List<LaboratoryTestType>> GetAllLaboratoryTestTypes()
        {
            var testTypes = await _laboratoryTestTypeRepository.GetAllLaboratoryTestTypes();
            return testTypes;

        }
        
        public async Task<(bool Confirmed, string Response, LaboratoryTestType? patient)> CreateLaboratoryTestType(LaboratoryTestType testType)
        {
            LaboratoryTestType _testType = new LaboratoryTestType
            {
                Name = testType.Name,
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
        
        public async Task<(bool Confirmed, string Response)> UpdateLaboratoryTestType(DiagnosticTestType testType)
        {
            LaboratoryTestType? _testType = await _laboratoryTestTypeRepository.GetLaboratoryTestTypeById(testType.Id);

            if (_testType == null)
            {
                return await Task.FromResult((false, "LaboratoryTestType with given id does not exist."));
            }
            else
            {
                var p = await _laboratoryTestTypeRepository.UpdateLaboratoryTestType(_testType);
                return await Task.FromResult((true, "LaboratoryTestType succesfully uptated"));
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
