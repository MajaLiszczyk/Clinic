using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Dtos
{
    public class UpdateMedicalSpecialisationDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
