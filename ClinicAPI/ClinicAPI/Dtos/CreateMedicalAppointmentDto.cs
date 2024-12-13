namespace ClinicAPI.Dtos
{
    public class CreateMedicalAppointmentDto
    {
        public DateDto Date { get; set; }
        public TimeDto Time { get; set; }
        //public int PatientId { get; set; }
        public int DoctorId { get; set; }
       /* public string Interview { get; set; }
        public string Diagnosis { get; set; }
        public int DiseaseUnit { get; set; }*/
    }
}
