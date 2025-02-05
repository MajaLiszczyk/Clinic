using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Models
{
    public enum LaboratoryAppointmentState
    {
        Empty = 0,
        Reserved = 1,
        ToBeCompleted = 2,
        WaitingForSupervisor = 3,
        ToBeFixed = 4,
        AllAccepted = 5, //ready for patient
        Finished = 6, //sent to patient
        Cancelled = 7
    }

    public class LaboratoryAppointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("LaboratoryWorker")]
        public int LaboratoryWorkerId { get; set; }

        [ForeignKey("LaboratorySupervisor")]
        public int SupervisorId { get; set; }

        public LaboratoryAppointmentState State { get; set; }

        public DateTime DateTime { get; set; }
        public string? CancelComment { get; set; }
    }
}
