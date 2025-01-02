using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Models
{
    public class Registrant 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } //To samo Id co w ApplicationUserRegistrant, bo relacja 1:1
        [ForeignKey("User")]
        public string? UserId { get; set; } // Nullable klucz obcy do User
        public User? User { get; set; } // Opcjonalna właściwość nawigacyjna do User
        public string Name { get; set; }
        public string Surname { get; set; }
        public string RegistrantNumber { get; set; }
        public bool IsAvailable { get; set; } = true;

    }
}
