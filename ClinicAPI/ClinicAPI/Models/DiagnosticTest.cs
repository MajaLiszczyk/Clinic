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

    public class DiagnosticTest // konkretne jedno badanie o danej godzinie??
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("MedicalAppointment")]
        public int MedicalAppointmentId { get; set; }

        //[ForeignKey("Doctor")]
        //public int DoctorId { get; set; } //w sumie pole niepotrzebne, bo doktor jest juz w wizycie, ale może tak będzie bardziej pod ręką
        //public DateTime date { get; set; } //nie wiem czy ma sens, bo to zawsze jest data wizyty
        [Required]
        public int DiagnosticTestTypeId { get; set; }
        //public DiagnosticTestType Type { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
