using ClinicAPI.Dtos;

namespace ClinicAPI.Services.Interfaces
{
    public interface IMedicalAppointmentService
    {
        public Task<ReturnMedicalAppointmentDto?> GetMedicalAppointmentAsync(int id);
        public Task<List<ReturnMedicalAppointmentDto>> GetAllMedicalAppointmentsAsync();
        public Task<(bool Confirmed, string Response)> CreateMedicalAppointmentAsync(CreateMedicalAppointmentDto request);
        public Task<(bool Confirmed, string Response)> UpdateMedicalAppointmentAsync(UpdateMedicalAppointmentDto request, int id);
        public Task<(bool Confirmed, string Response)> DeleteMedicalAppointmentAsync(int id);
    }
}
