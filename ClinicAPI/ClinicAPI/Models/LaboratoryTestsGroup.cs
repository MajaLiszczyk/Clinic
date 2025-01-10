using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Models
{
    public class LaboratoryTestsGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("MedicalAppointment")]
        [Required]
        public int MedicalAppointmentId { get; set; }

        [ForeignKey("LaboratoryAppointment")]
        public int? LaboratoryAppointmentId { get; set; }
    }
}
