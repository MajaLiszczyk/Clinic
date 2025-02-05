using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Dtos
{
    public class CreateRegistrantDto
    {
        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name must contain only letters.")]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Surname must contain only letters.")]
        public string Surname { get; set; }
    }
}
