using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Dtos
{
    public class CreateRegisterDoctorDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[A-Z])(?=.*[a-z])(?=.*[\W]).{6,100}$", ErrorMessage = "Password must be 6-100 characters long." +
                           " Password must contain at least one: digit, uppercase letter, lowercase letter, special character.")]
        public string Password { get; set; }
        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name must contain only letters.")]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Surname must contain only letters.")]
        public string Surname { get; set; }
        [Required]
        [MaxLength(7, ErrorMessage = "PWZ cannot exceed 7 digits.")]
        [RegularExpression(@"^[1-9][0-9]{6}$", ErrorMessage = "PWZ must consist of 7 digits and cannot start with 0.")]
        public string DoctorNumber { get; set; }
        public ICollection<int> MedicalSpecialisationsIds { get; set; } = new List<int>();
    }
}
