using ClinicAPI.Models;

namespace ClinicAPI.Dtos
{
    public class UpdateLaboratoryAppointmentDto
    {
        public int Id { get; set; } 
        public int LaboratoryWorkerId { get; set; } 
        public int SupervisorId { get; set; } 
        public LaboratoryAppointmentState State { get; set; }
        public DateTime DateTime { get; set; }
        public string? CancelComment { get; set; }
    }
}
