using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Dtos
{
    public class CreateRegisterLaboratoryWorkerDto
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
        [MaxLength(5, ErrorMessage = "LaboratoryWorkerNumber cannot exceed 5 digits.")]
        [RegularExpression(@"^[0-9]{5}$", ErrorMessage = "LaboratoryWorkerNumber must consist of 5")]
        public string LaboratoryWorkerNumber { get; set; }
    }
}
