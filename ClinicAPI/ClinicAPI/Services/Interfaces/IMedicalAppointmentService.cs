using ClinicAPI.Dtos;
using ClinicAPI.Models;

namespace ClinicAPI.Services.Interfaces
{
    public interface IMedicalAppointmentService
    {
        public Task<ReturnMedicalAppointmentDto?> GetMedicalAppointment(int id);
        public Task<List<ReturnMedicalAppointmentDto>> GetAllMedicalAppointments();
        public Task<List<ReturnMedicalAppointmentPatientDoctorDto>> GetAllMedicalAppointmentsPatientsDoctors();
        public Task<List<ReturnMedicalAppointmentDto>> GetMedicalAppointmentsBySpecialisation(int id);
        public Task<MedicalAppointmentsOfPatient> GetMedicalAppointmentsByDoctorId(int id);
        public Task<List<ReturnMedicalAppointmentPatientDoctorDto>> GetFutureMedicalAppointmentsByPatientOrUserId(int id, string userId, string role);
        public Task<List<ReturnMedicalAppointmentPatientDoctorDto>> GetPastMedicalAppointmentsByPatientOrUserId(int id, string userId, string role);
        public Task<MedicalAppointmentsOfPatient> GetMedicalAppointmentsByPatientId(int id);
        public Task<(bool Confirmed, string Response, ReturnMedicalAppointmentDto? medAppointment)> CreateMedicalAppointment(CreateMedicalAppointmentDto request);
        public Task<(bool Confirmed, string Response)> UpdatePatientCancel(ReturnMedicalAppointmentPatientDoctorDto request);
        public Task<(bool Confirmed, string Response)> UpdateMedicalAppointment(UpdateMedicalAppointmentDto request);
        public Task<(bool Confirmed, string Response)> DeleteMedicalAppointment(int id);
    }
}
