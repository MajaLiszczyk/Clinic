using ClinicAPI.Models;

namespace ClinicAPI.Repositories.Interfaces
{
    public interface ILaboratoryAppointmentRepository
    {
        public Task<LaboratoryAppointment> GetLaboratoryAppointmentById(int laboratoryAppointmentId);
        public Task<List<LaboratoryAppointment>> GetAllLaboratoryAppointments();
        public Task<LaboratoryAppointment> CreateLaboratoryAppointment(LaboratoryAppointment laboratoryAppointment);
        public Task<LaboratoryAppointment?> UpdateLaboratoryAppointment(LaboratoryAppointment laboratoryAppointment);
    }
}
