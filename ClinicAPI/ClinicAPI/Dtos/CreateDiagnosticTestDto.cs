using ClinicAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicAPI.Dtos
{
    public class CreateDiagnosticTestDto
    {
        [Required]
        public int MedicalAppointmentId { get; set; }
        [Required]
        public int DiagnosticTestTypeId { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
