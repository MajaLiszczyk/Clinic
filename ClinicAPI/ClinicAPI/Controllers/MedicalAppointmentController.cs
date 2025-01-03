using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Services;
using ClinicAPI.Services.Interfaces;
using ClinicAPI.UserFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        //[HttpGet("{id}"), Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var result = await _medicalAppointmentService.GetMedicalAppointment(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        //[HttpGet, Authorize(Roles = "Admin")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var result = await _medicalAppointmentService.GetAllMedicalAppointments();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        //[HttpGet, Authorize(Roles = "Admin")]
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


        //[HttpGet, Authorize(Roles = "Admin")]

        //[Authorize(Roles = UserRole.Patient)]
        [HttpGet("{id}")]
        [Authorize(Roles = UserRole.Patient + "," + UserRole.Registrant)]
        public async Task<IActionResult> GetByPatientId([FromRoute] int id)
        //public async Task<IActionResult> GetByPatientId([FromRoute] string id)
        {
            //var currentUser = _userContext.GetCurrentUser();
            var currentUserId = _userContext.GetCurrentUserId();
            var currentUserRole = _userContext.GetCurrentUserRole();
            //if (currentUser == null || currentUser.Id != id) //OGARNAC CZY ID POWINNO BYC INT CZY STRING
            if (currentUserId == null) //OGARNAC CZY ID POWINNO BYC INT CZY STRING
            {
                return Forbid(); // Zwraca 403, jeśli użytkownik próbuje uzyskać dane innego użytkownika
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
            else //ZOSTAWIC TAK?
            {
                return Forbid();
            }

            //var patient = _patientService.GetPatientByUserId(id);
            //var patient = _patientService.GetPatientByUserId(currentUser.Id);

        }

        //[HttpPost, Authorize]
        [HttpPost]
        [Authorize(Roles = UserRole.Registrant)]
        public async Task<IActionResult> Create([FromBody] CreateMedicalAppointmentDto request)
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
            /*using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Aktualizacja wizyty
                var updateResult = await _medicalAppointmentService.UpdateMedicalAppointment(request.MedicalAppointmentDto);
                if (!updateResult.Confirmed)
                    return BadRequest(updateResult.Response);

                // Tworzenie testów diagnostycznych
                foreach (var testDto in request.CreateDiagnosticTestDtos)
                {
                    var diagnosticTest = new DiagnosticTest
                    {
                        MedicalAppoitmentId = testDto.MedicalAppointmentId,
                        DiagnosticTestTypeId = testDto.DiagnosticTestTypeId,
                        Description = testDto.Description
                    };
                    _context.DiagnosticTest.Add(diagnosticTest);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { message = "Operation completed successfully." });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { message = "An error occurred", details = ex.Message });
            }
        }*/

        //[HttpDelete("{id}"), Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRole.Registrant)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _medicalAppointmentService.DeleteMedicalAppointment(id);
            if (result.Confirmed)
                return Ok(new { message = result.Response });
                //return Ok(result.Response);
            else return BadRequest(result.Response);
        }
    }
}
