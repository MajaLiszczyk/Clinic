using ClinicAPI.Services.Interfaces;
using ClinicAPI.UserFeatures;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [ApiController]
    //[Route("api/[controller]/[action]")]
    public class LaboratoryTestsGroupController : ControllerBase
    {
        private readonly ILaboratoryTestsGroupService _laboratoryTestsGroupService;
        private readonly IUserContext _userContext;

        public LaboratoryTestsGroupController(ILaboratoryTestsGroupService laboratoryTestsGroupService
                                            , IUserContext userContext)
        {
            _laboratoryTestsGroupService = laboratoryTestsGroupService;
            _userContext = userContext;
        }

        //[HttpPut("{groupId}/{laboratoryAppointmentId}")]
        [HttpPut]
        [Route("api/LaboratoryTestsGroup/UpdateLaboratoryAppointmentToGroup/{groupId}/{laboratoryAppointmentId}")]
        public async Task<IActionResult> UpdateLaboratoryAppointmentToGroup(
            [FromRoute] int groupId,
            [FromRoute] int laboratoryAppointmentId)
        {

            var _testsGroup = await _laboratoryTestsGroupService.UpdateMedicalSpecialisation(groupId, laboratoryAppointmentId);
            if (_testsGroup.Confirmed)
                return Ok(new { message = _testsGroup.Response });
            else return BadRequest(_testsGroup.Response);
        }


    }
}
