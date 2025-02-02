using ClinicAPI.Dtos;

namespace ClinicAPI.Models
{
    public class MedicalAppointmentsOfPatient
    {
        //public List<ReturnMedicalAppointmentDto> pastMedicalAppointments { get; set; } = new List<ReturnMedicalAppointmentDto>();
        //public List<ReturnMedicalAppointmentDto> futureMedicalAppointments { get; set; } = new List<ReturnMedicalAppointmentDto>();
        public List<ReturnMedicalAppointmentPatientDoctorDto> pastMedicalAppointments { get; set; } = new List<ReturnMedicalAppointmentPatientDoctorDto>();
        public List<ReturnMedicalAppointmentPatientDoctorDto> futureMedicalAppointments { get; set; } = new List<ReturnMedicalAppointmentPatientDoctorDto>();
    }
}
