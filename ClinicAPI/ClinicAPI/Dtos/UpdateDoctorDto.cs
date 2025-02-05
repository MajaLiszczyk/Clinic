using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Dtos
{
    public class UpdateDoctorDto
    {
        public int Id { get; set; }
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name must contain only letters.")]
        public string Name { get; set; }
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Surname must contain only letters.")]
        public string Surname { get; set; }
        [MaxLength(7, ErrorMessage = "PWZ cannot exceed 7 digits.")]
        [RegularExpression(@"^[1-9][0-9]{6}$", ErrorMessage = "PWZ must consist of 7 digits and cannot start with 0.")]
        public string DoctorNumber { get; set; }
        public ICollection<int> MedicalSpecialisationsIds { get; set; } = new List<int>();

    }
}
