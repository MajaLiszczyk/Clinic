namespace ClinicAPI.Dtos
{
    public class CreateRegisterDoctorDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string DoctorNumber { get; set; }
        public ICollection<int> MedicalSpecialisationsIds { get; set; } = new List<int>(); //??????????
        //public string MedicalLicenseNumber { get; set; } // Przykładowe pole
    }
}
