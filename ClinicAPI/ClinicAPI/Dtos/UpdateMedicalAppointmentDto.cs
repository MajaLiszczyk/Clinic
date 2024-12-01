using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicAPI.Dtos
{
    public class UpdateMedicalAppointmentDto
    {
        public int Id { get; set; }
        public DateTime dateTime { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string Interview { get; set; }
        public string Diagnosis { get; set; }
        public int DiseaseUnit { get; set; }
    }
}
