using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Models
{
    public class MedicalSpecialisation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAvailable { get; set; } = true;
    }

    public class Doctor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } //To samo Id co w ApplicationUserDoctor, bo relacja 1:1
        public string Name { get; set; }
        public string Surname { get; set; }
        public string DoctorNumber { get; set; }
        //public MedicalSpecialisation MedicalSpecialisation { get; set; }
        public ICollection<MedicalSpecialisation> MedicalSpecialisations { get; set; } = new List<MedicalSpecialisation>();
        public bool IsAvailable { get; set; } = true;



        //public List<MedicalAppointment> MedicalAppointments { get; set; } //wszystkie - przyszłe, przeszłe, nieodbyte
        //public List<DiagnosticTest> DiagnosticTests { get; set; }
    }
}


/*
 Chcę zmapować Doctor na ReturnDoctorDto za pomocą automapper. Problemem jest MedicalSpecialisations. ReturnDoctorDto mógłby zwracać listę typów prostych- listę id lub listę nazw. Kod:

public class MedicalSpecialisation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Doctor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } //To samo Id co w ApplicationUserDoctor, bo relacja 1:1
        public string Name { get; set; }
        public string Surname { get; set; }
        public string DoctorNumber { get; set; }
        public ICollection<MedicalSpecialisation> MedicalSpecialisations { get; set; } = new List<MedicalSpecialisation>();
    }

    public class ReturnDoctorDto
    {
        public int Id { get; set; } //To samo Id co w ApplicationUserDoctor, bo relacja 1:1
        public string Name { get; set; }
        public string Surname { get; set; }
        public ICollection<int> MedicalSpecialisationsIds { get; set; } = new List<int>();
    }

    CreateMap<Doctor, ReturnDoctorDto>();

 
 */
