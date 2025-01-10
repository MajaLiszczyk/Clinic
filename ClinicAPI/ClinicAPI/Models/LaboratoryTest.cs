using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Models
{
    public class LaboratoryTestType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAvailable { get; set; } = true;
    }


    public class LaboratoryTest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("MedicalAppointment")]
        public int MedicalAppointmentId { get; set; }
        public DateTime date { get; set; } 
        public int LaboratoryTestTypeId { get; set; }

        [ForeignKey("LaboratoryWorker")]
        public int LaboratoryWorkerId { get; set; }

        [ForeignKey("LaboratorySupervisor")]
        public int SupervisorId { get; set; }

        public string DoctorNote { get; set; }

    }
}
