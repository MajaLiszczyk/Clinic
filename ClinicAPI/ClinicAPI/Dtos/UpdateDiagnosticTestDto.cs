using ClinicAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicAPI.Dtos
{
    public class UpdateDiagnosticTestDto
    {
        public int Id { get; set; }
        public int MedicalAppointmentId { get; set; }
        public int DoctorId { get; set; }
        public DateTime date { get; set; } 
        public int DiagnosticTestTypeId { get; set; }
        public string Description { get; set; }
    }
}
