using ClinicAPI.Dtos;
using ClinicAPI.Services;
using ClinicAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DiagnosticTestController : ControllerBase
    {
        //private readonly IDiagnosticTestService _diagnosticTestService;

        /*public DiagnosticTestController(IDiagnosticTestService service)
        {
            _diagnosticTestService = service;
        }*/
        public DiagnosticTestController()
        {
        }

        //[HttpGet("{id}"), Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok();

            /*
            var result = await _diagnosticTestService.GetDiagnosticTestAsync(id);
            if (result != null)
                return Ok(result);
            return NotFound();*/
        }

        //[HttpGet, Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
/*
            var result = await _diagnosticTestService.GetAllDiagnosticTestsAsync();
            if (result != null)
                return Ok(result);
            return NotFound();*/
        }

        //[HttpPost, Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateDiagnosticTestDto request)
        {
            return Ok();
            /*
            var result = await _diagnosticTestService.CreateDiagnosticTestAsync(request);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);*/
        }

        //[HttpPut("{id}"), Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateDiagnosticTestDto request, int id)
        {
            return Ok();
/*
            var result = await _diagnosticTestService.UpdateDiagnosticTestAsync(request, id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);*/
        }

        //[HttpDelete("{id}"), Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok();
/*
            var result = await _diagnosticTestService.DeleteDiagnosticTestAsync(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);*/
        }

    }
}
