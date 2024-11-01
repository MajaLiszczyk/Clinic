using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Models
{
    public enum DiagnosticTestType
    {
        BloodPressure,
        Pulse,
        Usg
    }

    public class DiagnosticTest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("MedicalAppointment")]
        public int MedicalAppoitmentId { get; set; }

        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public DateTime date { get; set; } //nie wiem czy ma sens, bo to zawsze jest data wizyty
        public DiagnosticTestType Type { get; set; }
        public string Description { get; set; }
    }
}
