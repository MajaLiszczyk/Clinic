﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicAPI.Models.ApplicationUsers
{
    public class ApplicationUserDoctor
    {
        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; } //klucz obcy z tabeli ApplicationUser

        [Key] // Klucz główny, który jest równocześnie kluczem obcym do Doctor
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; } // klucz główny
    }
}
