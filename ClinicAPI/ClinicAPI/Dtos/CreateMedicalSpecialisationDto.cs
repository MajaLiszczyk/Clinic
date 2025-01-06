using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Dtos
{
    public class CreateMedicalSpecialisationDto
    {
        [Required]
        public string Name { get; set; }
    }
}
