﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Models
{
    public enum LaboratoryTestType
    {
        Ferrum,
        Glucose,
        Urine
    }

    public class LaboratoryTest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("MedicalAppointment")]
        public int MedicalAppointmentId { get; set; }
        public DateTime date { get; set; } 
        public LaboratoryTestType laboratoryTestType { get; set; }

        [ForeignKey("LaboratoryWorker")]
        public int LaboratoryWorkerId { get; set; }

        [ForeignKey("LaboratorySupervisor")]
        public int SupervisorId { get; set; }

        public string DoctorNote { get; set; }
        //public LaboratorySupervisor Supervisor { get; set; }
        //public LaboratoryWorker Worker { get; set; }

    }
}
