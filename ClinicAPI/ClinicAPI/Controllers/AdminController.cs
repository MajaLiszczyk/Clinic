using ClinicAPI.Dtos;
using ClinicAPI.Services;
using ClinicAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        /*public AdminController(IAdminService service)
        {
            _adminService = service;
        }*/
        public AdminController()
        {
        }

        //[HttpGet("{id}"), Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            /*var result = await _adminService.GetAdminAsync(id);
            if (result != null)
                return Ok(result);
            return NotFound();*/
            return Ok();

        }

        //[HttpGet, Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            /*var result = await _adminService.GetAllAdminsAsync();
            if (result != null)
                return Ok(result);
            return NotFound();*/
            return Ok();

        }

        //[HttpPost, Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateAdminDto request)
        {
            /* var result = await _adminService.CreateAdminAsync(request);
             if (result.Confirmed)
                 return Ok(result.Response);
             else return BadRequest(result.Response);*/
            return Ok();

        }

        //[HttpPut("{id}"), Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateAdminDto request, int id)
        {
            /*var result = await _adminService.UpdateAdminAsync(request, id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);*/
            return Ok();

        }

        //[HttpDelete("{id}"), Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            /*var result = await _adminService.DeleteAdminAsync(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);*/
            return Ok();

        }
    }
}
