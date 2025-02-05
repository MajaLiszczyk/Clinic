using ClinicAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicAPI.Dtos
{
    public class CreateLaboratoryTestDto
    {
        [Required]
        public int LaboratoryTestTypeId { get; set; }
        public string? DoctorNote { get; set; }
    }
}
