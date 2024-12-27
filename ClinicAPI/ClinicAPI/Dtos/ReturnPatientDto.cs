namespace ClinicAPI.Dtos
{
    public class ReturnPatientDto
    {
        public int Id { get; set; } 
        public string Pesel { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PatientNumber { get; set; }
        public bool isAvailable { get; set; }

    }
}
