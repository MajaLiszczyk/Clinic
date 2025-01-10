using ClinicAPI.Models;
using ClinicAPI.Services;
using ClinicAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LaboratoryTestTypeController : ControllerBase

    {
        private readonly ILaboratoryTestTypeService _testService;

        public LaboratoryTestTypeController(ILaboratoryTestTypeService service)
        {
            _testService = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            LaboratoryTestType? testType = await _testService.GetLaboratoryTestType(id);
            if (testType != null)
                return Ok(testType);
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<LaboratoryTestType> testTypes = await _testService.GetAllLaboratoryTestTypes();
            if (testTypes != null)
                return Ok(testTypes);
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(LaboratoryTestType testType)
        {
            var _testType = await _testService.CreateLaboratoryTestType(testType);
            if (_testType.Confirmed)
                return Ok(new { message = _testType.Response });
            else return BadRequest(_testType.Response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] LaboratoryTestType testType)
        {
            var _testType = await _testService.UpdateLaboratoryTestType(testType);
            if (_testType.Confirmed)
                return Ok(new { message = _testType.Response });
            else return BadRequest(_testType.Response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> TransferToArchive([FromRoute] int id)
        {
            var result = await _testService.TransferToArchive(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _testService.DeleteLaboratoryTestType(id);
            if (result.Confirmed)
                return Ok(new { message = result.Response });
            else return BadRequest(result.Response);
        }
    }
}
