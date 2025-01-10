using AutoMapper;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Services.Interfaces;
using System.Transactions;

namespace ClinicAPI.Services
{
    public class MedicalAppointmentDiagnosticTestService : IMedicalAppointmentDiagnosticTestService
    {
        private readonly IMedicalAppointmentRepository _medicalAppointmentRepository;
        private readonly IDiagnosticTestRepository _diagnosticTestRepository;
        private readonly IMapper _mapper;


        public MedicalAppointmentDiagnosticTestService(
            IMedicalAppointmentRepository medicalAppointmentRepository,
            IDiagnosticTestRepository diagnosticTestRepository,
            IMapper mapper)
        {
            _medicalAppointmentRepository = medicalAppointmentRepository;
            _diagnosticTestRepository = diagnosticTestRepository;
            _mapper = mapper;
        }

        public async Task<string> FinishAppointment(FinishMedicalAppointmentDto dto)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                               new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                               TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                // Aktualizacja wizyty
                var appointment = await _medicalAppointmentRepository.GetMedicalAppointmentById(dto.MedicalAppointmentDto.Id);
                if (appointment == null)
                {
                    throw new Exception("Appointment not found");
                }
                    

                appointment.DateTime = dto.MedicalAppointmentDto.DateTime;
                appointment.PatientId = dto.MedicalAppointmentDto.PatientId;
                appointment.DoctorId = dto.MedicalAppointmentDto.DoctorId;
                appointment.Interview = dto.MedicalAppointmentDto.Interview;
                appointment.Diagnosis = dto.MedicalAppointmentDto.Diagnosis;
                appointment.IsFinished = dto.MedicalAppointmentDto.IsFinished;
                appointment.IsCancelled = dto.MedicalAppointmentDto.IsCancelled;

                var p = await _medicalAppointmentRepository.UpdateMedicalAppointment(appointment);

                // Tworzenie testów diagnostycznych
                foreach (var testDto in dto.CreateDiagnosticTestDtos)
                {
                    var diagnosticTest = new DiagnosticTest
                    {
                        MedicalAppointmentId = testDto.MedicalAppointmentId,
                        DiagnosticTestTypeId = testDto.DiagnosticTestTypeId,
                        Description = testDto.Description
                    };
                    DiagnosticTest? r = await _diagnosticTestRepository.CreateDiagnosticTest(diagnosticTest);
                    if (r != null)
                    {
                        ReturnDiagnosticTestDto q = _mapper.Map<ReturnDiagnosticTestDto>(r);
                    }
                    else
                    {
                        ReturnDiagnosticTestDto? k = null; 

                    }
                }

                scope.Complete();
                return "Operation completed successfully.";
            }
            catch (Exception ex)
            {
                //transaction.Dispose();
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }
    




    }
}
