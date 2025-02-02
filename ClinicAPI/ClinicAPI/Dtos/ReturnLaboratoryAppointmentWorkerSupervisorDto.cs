using ClinicAPI.Models;

namespace ClinicAPI.Dtos
{
    public class ReturnLaboratoryAppointmentWorkerSupervisorDto
    {
        public int Id { get; set; }
        public int LaboratoryWorkerId { get; set; }
        public string LaboratoryWorkerName { get; set; }
        public string LaboratoryWorkerSurname { get; set; }
        public int SupervisorId { get; set; }
        public string SupervisorName { get; set; }
        public string SupervisorSurname { get; set; }
        public LaboratoryAppointmentState State { get; set; }
        public DateTime DateTime { get; set; }
        public string? CancelComment { get; set; }
    }
}
