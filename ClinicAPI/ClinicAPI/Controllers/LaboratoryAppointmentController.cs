using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Services;
using ClinicAPI.Services.Interfaces;
using ClinicAPI.UserFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LaboratoryAppointmentController : ControllerBase
    {
        private readonly ILaboratoryAppointmentService _laboratoryAppointmentService;
        private readonly IUserContext _userContext;

        public LaboratoryAppointmentController(ILaboratoryAppointmentService laboratoryAppointmentService
                                            , IUserContext userContext)
        {
            _userContext = userContext;
            _laboratoryAppointmentService = laboratoryAppointmentService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var result = await _laboratoryAppointmentService.GetAllLaboratoryAppointments();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAvailableLaboratoryAppointments()
        {
            var result = await _laboratoryAppointmentService.GetAvailableLaboratoryAppointments();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> getPlannedLaboratoryAppointmentsByPatientId([FromRoute] int id)
        {
            var result = await _laboratoryAppointmentService.getPlannedLaboratoryAppointmentsByPatientId(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> getFinishedLaboratoryAppointmentsByPatientId([FromRoute] int id)
        {
            var result = await _laboratoryAppointmentService.getFinishedLaboratoryAppointmentsByPatientId(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> cancelPlannedAppointment([FromRoute] int id)
        {
            var result = await _laboratoryAppointmentService.CancelPlannedAppointment(id);
            if (result.Confirmed)
                return Ok(new { message = result.Response });
            else return BadRequest(result.Response);
        }

        




        [HttpPost]
        [Authorize(Roles = UserRole.Registrant)]
        public async Task<IActionResult> Create([FromBody] CreateLaboratoryAppointmentDto request)
        {
            var result = await _laboratoryAppointmentService.CreateLaboratoryAppointment(request);
            if (result.Confirmed)
                return Ok(new { message = result.Response, medAppointment = result.medAppointment });
            else return BadRequest(result.Response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateLaboratoryAppointmentDto request)
        {
            var result = await _laboratoryAppointmentService.UpdateLaboratoryAppointment(request);
            if (result.Confirmed)
                return Ok(new { message = result.Response });
            else return BadRequest(result.Response);
        }
    }
}
