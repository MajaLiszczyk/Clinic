namespace ClinicAPI.Services.Interfaces
{
    public interface ILaboratoryTestsGroupService
    {
        public Task<(bool Confirmed, string Response)> UpdateMedicalSpecialisation(int groupId, int laboratoryAppointmentId);
    }
}
