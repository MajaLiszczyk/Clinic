using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Dtos
{
    public class CreatePatientDto
    {
        //[PeselValidator]
        [Required]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "PESEL must consist of exactly 11 digits.")]
        public string Pesel { get; set; }
        [Required]
        [MaxLength(100)] //czemu 100?
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name must contain only letters.")]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Surname must contain only letters.")]
        public string Surname { get; set; }
    }

    /*public class PeselValidatorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var pesel = value as string;
            if (string.IsNullOrEmpty(pesel) || pesel.Length != 11 || !pesel.All(char.IsDigit))
            {
                return new ValidationResult("PESEL must be exactly 11 digits long.");
            }
            return ValidationResult.Success;
        }
    } */

}
