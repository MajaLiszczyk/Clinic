using ClinicAPI.Dtos;
using ClinicAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        /*public PatientController(IPatientService service)
        {
            _patientService = service;
        }*/

        //[HttpGet("{id}"), Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok();
/*
            var result = await _patientService.GetPatientAsync(id);
            if (result != null)
                return Ok(result);
            return NotFound();
  */      }

        //[HttpGet, Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
/*
            var result = await _patientService.GetAllPatientsAsync();
            if (result != null)
                return Ok(result);
            return NotFound();
  */      }

        //[HttpPost, Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePatientDto request)
        {
            return Ok();
/*
            var result = await _patientService.CreatePatientAsync(request);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
  */      }

        //[HttpPut("{id}"), Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdatePatientDto request, int id)
        {
            return Ok();

/*            var result = await _patientService.UpdatePatientAsync(request, id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
  */      }

        //[HttpDelete("{id}"), Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok();
/*
            var result = await _patientService.DeletePatientAsync(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
  */      }
    }


}
