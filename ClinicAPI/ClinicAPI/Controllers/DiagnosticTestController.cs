﻿using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Services;
using ClinicAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DiagnosticTestController : ControllerBase
    {
        private readonly IDiagnosticTestService _diagnosticTestService;

        public DiagnosticTestController(IDiagnosticTestService service)
        {
            _diagnosticTestService = service;
        }


        //[HttpGet("{id}"), Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {

            ReturnDiagnosticTestDto? diagnosticTest = await _diagnosticTestService.GetDiagnosticTest(id);
            if (diagnosticTest != null)
                return Ok(diagnosticTest);
            return NotFound();
        }

        //[HttpGet, Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<ReturnDiagnosticTestDto> diagnosticTests = await _diagnosticTestService.GetAllDiagnosticTests();
            if (diagnosticTests != null)
                return Ok(diagnosticTests);
            return NotFound();
        }

        //[HttpPost, Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateDiagnosticTestDto diagnosticTest)
        {            
            var _diagnosticTest = await _diagnosticTestService.CreateDiagnosticTest(diagnosticTest);
            if (_diagnosticTest.Confirmed)
                return Ok(_diagnosticTest.Response);
            else return BadRequest(_diagnosticTest.Response);
        }

        //[HttpPut("{id}"), Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateDiagnosticTestDto diagnosticTest)
        {
          
           var _diagnosticTest = await _diagnosticTestService.UpdateDiagnosticTest(diagnosticTest);
           if (_diagnosticTest.Confirmed)
                return Ok(_diagnosticTest.Response);
           else return BadRequest(_diagnosticTest.Response);
        }

        //[HttpDelete("{id}"), Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {

            var diagnosticTest = await _diagnosticTestService.DeleteDiagnosticTest(id);
            if (diagnosticTest.Confirmed)
                return Ok(diagnosticTest.Response);
            else return BadRequest(diagnosticTest.Response);
        }

    }
}
