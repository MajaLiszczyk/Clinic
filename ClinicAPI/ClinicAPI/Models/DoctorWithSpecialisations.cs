namespace ClinicAPI.Models
{
    public class DoctorWithSpecialisations
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string DoctorNumber { get; set; }
        public List<int> SpecialisationIds { get; set; } = new List<int>();
        public bool IsAvailable { get; set; }

    }
}
