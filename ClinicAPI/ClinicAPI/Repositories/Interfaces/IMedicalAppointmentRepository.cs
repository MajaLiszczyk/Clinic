using ClinicAPI.Dtos;
using ClinicAPI.Models;

namespace ClinicAPI.Repositories.Interfaces
{
    public interface IMedicalAppointmentRepository
    {
        public Task<MedicalAppointment?> GetMedicalAppointmentById(int id);
        public Task<ReturnMedicalAppointmentPatientDto?> GetMedicalAppointmentByIdWithPatient(int id);
        public Task<List<ReturnMedicalAppointmentPatientDoctorDto>> GetAllMedicalAppointmentsPatientsDoctors();
        public Task<List<MedicalAppointment>> GetAllMedicalAppointments();
        public Task<List<ReturnMedicalAppointmentDoctorDto>> GetMedicalAppointmentsBySpecialisation(int id);
        public Task<List<ReturnMedicalAppointmentPatientDoctorDto>> GetMedicalAppointmentsByDoctorId(int id);
        public Task<List<ReturnMedicalAppointmentPatientDoctorDto>> GetMedicalAppointmentsByPatientId(int id);
        public Task<List<ReturnMedicalAppointmentPatientDoctorDto>> GetFutureMedicalAppointmentsByPatientUserId(string userId);
        public Task<List<ReturnMedicalAppointmentPatientDoctorDto>> GetPastMedicalAppointmentsByPatientUserId(string userId);
        public Task<List<ReturnMedicalAppointmentPatientDoctorDto>> GetFutureMedicalAppointmentsByPatientId(int patientId);
        public Task<List<ReturnMedicalAppointmentPatientDoctorDto>> GetPastMedicalAppointmentsByPatientId(int patientId);
        public Task<MedicalAppointment> CreateMedicalAppointment(MedicalAppointment medicalAppointment);
        public Task<MedicalAppointment?> UpdateMedicalAppointment(MedicalAppointment medicalAppointment);
        public Task<bool> DeleteMedicalAppointment(int id);
        public Task<bool> HasPatientMedicalAppointments(int patientId);
        public Task<bool> HasDoctorMedicalAppointments(int doctorId);
    }
}
