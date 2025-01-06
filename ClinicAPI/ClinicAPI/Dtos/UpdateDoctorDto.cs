using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Dtos
{
    public class UpdateDoctorDto
    {
        public int Id { get; set; } //To samo Id co w ApplicationUserDoctor, bo relacja 1:1
        [MaxLength(100)] //czemu 100?
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name must contain only letters.")]
        public string Name { get; set; }
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Surname must contain only letters.")]
        public string Surname { get; set; }
        public string DoctorNumber { get; set; }
        //public List<int> SpecialisationsList { get; set; }
        public ICollection<int> MedicalSpecialisationsIds { get; set; } = new List<int>();

    }
}
