using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ClinicAPI.Models.ApplicationUsers
{
    public class ApplicationUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Login { get; set; }
        public int Password { get; set; }
        //public int Pesel { get; set; }
        //public string Name { get; set; }
        //public string Surname { get; set; }
        public string Email { get; set; }
        private List<Role> Roles { get; set; } = new List<Role>(); //nie wiem czy potrzebne
    }
}

/*
 Mam takie klasy:
    public class ApplicationUser
    {
        public int Id { get; set; }
        public int Login { get; set; }
        public int Password { get; set; }
        public string Email { get; set; }
        public List<Role> Roles { get; set; } = new List<Role>();
    }

    public class ApplicationUserDoctor
    {
        public int ApplicationUserId { get; set; } //klucz obcy z tabeli ApplicationUser
        public int DoctorId { get; set; } // klucz główny
    }
    public class Doctor
    {
        public int Id { get; set; } //To samo Id co DoctorId w ApplicationUserDoctor, bo relacja 1:1
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<MedicalAppointment> MedicalAppointments { get; set; } //wszystkie - przyszłe, przeszłe, nieodbyte
        public List<DiagnosticTest> DiagnosticTests { get; set; }
    }
Jak zrobić, żeby  ApplicationUserId z klasy ApplicationUserDoctor był kluczem obcym z klasy AppliactionUser?
Jak zrobić, żeby  Id z klasy Doctor był taki sam jak DoctorId z klasy ApplicationUserDoctor (chyba relacja 1:1)
Chcę, żeby nie było referencji do obiektów w klasach, tylko id jako klucze obce, bez nawigacji.
W ApplicationUser nie może być informacji o ApplicationUserDoctor, tylko na odwrót. Ponieważ ApplicationUser jest wspólna dla wielu klas.
Klasa Doctor nie może być zależna od ApplicationUserDoctor. To powinno być opcjonalne

 */