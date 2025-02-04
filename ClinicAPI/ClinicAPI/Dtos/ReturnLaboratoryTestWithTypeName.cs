using ClinicAPI.Models;

namespace ClinicAPI.Dtos
{
    public class ReturnLaboratoryTestWithTypeName
    {
        public int Id { get; set; }
        public int LaboratoryTestsGroupId { get; set; }
        public LaboratoryTestState State { get; set; }
        public int LaboratoryTestTypeId { get; set; }
        public string LaboratoryTestTypeName { get; set; }
        public string? Result { get; set; }
        public string? DoctorNote { get; set; }
        public string? RejectComment { get; set; }
    }
}
