using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Services;
using ClinicAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RegistrantController : ControllerBase
    {
        private readonly IRegistrantService _registrantService;

        public RegistrantController(IRegistrantService service)
        {
            _registrantService = service;
        }

        //[HttpGet("{id}"), Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var result = await _registrantService.GetRegistrant(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        //[HttpGet, Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _registrantService.GetAllRegistrants();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        //[HttpPost, Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateRegistrantDto request)
        //public async Task<IActionResult> Create(CreateRegistrantDto request)
        {
            //return Ok();

            var result = await _registrantService.CreateRegistrant(request);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        //[HttpPut("{id}"), Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateRegistrantDto request)
        {
            var result = await _registrantService.UpdateRegistrant(request);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        //[HttpDelete("{id}"), Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _registrantService.DeleteRegistrant(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }
    }
}
