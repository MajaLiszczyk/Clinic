namespace ClinicAPI.Dtos
{
    public class FinishMedicalAppointmentDto
    {
        public UpdateMedicalAppointmentDto MedicalAppointmentDto { get; set; }
        public List<CreateDiagnosticTestDto> CreateDiagnosticTestDtos { get; set; }
        public List<CreateLaboratoryTestDto> CreateLaboratoryTestDtos { get; set; }
    }
}
