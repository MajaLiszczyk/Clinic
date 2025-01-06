using ClinicAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Dtos
{
    public class CreateDoctorDto //chyba juz nie używane, bo musi miec konto
    {
        [Required]
        [MaxLength(100)] //czemu 100?
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name must contain only letters.")]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Surname must contain only letters.")]
        public string Surname { get; set; }
        public string DoctorNumber { get; set; }
        public ICollection<int> MedicalSpecialisationsIds { get; set; } = new List<int>();
        //public ICollection<MedicalSpecialisation> MedicalSpecialisations { get; set; } = new List<MedicalSpecialisation>();
    }
}
