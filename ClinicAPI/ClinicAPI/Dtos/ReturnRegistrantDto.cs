namespace ClinicAPI.Dtos
{
    public class ReturnRegistrantDto
    {
        public int Id { get; set; } //To samo Id co w ApplicationUserRegistrant, bo relacja 1:1
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool isAvailable { get; set; }

    }
}
