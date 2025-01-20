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

    public class CreateLaboratoryTestTypeDto
    {
        [Required]
        public string Name { get; set; }
    }

    public class UpdateLaboratoryTestTypeDto
    {
        [Required]
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? IsAvailable { get; set; }
    }

    public enum LaboratoryTestState
    {
        Comissioned = 0,
        ToBeCompleted = 1,  //moze lab worker zmienic
        Completed = 2, //moze lab worker zmienic
        WaitingForSupervisor = 3, 
        Accepted = 4,
        Rejected = 5 //moze lab worker zmienic
    }

    public class LaboratoryTest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("LaboratoryTestsGroup")]
        public int LaboratoryTestsGroupId { get; set; }

        [Required]
        public LaboratoryTestState State { get; set; }       

        public int LaboratoryTestTypeId { get; set; }
        public string? Result {  get; set; }
        public string? DoctorNote { get; set; }
        public string? RejectComment { get; set; }
    }




    /*public static class LaboratoryTestState
    {

        public const string ToBeCompleted = "ToBeCompleted";
        public const string WaitingForSupervisor = "WaitingForSupervisor";
        public const string Accepted = "Accepted";
        public const string Rejected = "Rejected";
    }*/

    /*public class LaboratoryTest
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



    }*/
}
