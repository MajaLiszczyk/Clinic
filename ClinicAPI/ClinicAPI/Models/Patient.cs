using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Models
{
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string? UserId { get; set; } 
        public User? User { get; set; }
        [Required]
        [MaxLength(11)]
        public string Pesel { get; set; }
        [Required]
        [MaxLength(100)] 
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Surname { get; set; }
        public string PatientNumber { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
