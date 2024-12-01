namespace ClinicAPI.Dtos
{
    public class CreatePatientDto
    {
        public string Pesel { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
