namespace ClinicAPI.Dtos
{
    public class CreateRegisterPatientDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Pesel { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        //public string PatientNumber { get; set; }
    }
}
