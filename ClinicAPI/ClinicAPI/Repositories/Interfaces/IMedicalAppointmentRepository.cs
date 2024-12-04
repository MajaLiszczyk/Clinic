using ClinicAPI.Models;

namespace ClinicAPI.Repositories.Interfaces
{
    public interface IMedicalAppointmentRepository
    {
        public Task<MedicalAppointment?> GetMedicalAppointmentById(int id);
        public Task<List<MedicalAppointment>> GetAllMedicalAppointments();
        public Task<List<MedicalAppointment>> GetMedicalAppointmentsBySpecialisation(int id);
        public Task<MedicalAppointment> CreateMedicalAppointment(MedicalAppointment medicalAppointment);
        public Task<MedicalAppointment?> UpdateMedicalAppointment(MedicalAppointment medicalAppointment);
        public Task<bool> DeleteMedicalAppointment(int id);
    }
}
