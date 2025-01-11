namespace ClinicAPI.Dtos
{
    public class FinishMedicalAppointmentDto
    {
        public UpdateMedicalAppointmentDto MedicalAppointmentDto { get; set; }
        public List<CreateDiagnosticTestDto> CreateDiagnosticTestDtos { get; set; }
        public List<CreateLaboratoryTestDto> CreateLaboratoryTestDtos { get; set; }
        //    public List<CreateDiagnosticTestDto> CreateDiagnosticTestDtos { get; set; } = new();




        /*public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string Interview { get; set; }
        public string Diagnosis { get; set; }
        public int DiseaseUnit { get; set; }
        public bool IsFinished { get; set; }
        public bool IsCancelled { get; set; } */


    }
}
