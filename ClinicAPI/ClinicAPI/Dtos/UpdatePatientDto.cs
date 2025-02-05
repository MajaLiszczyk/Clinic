using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Dtos
{
    public class UpdatePatientDto
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "PESEL must consist of exactly 11 digits.")]
        public string Pesel { get; set; }
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
