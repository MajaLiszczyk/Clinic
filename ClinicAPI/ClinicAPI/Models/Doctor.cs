using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Models
{
    public class MedicalSpecialisation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsAvailable { get; set; } = true;
    }

    public class Doctor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        [ForeignKey("User")]
        public string? UserId { get; set; } 
        public User? User { get; set; } 
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
        public ICollection<MedicalSpecialisation> MedicalSpecialisations { get; set; } = new List<MedicalSpecialisation>();
        public bool IsAvailable { get; set; } = true;
    }
}



