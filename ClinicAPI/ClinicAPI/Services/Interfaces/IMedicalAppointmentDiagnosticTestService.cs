using ClinicAPI.Dtos;
using ClinicAPI.Models;

namespace ClinicAPI.Services.Interfaces
{
    public interface IMedicalAppointmentDiagnosticTestService
    {        
        public Task<string> FinishAppointment(FinishMedicalAppointmentDto dto);
    }
}
