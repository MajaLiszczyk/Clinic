namespace ClinicAPI.Dtos
{
    public class ReturnRegistrantDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string RegistrantNumber { get; set; }
        public bool isAvailable { get; set; }
    }
}
