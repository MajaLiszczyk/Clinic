using ClinicAPI.Models.ApplicationUsers;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Models
{
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } //To samo Id co w ApplicationUserPatient, bo relacja 1:1
        public string Pesel { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PatientNumber { get; set; }
        //static int patientsAmount {get; set;}
        //public List<MedicalAppointment> MedicalAppointments { get; set; } //wszystkie - przyszłe, przeszłe, nieodbyte
        //public List<DiagnosticTest> DiagnosticTests { get; set; } 
        //public List<LaboratoryTest> LaboratoryTests { get; set; } //wszystkie - przyszłe, przeszłe, nieodbyte
    }
}
