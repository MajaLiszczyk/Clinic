using ClinicAPI.Dtos;

namespace ClinicAPI.Services.Interfaces
{
    public interface IMedicalAppointmentService
    {
        public Task<ReturnMedicalAppointmentDto?> GetMedicalAppointment(int id);
        public Task<List<ReturnMedicalAppointmentDto>> GetAllMedicalAppointments();
        public Task<(bool Confirmed, string Response, ReturnMedicalAppointmentDto? medAppointment)> CreateMedicalAppointment(CreateMedicalAppointmentDto request);
        public Task<(bool Confirmed, string Response)> UpdateMedicalAppointment(UpdateMedicalAppointmentDto request);
        public Task<(bool Confirmed, string Response)> DeleteMedicalAppointment(int id);
    }
}
