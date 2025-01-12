using ClinicAPI.Models;

namespace ClinicAPI.Dtos
{
    public class ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto
    {
        //medical appointment
        public int MedicalAppointmentId { get; set; }
        public DateTime MedicalAppointmentDateTime { get; set; }
        //doctor
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string DoctorSurname { get; set; }
        //laboratory appointment
        public int LaboratoryAppointmentId { get; set; }
        public int LaboratoryWorkerId { get; set; }
        public int SupervisorId { get; set; }
        public LaboratoryAppointmentState State { get; set; }
        public DateTime DateTime { get; set; }
        public string? CancelComment { get; set; }
        //patient
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string PatientSurname { get; set; }
        public string PatientPesel { get; set; }
        //laboratory tests
        public List<LaboratoryTest> laboratoryTests { get; set; }
    }
}
