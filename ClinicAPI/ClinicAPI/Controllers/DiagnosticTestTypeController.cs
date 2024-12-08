using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DiagnosticTestTypeController : ControllerBase
    {
        private readonly IDiagnosticTestTypeService _testService;

        public DiagnosticTestTypeController(IDiagnosticTestTypeService service)
        {
            _testService = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {

            DiagnosticTestType? testType = await _testService.GetDiagnosticTestType(id);
            if (testType != null)
                return Ok(testType);
            return NotFound();

        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {

            List<DiagnosticTestType> testTypes = await _testService.GetAllDiagnosticTestTypes();
            if (testTypes != null)
                return Ok(testTypes);
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DiagnosticTestType testType)
        {
            var _testType = await _testService.CreateDiagnosticTestType(testType);
            if (_testType.Confirmed)
                return Ok(new { message = _testType.Response });
            else return BadRequest(_testType.Response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] DiagnosticTestType testType)
        {
            var _testType = await _testService.UpdateDiagnosticTestType(testType);
            if (_testType.Confirmed)
                //return Ok(_patient.Response);
                return Ok(new { message = _testType.Response });
            else return BadRequest(_testType.Response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _testService.DeleteDiagnosticTestType(id);
            if (result.Confirmed)
                return Ok(new { message = result.Response });
            else return BadRequest(result.Response);
        }

    }
}
