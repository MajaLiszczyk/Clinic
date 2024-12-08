using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Models
{
    public class MedicalAppointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime dateTime { get; set; } //TYMCZASOWO Z PYTAJNIKIEM!!!!!
        //public DateTime? dateTime { get; set; } = DateTime.UtcNow;

        [ForeignKey("Patient")]
        public int? PatientId { get; set; }

        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public string? Interview { get; set; }
        public string? Diagnosis { get; set; }
        public int? DiseaseUnit { get; set;}
        //List<DiagnosticTest> DiagnosticTests { get; set; } //opcjonalne, może być pusta
        //List<LaboratoryTest> LaboratoryTests { get; set; } //opcjonalne, może być pusta
    }
}
