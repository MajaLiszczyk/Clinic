namespace ClinicAPI.Dtos
{
    public class UpdatePatientDto
    {
        public int Id { get; set; }
        public string Pesel { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
