using ClinicAPI.Dtos;
using ClinicAPI.Services;
using ClinicAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LaboratoryWorkerController : ControllerBase
    {
        private readonly ILaboratoryWorkerService _laboratoryWorkerService;

        public LaboratoryWorkerController(ILaboratoryWorkerService service)
        {
            _laboratoryWorkerService = service;
        }

        //[HttpGet("{id}"), Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var result = await _laboratoryWorkerService.GetLaboratoryWorker(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        //[HttpGet, Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _laboratoryWorkerService.GetAllLaboratoryWorkers();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        //[HttpPost, Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateLaboratoryWorkerDto request)
        {
            var result = await _laboratoryWorkerService.CreateLaboratoryWorker(request);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        //[HttpPut("{id}"), Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateLaboratoryWorkerDto request)
        {
            var result = await _laboratoryWorkerService.UpdateLaboratoryWorker(request);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        //[HttpDelete("{id}"), Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _laboratoryWorkerService.DeleteLaboratoryWorker(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }
    }
}
