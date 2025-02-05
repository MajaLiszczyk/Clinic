namespace ClinicAPI.Dtos
{
    public class ReturnMedicalAppointmentPatientDto
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int? PatientId { get; set; }
        public string PatientName { get; set; }
        public string PatientSurname { get; set; }
        public string PatientPesel { get; set; }
        public int DoctorId { get; set; }
        public string? Interview { get; set; }
        public string? Diagnosis { get; set; }
        public bool? IsFinished { get; set; }
        public bool? IsCancelled { get; set; }
        public string? CancellingComment { get; set; }
    }
}
