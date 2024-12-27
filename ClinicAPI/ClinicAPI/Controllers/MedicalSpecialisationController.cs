using ClinicAPI.Dtos;
using ClinicAPI.Services;
using ClinicAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MedicalSpecialisationController : ControllerBase
    {
        private readonly IMedicalSpecialisationService _medicalSpecialisationService;

        public MedicalSpecialisationController(IMedicalSpecialisationService service)
        {
            _medicalSpecialisationService = service;
        }

        //[HttpGet("{id}"), Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var result = await _medicalSpecialisationService.GetMedicalSpecialisation(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        //[HttpGet, Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _medicalSpecialisationService.GetAllMedicalSpecialisations();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        //[HttpPost, Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateMedicalSpecialisationDto request)
        {
            var result = await _medicalSpecialisationService.CreateMedicalSpecialisation(request);
            if (result.Confirmed)
                return Ok(new { message = result.Response, doctor = result.medSpecialisation });
            //return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        //[HttpPut("{id}"), Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateMedicalSpecialisationDto request)
        {
            var result = await _medicalSpecialisationService.UpdateMedicalSpecialisation(request);
            if (result.Confirmed)
                //return Ok(result.Response);
                return Ok(new { message = result.Response });
            else return BadRequest(result.Response);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> TransferToArchive([FromRoute] int id)
        {
            var result = await _medicalSpecialisationService.TransferToArchive(id);
            if (result.Confirmed)
                //return Ok(result.Response);
                return Ok(new { message = result.Response });
            else return BadRequest(result.Response);
        }

        //[HttpDelete("{id}"), Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _medicalSpecialisationService.DeleteMedicalSpecialisation(id);
            if (result.Confirmed)
                //return Ok(result.Response);
                return Ok(new { message = result.Response });
            else return BadRequest(result.Response);
        }
    }
}
