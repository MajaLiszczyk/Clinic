namespace ClinicAPI.Dtos
{
    public class UpdateLaboratoryWorkerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? LaboratoryWorkerNumber { get; set; }

    }
}
