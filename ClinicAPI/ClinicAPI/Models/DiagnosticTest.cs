using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Models
{
    public class DiagnosticTestType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsAvailable { get; set; } = true;

    }

    public class DiagnosticTest 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("MedicalAppointment")]
        public int MedicalAppointmentId { get; set; }
        [Required]
        public int DiagnosticTestTypeId { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
