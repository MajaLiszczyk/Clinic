using AutoMapper;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Services.Interfaces;
using System.Transactions;

namespace ClinicAPI.Services
{
    public class DiagnosticTestService : IDiagnosticTestService
    {
        private readonly IDiagnosticTestRepository _diagnosticTestRepository;
        private readonly IMapper _mapper;

        public DiagnosticTestService(IDiagnosticTestRepository diagnosticTestRepository, IMapper mapper)
        {
            _diagnosticTestRepository = diagnosticTestRepository;
            _mapper = mapper;

        }
        public async Task<ReturnDiagnosticTestDto?> GetDiagnosticTest(int id)
        {
            var diagnosticTest = await _diagnosticTestRepository.GetDiagnosticTestById(id);
            return _mapper.Map<ReturnDiagnosticTestDto>(diagnosticTest);
        }

        public async Task<List<ReturnDiagnosticTestDto>> GetAllDiagnosticTests()
        {
            var diagnosticTests = await _diagnosticTestRepository.GetAllDiagnosticTests();
            return _mapper.Map<List<ReturnDiagnosticTestDto>>(diagnosticTests);
        }

        public async Task<List<ReturnDiagnosticTestDto>> GetByMedicalAppointmentId(int id)
        {          
            var tests = await _diagnosticTestRepository.GetByMedicalAppointmentId(id);
            return tests;
        }

        public async Task<(bool Confirmed, string Response, ReturnDiagnosticTestDto? diagnosticTest)> CreateDiagnosticTest(CreateDiagnosticTestDto diagnosticTest)
        {
            DiagnosticTest _diagnosticTest = new DiagnosticTest
            {
                MedicalAppointmentId = diagnosticTest.MedicalAppointmentId,
                Description = diagnosticTest.Description,
                DiagnosticTestTypeId = diagnosticTest.DiagnosticTestTypeId,
            };
            DiagnosticTest? p = await _diagnosticTestRepository.CreateDiagnosticTest(_diagnosticTest);
            if (p != null)
            {
                ReturnDiagnosticTestDto r = _mapper.Map<ReturnDiagnosticTestDto>(p);
                return await Task.FromResult((true, "DiagnosticTest successfully created.", r));
            }
            else
            {
                ReturnDiagnosticTestDto? k = null;
                return await Task.FromResult((false, "DiagnosticTest was not created.", k));

            }
        }

        public async Task<(bool Confirmed, string Response)> UpdateDiagnosticTest(UpdateDiagnosticTestDto diagnosticTest)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                           new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                           TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var _diagnosticTest = await _diagnosticTestRepository.GetDiagnosticTestById(diagnosticTest.Id);

                if (_diagnosticTest == null)
                {
                    return await Task.FromResult((false, "DiagnosticTest with given id does not exist."));
                }

                DiagnosticTest r = _mapper.Map<DiagnosticTest>(diagnosticTest);
                var p = await _diagnosticTestRepository.UpdateDiagnosticTest(r);
                scope.Complete();
                return await Task.FromResult((true, "DiagnosticTest succesfully uptated"));
              
            }
            catch (Exception ex) {
                return (false, $"Error updating DiagnosticTest: {ex.Message}");
            }
        }
    }
}
