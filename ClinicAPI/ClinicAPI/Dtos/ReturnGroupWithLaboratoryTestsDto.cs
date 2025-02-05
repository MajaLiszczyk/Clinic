using ClinicAPI.Models;

namespace ClinicAPI.Dtos
{
    public class ReturnGroupWithLaboratoryTestsDto
    {
        public int groupId { get; set; }
        public List<ReturnLaboratoryTestWithTypeName> laboratoryTests { get; set; }   
    }

}
