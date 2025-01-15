using ClinicAPI.Dtos;
using ClinicAPI.Models;

namespace ClinicAPI.Repositories.Interfaces
{
    public interface ILaboratoryAppointmentRepository
    {
        public Task<LaboratoryAppointment> GetLaboratoryAppointmentById(int laboratoryAppointmentId);
        public Task<List<LaboratoryAppointment>> GetAllLaboratoryAppointments();
        public Task<List<LaboratoryAppointment>> GetAvailableLaboratoryAppointments();
        public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getPlannedLaboratoryAppointmentsByPatientId(int patientId);
        public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getFinishedLaboratoryAppointmentsByPatientId(int patientId);
        public Task<LaboratoryAppointment> CreateLaboratoryAppointment(LaboratoryAppointment laboratoryAppointment);
        public Task<LaboratoryAppointment?> UpdateLaboratoryAppointment(LaboratoryAppointment laboratoryAppointment);
    }
}
