using ClinicAPI.Models;

namespace ClinicAPI.Dtos
{
    public class CreateDoctorDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string DoctorNumber { get; set; }
        public ICollection<int> MedicalSpecialisationsIds { get; set; } = new List<int>();
        //public ICollection<MedicalSpecialisation> MedicalSpecialisations { get; set; } = new List<MedicalSpecialisation>();
    }
}
