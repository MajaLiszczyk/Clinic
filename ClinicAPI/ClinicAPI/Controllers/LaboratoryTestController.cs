using ClinicAPI.Dtos;
using ClinicAPI.Services;
using ClinicAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    public class LaboratoryTestResultDto
    {
        public string ResultValue { get; set; }
    }

    public class LaboratoryTestRejectCommentDto
    {
        public string RejectCommentValue { get; set; }
    }

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LaboratoryTestController : ControllerBase
    {
        private readonly ILaboratoryTestService _laboratoryTestService;

        public LaboratoryTestController(ILaboratoryTestService service)
        {
            _laboratoryTestService = service;
        }

        //[HttpGet("{id}"), Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            ReturnLaboratoryTestDto? result = await _laboratoryTestService.GetLaboratoryTest(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        /*[HttpGet]
        [Authorize]
        public async Task<IActionResult> GetLaboratoryTestsByPatientId(int id)
        {
            var result = await _laboratoryTestService.GetLaboratoryTestsByPatientId(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        } */

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetComissionedLaboratoryTestsWithGroupByPatientId([FromRoute] int id)
        {
            var result = await _laboratoryTestService.GetComissionedLaboratoryTestsWithGroupByPatientId(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetByMedicalAppointmentId([FromRoute]int id)
        {
            var result = await _laboratoryTestService.GetLaboratoryTestsByMedicalAppointmentId(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }


        //[HttpGet, Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _laboratoryTestService.GetAllLaboratoryTests();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetLaboratoryTestsByLabAppId([FromRoute] int id)
        {
            try
            {
                var incompleteTests = await _laboratoryTestService.GetLaboratoryTestsByLabAppId(id);
                return Ok(incompleteTests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[HttpPost, Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateLaboratoryTestDto request)
        {

            var result = await _laboratoryTestService.CreateLaboratoryTest(request);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        //[HttpPut("{id}"), Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateLaboratoryTestDto request)
        {

            var result = await _laboratoryTestService.UpdateLaboratoryTest(request);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> SaveLaboratoryTestResult([FromRoute] int id, [FromBody] LaboratoryTestResultDto resultDto)
        {
            if (resultDto == null || string.IsNullOrWhiteSpace(resultDto.ResultValue))
            {
                return BadRequest("Invalid data.");
            }
            var result = await _laboratoryTestService.SaveLaboratoryTestResult(id, resultDto.ResultValue);
            if (result.Confirmed)
                return Ok(new { message = result.Response });
                //return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AcceptLaboratoryTest([FromRoute] int id)
        {
            var result = await _laboratoryTestService.AcceptLaboratoryTestResult(id);
            if (result.Confirmed)
                return Ok(new { message = result.Response });
            else return BadRequest(result.Response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> RejectLaboratoryTest([FromRoute] int id, [FromBody] LaboratoryTestRejectCommentDto rejectCommentDto)
        {
            if (rejectCommentDto == null || string.IsNullOrWhiteSpace(rejectCommentDto.RejectCommentValue))
            {
                return BadRequest("Invalid data.");
            }
            var result = await _laboratoryTestService.RejectLaboratoryTestResult(id, rejectCommentDto.RejectCommentValue);
            if (result.Confirmed)
                return Ok(new { message = result.Response });
            //return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        
        //[HttpDelete("{id}"), Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _laboratoryTestService.DeleteLaboratoryTest(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }
    }
}
