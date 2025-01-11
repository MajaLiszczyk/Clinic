﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Models
{
    public class LaboratoryWorker
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        [ForeignKey("User")]
        public string? UserId { get; set; } 
        public User? User { get; set; } 
        public string Name { get; set; }
        public string Surname { get; set; }
        public string LaboratoryWorkerNumber { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
