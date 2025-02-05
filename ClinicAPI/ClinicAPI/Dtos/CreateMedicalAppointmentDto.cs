using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Dtos
{
    public class CreateMedicalAppointmentDto
    {
        [Required]
        public DateDto Date { get; set; }
        [Required]
        public TimeDto Time { get; set; }
        [Required]
        public int DoctorId { get; set; }
    }
}
