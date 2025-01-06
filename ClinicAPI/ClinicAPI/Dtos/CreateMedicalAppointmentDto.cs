using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Dtos
{
    public class CreateMedicalAppointmentDto
    {
        [Required]
        public DateDto Date { get; set; }
        [Required]
        public TimeDto Time { get; set; }
        //public int PatientId { get; set; }
        [Required]
        public int DoctorId { get; set; }
       /* public string Interview { get; set; }
        public string Diagnosis { get; set; }
        public int DiseaseUnit { get; set; }*/
    }
}
