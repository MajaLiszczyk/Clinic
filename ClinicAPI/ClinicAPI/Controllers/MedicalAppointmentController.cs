using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Services;
using ClinicAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MedicalAppointmentController : ControllerBase
    {
        private readonly IMedicalAppointmentService _medicalAppointmentService;
        private readonly IMedicalAppointmentDiagnosticTestService _medicalAppointmentDiagnosticTestService;

        public MedicalAppointmentController(IMedicalAppointmentService service, IMedicalAppointmentDiagnosticTestService medicalAppointmentDiagnosticTestService)
        {
            _medicalAppointmentService = service;
            _medicalAppointmentDiagnosticTestService = medicalAppointmentDiagnosticTestService;
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
