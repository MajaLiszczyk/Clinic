namespace ClinicAPI.Dtos
{
    public class ReturnDoctorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public ICollection<int> MedicalSpecialisationsIds { get; set; } = new List<int>();
        public bool isAvailable { get; set; }
    }
}
