using ClinicAPI.Dtos;
using ClinicAPI.Services;
using ClinicAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LaboratoryWorkerController : ControllerBase
    {
        private readonly ILaboratoryWorkerService _laboratoryWorkerService;

        public LaboratoryWorkerController(ILaboratoryWorkerService service)
        {
            _laboratoryWorkerService = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var result = await _laboratoryWorkerService.GetLaboratoryWorker(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _laboratoryWorkerService.GetAllLaboratoryWorkers();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailable()
        {
            var result = await _laboratoryWorkerService.GetAllAvailableLaboratoryWorkers();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLaboratoryWorkerDto request)
        {
            var result = await _laboratoryWorkerService.CreateLaboratoryWorker(request);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateLaboratoryWorkerDto request)
        {
            var result = await _laboratoryWorkerService.UpdateLaboratoryWorker(request);
            if (result.Confirmed)
                return Ok(new { message = result.Response });
            else return BadRequest(result.Response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> TransferToArchive([FromRoute] int id)
        {
            var result = await _laboratoryWorkerService.TransferToArchive(id);
            if (result.Confirmed)
                return Ok(new { message = result.Response });
            else return BadRequest(result.Response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _laboratoryWorkerService.DeleteLaboratoryWorker(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }
    }
}
