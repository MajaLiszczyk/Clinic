namespace ClinicAPI.Dtos
{
    public class UpdateDoctorDto
    {
        public int Id { get; set; } //To samo Id co w ApplicationUserDoctor, bo relacja 1:1
        public string Name { get; set; }
        public string Surname { get; set; }
        public string DoctorNumber { get; set; }
        //public List<int> SpecialisationsList { get; set; }
        public ICollection<int> MedicalSpecialisationsIds { get; set; } = new List<int>();

    }
}
