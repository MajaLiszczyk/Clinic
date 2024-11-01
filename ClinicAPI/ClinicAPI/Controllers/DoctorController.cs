using ClinicAPI.Dtos;
using ClinicAPI.Services;
using ClinicAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        /*public DoctorController(IDoctorService service)
        {
            _doctorService = service;
        }*/

        //[HttpGet("{id}"), Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok();
/*
            var result = await _doctorService.GetDoctorAsync(id);
            if (result != null)
                return Ok(result);
            return NotFound();
   */     }

        //[HttpGet, Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
/*
            var result = await _doctorService.GetAllDoctorsAsync();
            if (result != null)
                return Ok(result);
            return NotFound();
  */      }

        //[HttpPost, Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateDoctorDto request)
        {
            /*var result = await _doctorService.CreateDoctorAsync(request);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);*/
            return Ok();
        }

        //[HttpPut("{id}"), Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateDoctorDto request, int id)
        {
            /*var result = await _doctorService.UpdateDoctorAsync(request, id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);*/
            return Ok();

        }

        //[HttpDelete("{id}"), Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            /*var result = await _doctorService.DeleteDoctorAsync(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);*/

            return Ok();

        }


    }
}
