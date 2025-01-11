using ClinicAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Dtos
{
    public class CreateLaboratoryAppointmentDto
    {
        [Required]
        public int LaboratoryWorkerId { get; set; }
        [Required]
        public int SupervisorId { get; set; }
        [Required]
        public DateDto Date { get; set; }
        [Required]
        public TimeDto Time { get; set; }
    }
}
