using ClinicAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicAPI.Dtos
{
    public class UpdateLaboratoryTestDto
    {
        public int Id { get; set; }
        public int MedicalAppointmentId { get; set; }
        public DateTime date { get; set; }
        public int LaboratoryTestTypeId { get; set; }
        public int LaboratoryWorkerId { get; set; }
        public int SupervisorId { get; set; }
        public string DoctorNote { get; set; }
    }
}
