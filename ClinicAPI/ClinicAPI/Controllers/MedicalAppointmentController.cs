using ClinicAPI.Dtos;
using ClinicAPI.Services;
using ClinicAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MedicalAppointmentController : ControllerBase
    {
        private readonly IMedicalAppointmentService _medicalAppointmentService;

        public MedicalAppointmentController(IMedicalAppointmentService service)
        {
            _medicalAppointmentService = service;
        }

        //[HttpGet("{id}"), Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var result = await _medicalAppointmentService.GetMedicalAppointment(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        //[HttpGet, Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _medicalAppointmentService.GetAllMedicalAppointments();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        //[HttpGet, Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBySpecialisation([FromRoute] int id)
        {
            var result = await _medicalAppointmentService.GetMedicalAppointmentsBySpecialisation(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByDoctorId([FromRoute] int id)
        {
            var result = await _medicalAppointmentService.GetMedicalAppointmentsByDoctorId(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }
        

        //[HttpGet, Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByPatientId([FromRoute] int id)
        {
            var result = await _medicalAppointmentService.GetMedicalAppointmentsByPatientId(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        //[HttpPost, Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateMedicalAppointmentDto request)
        {
            var result = await _medicalAppointmentService.CreateMedicalAppointment(request);
            if (result.Confirmed)
                return Ok(new { message = result.Response, medAppointment = result.medAppointment });
                //return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        //[HttpPut("{id}"), Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateMedicalAppointmentDto request)
        {
            var result = await _medicalAppointmentService.UpdateMedicalAppointment(request);
            if (result.Confirmed)
                return Ok(new { message = result.Response});
                //return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        //[HttpDelete("{id}"), Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _medicalAppointmentService.DeleteMedicalAppointment(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }
    }
}
