namespace ClinicAPI.Dtos
{
    public class ReturnLaboratoryWorkerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string LaboratoryWorkerNumber { get; set; }
        public bool isAvailable { get; set; }
    }
}
