using ClinicAPI.Dtos;
using ClinicAPI.Services;
using ClinicAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LaboratorySupervisorController : ControllerBase
    {
        private readonly ILaboratorySupervisorService _laboratorySupervisorService;

        public LaboratorySupervisorController(ILaboratorySupervisorService service)
        {
            _laboratorySupervisorService = service;
        }

        //[HttpGet("{id}"), Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var result = await _laboratorySupervisorService.GetLaboratorySupervisor(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        //[HttpGet, Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _laboratorySupervisorService.GetAllLaboratorySupervisors();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailable()
        {
            var result = await _laboratorySupervisorService.GetAllAvailableLaboratorySupervisors();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        //[HttpPost, Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateLaboratorySupervisorDto request)
        {
            var result = await _laboratorySupervisorService.CreateLaboratorySupervisor(request);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        //[HttpPut("{id}"), Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateLaboratorySupervisorDto request)
        {
            var result = await _laboratorySupervisorService.UpdateLaboratorySupervisor(request);
            if (result.Confirmed)
                return Ok(new { message = result.Response });
            else return BadRequest(result.Response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> TransferToArchive([FromRoute] int id)
        {
            var result = await _laboratorySupervisorService.TransferToArchive(id);
            if (result.Confirmed)
                return Ok(new { message = result.Response });
            else return BadRequest(result.Response);
        }

        //[HttpDelete("{id}"), Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _laboratorySupervisorService.DeleteLaboratorySupervisor(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }


    }
}
