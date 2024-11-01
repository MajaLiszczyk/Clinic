using ClinicAPI.Dtos;
using ClinicAPI.Services;
using ClinicAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MedicalAppointmentController : ControllerBase
    {
        private readonly IMedicalAppointmentService _medicalAppointmentService;

        /*public MedicalAppointmentController(IMedicalAppointmentService service)
        {
            _medicalAppointmentService = service;
        }*/

        //[HttpGet("{id}"), Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok();
/*
            var result = await _medicalAppointmentService.GetMedicalAppointmentAsync(id);
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
            var result = await _medicalAppointmentService.GetAllMedicalAppointmentsAsync();
            if (result != null)
                return Ok(result);
            return NotFound();
        */}

        //[HttpPost, Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateMedicalAppointmentDto request)
        {
            return Ok();
/*
            var result = await _medicalAppointmentService.CreateMedicalAppointmentAsync(request);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
  */      }

        //[HttpPut("{id}"), Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateMedicalAppointmentDto request, int id)
        {
            return Ok();
/*
            var result = await _medicalAppointmentService.UpdateMedicalAppointmentAsync(request, id);
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
            var result = await _medicalAppointmentService.DeleteMedicalAppointmentAsync(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
  */      }
    }
}
