using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Models
{
    public class Doctor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } //To samo Id co w ApplicationUserDoctor, bo relacja 1:1
        public string Name { get; set; }
        public string Surname { get; set; }
        //public List<MedicalAppointment> MedicalAppointments { get; set; } //wszystkie - przyszłe, przeszłe, nieodbyte
        //public List<DiagnosticTest> DiagnosticTests { get; set; }
    }
}
