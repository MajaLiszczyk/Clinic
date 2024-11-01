using ClinicAPI.Dtos;
using ClinicAPI.Services;
using ClinicAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LaboratoryTestController : ControllerBase
    {
        private readonly ILaboratoryTestService _laboratoryTestService;

        /*public LaboratoryTestController(ILaboratoryTestService service)
        {
            _laboratoryTestService = service;
        }*/

        //[HttpGet("{id}"), Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok();
/*
            var result = await _laboratoryTestService.GetLaboratoryTestAsync(id);
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
            var result = await _laboratoryTestService.GetAllLaboratoryTestsAsync();
            if (result != null)
                return Ok(result);
            return NotFound();
        */}

        //[HttpPost, Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateLaboratoryTestDto request)
        {
            return Ok();
/*
            var result = await _laboratoryTestService.CreateLaboratoryTestAsync(request);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
  */      }

        //[HttpPut("{id}"), Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateLaboratoryTestDto request, int id)
        {
            return Ok();
/*
            var result = await _laboratoryTestService.UpdateLaboratoryTestAsync(request, id);
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
            var result = await _laboratoryTestService.DeleteLaboratoryTestAsync(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
  */      }
    }
}
