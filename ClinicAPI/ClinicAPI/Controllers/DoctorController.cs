using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Services;
using ClinicAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly IMedicalSpecialisationService _medicalSpecialisationService;

        public DoctorController(IDoctorService doctorService, IMedicalSpecialisationService medicalSpecialisationService)
        {
            _doctorService = doctorService;
            _medicalSpecialisationService = medicalSpecialisationService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var result = await _doctorService.GetDoctor(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _doctorService.GetAllDoctors();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailable()
        {
            var result = await _doctorService.GetAllAvailableDoctors();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetWithSpecialisations()
        {
            var result = await _doctorService.GetDoctorsWithSpecialisations();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDoctorDto request)
        {

            var _doctor = await _doctorService.CreateDoctorWithSpecialisations(request);
            if (_doctor.Confirmed)
                return Ok(new { message = _doctor.Response });
            else return BadRequest(_doctor.Response);        
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateDoctorDto request)
        {
            var _doctor = await _doctorService.UpdateDoctorWithSpecialisations(request);
            if (_doctor.Confirmed)
                return Ok(new { message = _doctor.Response });
            else return BadRequest(_doctor.Response);        
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> TransferToArchive([FromRoute] int id)
        {
            var result = await _doctorService.TransferToArchive(id);
            if (result.Confirmed)
                return Ok(new { message = result.Response }); 
            else return BadRequest(result.Response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _doctorService.DeleteDoctor(id);
            if (result.Confirmed)
                return Ok(new { message = result.Response }); 
            else return BadRequest(result.Response);
        }
    }
}
