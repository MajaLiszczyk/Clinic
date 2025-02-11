﻿using System.ComponentModel.DataAnnotations.Schema;
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
}
