using ClinicAPI.Dtos;
using ClinicAPI.Models;

namespace ClinicAPI.Repositories.Interfaces
{
    public interface ILaboratoryAppointmentRepository
    {
        public Task<LaboratoryAppointment> GetLaboratoryAppointmentById(int laboratoryAppointmentId);
        public Task<ReturnLaboratoryAppointmentWithPatientWithTestsWithTypeNameWithMedAppDto?> GetLabAppDetailsByLabAppId(int laboratoryAppointmentId);
        //public Task<bool> IsAllLaboratoryTestsResultsCompleted(int laboratoryAppointmentId);

        public Task<List<LaboratoryAppointment>> GetAllLaboratoryAppointments();
        public Task<List<ReturnLaboratoryAppointmentWorkerSupervisorDto>> GetAllLaboratoryAppointmentsWorkersSupervisors();
        public Task<List<LaboratoryAppointment>> GetAvailableLaboratoryAppointments();
        public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getPlannedLaboratoryAppointmentsByPatientId(int patientId);
        public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getFinishedLaboratoryAppointmentsByPatientId(int patientId);
        public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getInProcessLaboratoryAppointmentsByPatientId(int patientId);

        public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getSomeLabAppsByLabWorkerId(int id, LaboratoryAppointmentState labAppState);
        public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> GetSomeLabAppsBySupervisorId(int id, LaboratoryAppointmentState labAppState);
        public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> GetAcceptedAndFinishedLabAppsBySupervisorId(int id);
        /*public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getFutureLabAppsByLabWorkerId(int id);
        public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getWaitingForFillLabAppsByLabWorkerId(int id);
        public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getWaitingForSupervisorLabAppsByLabWorkerId(int id);
        public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getToBeFixedLabAppsByLabWorkerId(int id);
        public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getReadyForPatientLabAppsByLabWorkerId(int id);
        public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getSentToPatientLabAppsByLabWorkerId(int id);
        public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getCancelledLabAppsByLabWorkerId(int id); */

        public Task<LaboratoryAppointment> CreateLaboratoryAppointment(LaboratoryAppointment laboratoryAppointment);
        public Task<LaboratoryAppointment?> UpdateLaboratoryAppointment(LaboratoryAppointment laboratoryAppointment);
    }
}
