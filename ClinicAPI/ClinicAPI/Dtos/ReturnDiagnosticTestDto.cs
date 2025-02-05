using ClinicAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicAPI.Dtos
{
    public class ReturnDiagnosticTestDto
    {
        public int Id { get; set; }
        public int MedicalAppointmentId { get; set; }
        public int DiagnosticTestTypeId { get; set; }
        public string DiagnosticTestTypeName { get; set; }
        public string Description { get; set; }
    } 
}
