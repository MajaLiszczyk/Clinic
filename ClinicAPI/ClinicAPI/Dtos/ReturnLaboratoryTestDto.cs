using ClinicAPI.Models;

namespace ClinicAPI.Dtos
{
    public class ReturnLaboratoryTestDto
    {
        public int Id { get; set; }
        public int MedicalAppointmentId { get; set; }
        public DateTime Date { get; set; }
        //public LaboratoryTestType laboratoryTestType { get; set; }
        public int LaboratoryTestTypeId { get; set; }
        public string LaboratoryTestTypeName { get; set; }
        public int LaboratoryWorkerId { get; set; }
        public int SupervisorId { get; set; }
        public string? DoctorNote { get; set; }
        public string? Result { get; set; }
        //GDZIE RESULT?!!
    }
} 
