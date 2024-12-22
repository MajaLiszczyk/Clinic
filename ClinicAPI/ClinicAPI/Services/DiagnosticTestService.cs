using AutoMapper;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Services.Interfaces;

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
            /*List<ReturnDiagnosticTestDto> dtoList = new List<ReturnDiagnosticTestDto>();
            var diagnosticTests = await _diagnosticTestRepository.GetByMedicalAppointmentId(id);
            //List<ReturnMedicalAppointmentDto> mappedAppointments = _mapper.Map<List<ReturnMedicalAppointmentDto>>(medicalAppointments);
            foreach(DiagnosticTest test in diagnosticTests)
            {
                ReturnDiagnosticTestDto dto = new ReturnDiagnosticTestDto()
                {
                    Id = test.Id,
                    MedicalAppoitmentId = test.MedicalAppoitmentId,
                    DiagnosticTestTypeId = test.DiagnosticTestTypeId,
                    DiagnosticTestTypeName =
                    Description = test.Description,
                };
                dtoList.Add(dto);
            }

            return dtoList; */
        }


        public async Task<(bool Confirmed, string Response, ReturnDiagnosticTestDto? diagnosticTest)> CreateDiagnosticTest(CreateDiagnosticTestDto diagnosticTest)
        {
            DiagnosticTest _diagnosticTest = new DiagnosticTest
            {
                MedicalAppoitmentId = diagnosticTest.MedicalAppoitmentId,
                //DoctorId = diagnosticTest.DoctorId,
                //date = diagnosticTest.date,
                Description = diagnosticTest.Description,
                DiagnosticTestTypeId = diagnosticTest.DiagnosticTestTypeId,
                //Type = diagnosticTest.Type,
            };
            DiagnosticTest? p = await _diagnosticTestRepository.CreateDiagnosticTest(_diagnosticTest);
            if (p != null)
            {
                ReturnDiagnosticTestDto r = _mapper.Map<ReturnDiagnosticTestDto>(p);
                return await Task.FromResult((true, "DiagnosticTest successfully created.", r));
            }
            else
            {
                ReturnDiagnosticTestDto? k = null; //bez sensu tak obchodzić, da się inaczej?
                return await Task.FromResult((false, "DiagnosticTest was not created.", k));

            }

        }
        public async Task<(bool Confirmed, string Response)> UpdateDiagnosticTest(UpdateDiagnosticTestDto diagnosticTest)
        {
            //DiagnosticTest? _diagnosticTest = await _diagnosticTestRepository.GetDiagnosticTestById(diagnosticTest.Id);
            var _diagnosticTest = await _diagnosticTestRepository.GetDiagnosticTestById(diagnosticTest.Id);

            if (_diagnosticTest == null)
            {
                return await Task.FromResult((false, "DiagnosticTest with given id does not exist."));
            }
            else
            {
                DiagnosticTest r = _mapper.Map<DiagnosticTest>(diagnosticTest);
                var p = await _diagnosticTestRepository.UpdateDiagnosticTest(r);
                return await Task.FromResult((true, "DiagnosticTest succesfully uptated"));
            }
        }
        public async Task<(bool Confirmed, string Response)> DeleteDiagnosticTest(int id)
        {
            var _diagnosticTest = await _diagnosticTestRepository.GetDiagnosticTestById(id);
            if (_diagnosticTest == null) return await Task.FromResult((false, "DiagnosticTest with given id does not exist."));
            else
            {
                await _diagnosticTestRepository.DeleteDiagnosticTest(id);
                return await Task.FromResult((true, "Diagnostic test successfully deleted."));
            }
        }



        /*public Task<ReturnDiagnosticTestDto?> GetDiagnosticTestAsync(int id)
        {

        }
        public Task<List<ReturnDiagnosticTestDto>> GetAllDiagnosticTestsAsync()
        {

        }
        public Task<(bool Confirmed, string Response)> CreateDiagnosticTestAsync(CreateDiagnosticTestDto request)
        {

        }
        public Task<(bool Confirmed, string Response)> UpdateDiagnosticTestAsync(UpdateDiagnosticTestDto request, int id)
        {

        }
        public Task<(bool Confirmed, string Response)> DeleteDiagnosticTestAsync(int id)
        {

        }*/

    }
}
