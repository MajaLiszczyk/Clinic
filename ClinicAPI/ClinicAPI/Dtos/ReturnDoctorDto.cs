namespace ClinicAPI.Dtos
{
    public class ReturnDoctorDto
    {
        public int Id { get; set; } //To samo Id co w ApplicationUserDoctor, bo relacja 1:1
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
