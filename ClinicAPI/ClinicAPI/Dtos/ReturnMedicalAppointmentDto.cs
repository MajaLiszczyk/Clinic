namespace ClinicAPI.Dtos
{
    public class ReturnMedicalAppointmentDto
    {
        public int Id { get; set; }
        public DateTime dateTime { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string Interview { get; set; }
        public string Diagnosis { get; set; }
        public int DiseaseUnit { get; set; }
        public bool IsFinished { get; set; }
        public bool IsCancelled { get; set; }
    }
}
