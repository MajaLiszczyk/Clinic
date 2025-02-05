using ClinicAPI.Dtos;

namespace ClinicAPI.Models
{
    public class MedicalAppointmentsOfPatient
    {
        public List<ReturnMedicalAppointmentPatientDoctorDto> pastMedicalAppointments { get; set; } = new List<ReturnMedicalAppointmentPatientDoctorDto>();
        public List<ReturnMedicalAppointmentPatientDoctorDto> futureMedicalAppointments { get; set; } = new List<ReturnMedicalAppointmentPatientDoctorDto>();
    }
}
