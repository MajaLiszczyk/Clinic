using ClinicAPI.Dtos;

namespace ClinicAPI.Services.Interfaces
{
    public interface ILaboratoryAppointmentService
    {
        public Task<List<ReturnLaboratoryAppointmentDto>> GetAllLaboratoryAppointments();
        public Task<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto> GetLabAppDetailsByLabAppId(int id);
        //public Task<bool> IsAllLaboratoryTestsResultsCompleted(int id);
        public Task<List<ReturnLaboratoryAppointmentDto>> GetAvailableLaboratoryAppointments();
        public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getPlannedLaboratoryAppointmentsByPatientId(int id);
        public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getFinishedLaboratoryAppointmentsByPatientId(int id);
        public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getInProcessLaboratoryAppointmentsByPatientId(int id);

        public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getFutureLabAppsByLabWorkerId(int id);
        public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getWaitingForFillLabAppsByLabWorkerId(int id);
        public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getWaitingForSupervisorLabAppsByLabWorkerId(int id);
        public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getToBeFixedLabAppsByLabWorkerId(int id);
        public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getReadyForPatientLabAppsByLabWorkerId(int id);
        public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getSentToPatientLabAppsByLabWorkerId(int id);
        public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getCancelledLabAppsByLabWorkerId(int id);

        //supervisor
        public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> GetWaitingForReviewLabAppsBySupervisorId(int id);
        public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> GetAcceptedAndFinishedLabAppsBySupervisorId(int id);
        public Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> GetSentBackLabAppsBySupervisorId(int id);

        public Task<(bool Confirmed, string Response, ReturnLaboratoryAppointmentDto? medAppointment)> CreateLaboratoryAppointment(CreateLaboratoryAppointmentDto request);
        public Task<(bool Confirmed, string Response)> UpdateLaboratoryAppointment(UpdateLaboratoryAppointmentDto request);
        public Task<(bool Confirmed, string Response)> MakeCancelledLaboratoryAppointment(int id, string cancelComment);
        public Task<(bool Confirmed, string Response)> FinishLaboratoryAppointment(int id);
        public Task<(bool Confirmed, string Response)> SendLaboratoryTestsToSupervisor(int id);
        public Task<(bool Confirmed, string Response)> SendLaboratoryTestsToLaboratoryWorker(int id);
        public Task<(bool Confirmed, string Response)> SendLaboratoryTestsResultsToPatient(int id);
        public Task<(bool Confirmed, string Response)> CancelPlannedAppointment(int id);
        
    }
}
