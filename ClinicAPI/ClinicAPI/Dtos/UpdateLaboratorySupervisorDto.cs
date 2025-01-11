namespace ClinicAPI.Dtos
{
    public class UpdateLaboratorySupervisorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? LaboratorySupervisorNumber { get; set; }

    }
}
