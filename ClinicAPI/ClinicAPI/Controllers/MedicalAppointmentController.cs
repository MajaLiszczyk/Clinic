using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Services;
using ClinicAPI.Services.Interfaces;
using ClinicAPI.UserFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ClinicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MedicalAppointmentController : ControllerBase
    {
        private readonly IMedicalAppointmentService _medicalAppointmentService;
        private readonly IPatientService _patientService;
        private readonly IMedicalAppointmentDiagnosticTestService _medicalAppointmentDiagnosticTestService;
        private readonly IUserContext _userContext;

        public MedicalAppointmentController(IMedicalAppointmentService service
                                            ,IPatientService patientService
                                            ,IMedicalAppointmentDiagnosticTestService medicalAppointmentDiagnosticTestService
                                            ,IUserContext userContext)
        {
            _medicalAppointmentService = service;
            _patientService = patientService;
            _medicalAppointmentDiagnosticTestService = medicalAppointmentDiagnosticTestService;
            _userContext = userContext;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var result = await _medicalAppointmentService.GetMedicalAppointment(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetWithPatientsDoctors()
        {
            var result = await _medicalAppointmentService.GetAllMedicalAppointmentsPatientsDoctors();
            if (result != null)
                return Ok(result);
            return NotFound();
        }    

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var result = await _medicalAppointmentService.GetAllMedicalAppointments();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetBySpecialisation([FromRoute] int id)
        {
            var result = await _medicalAppointmentService.GetMedicalAppointmentsBySpecialisation(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = UserRole.Doctor + "," + UserRole.Registrant)]
        public async Task<IActionResult> GetByDoctorId([FromRoute] int id)
        {
            var result = await _medicalAppointmentService.GetMedicalAppointmentsByDoctorId(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = UserRole.Patient + "," + UserRole.Registrant)]
        public async Task<IActionResult> GetFutureMedicalAppointmentsByPatientOrUserId([FromRoute] int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var role = User.FindFirst(ClaimTypes.Role)?.Value;

                var result = await _medicalAppointmentService.GetFutureMedicalAppointmentsByPatientOrUserId(id, userId, role);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Wystąpił błąd serwera.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = UserRole.Patient + "," + UserRole.Registrant)]
        public async Task<IActionResult> GetPastMedicalAppointmentsByPatientOrUserId([FromRoute] int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var role = User.FindFirst(ClaimTypes.Role)?.Value;

                var result = await _medicalAppointmentService.GetPastMedicalAppointmentsByPatientOrUserId(id, userId, role);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Wystąpił błąd serwera.", details = ex.Message });
            }
        }


        [HttpGet("{id}")]
        [Authorize(Roles = UserRole.Patient + "," + UserRole.Registrant)]
        public async Task<IActionResult> GetByPatientId([FromRoute] int id)
        {
            var currentUserId = _userContext.GetCurrentUserId();
            var currentUserRole = _userContext.GetCurrentUserRole();
            if (currentUserId == null)
            {
                return Forbid();
            }

            if(currentUserRole == UserRole.Registrant)
            {
                var result = await _medicalAppointmentService.GetMedicalAppointmentsByPatientId(id);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }

            if(currentUserRole == UserRole.Patient)
            {
                var patient = await _patientService.GetPatientByUserId(currentUserId);

                var result = await _medicalAppointmentService.GetMedicalAppointmentsByPatientId(patient.Id);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            else 
            {
                return Forbid();
            }
        }

        [HttpPost]
        [Authorize(Roles = UserRole.Registrant)]
        public async Task<IActionResult> Create([FromBody] CreateMedicalAppointmentDto request)
        {
            var result = await _medicalAppointmentService.CreateMedicalAppointment(request);
            if (result.Confirmed)
                return Ok(new { message = result.Response, medAppointment = result.medAppointment });
            else return BadRequest(result.Response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePatientCancel([FromBody] ReturnMedicalAppointmentPatientDoctorDto request)
        {
            var result = await _medicalAppointmentService.UpdatePatientCancel(request);
            if (result.Confirmed)
                return Ok(new { message = result.Response});
            else return BadRequest(result.Response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateMedicalAppointmentDto request)
        {
            var result = await _medicalAppointmentService.UpdateMedicalAppointment(request);
            if (result.Confirmed)
                return Ok(new { message = result.Response });
            else return BadRequest(result.Response);
        }

        [HttpPost]
        [Authorize(Roles = UserRole.Doctor)]
        public async Task<IActionResult> FinishMedicalAppointment([FromBody] FinishMedicalAppointmentDto request)
        {
                try
                {
                    var response = await _medicalAppointmentDiagnosticTestService.FinishAppointment(request);
                    return Ok(new { message = response });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = "An error occurred", details = ex.Message });
                }
        }
           
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRole.Registrant)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _medicalAppointmentService.DeleteMedicalAppointment(id);
            if (result.Confirmed)
                return Ok(new { message = result.Response });
            else return BadRequest(result.Response);
        }
    }
}
