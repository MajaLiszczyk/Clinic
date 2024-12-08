using ClinicAPI.Models;

namespace ClinicAPI.Dtos
{
    public class CreateLaboratoryTestDto
    {
        public int MedicalAppointmentId { get; set; }
        public DateTime date { get; set; }
        //public LaboratoryTestType laboratoryTestType { get; set; }
        public int LaboratoryTestTypeId { get; set; }
        public int LaboratoryWorkerId { get; set; }
        public int SupervisorId { get; set; }
        public string DoctorNote { get; set; }
    }
}
