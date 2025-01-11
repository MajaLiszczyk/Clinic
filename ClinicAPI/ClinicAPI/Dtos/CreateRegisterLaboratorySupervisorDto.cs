using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Dtos
{
    public class CreateRegisterLaboratorySupervisorDto
    {
        [Required(ErrorMessage = "Email required")]
        [EmailAddress(ErrorMessage = "Email format required")]
        [MaxLength(256, ErrorMessage = "Email max length 256")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password required")]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[A-Z])(?=.*[a-z])(?=.*[\W]).{6,100}$", ErrorMessage = "Password must be 6-100 characters long." +
                           " Password must contain at least one: digit, uppercase letter, lowercase letter, special character.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Name required")]
        [MaxLength(100, ErrorMessage = "Name max length 100")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name must contain only letters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Surname required")]
        [MaxLength(100, ErrorMessage = "Surname max length 100")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Surname must contain only letters.")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Number required")]
        [MaxLength(5, ErrorMessage = "LaboratorySupervisorNumber cannot exceed 5 digits.")]
        [RegularExpression(@"^[0-9]{5}$", ErrorMessage = "LaboratorySupervisorNumber must consist of 5")]
        public string LaboratorySupervisorNumber { get; set; }
    }
}
