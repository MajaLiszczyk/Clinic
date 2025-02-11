﻿using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Services;
using ClinicAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DiagnosticTestController : ControllerBase
    {
        private readonly IDiagnosticTestService _diagnosticTestService;

        public DiagnosticTestController(IDiagnosticTestService service)
        {
            _diagnosticTestService = service;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = UserRole.Doctor)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            ReturnDiagnosticTestDto? diagnosticTest = await _diagnosticTestService.GetDiagnosticTest(id);
            if (diagnosticTest != null)
                return Ok(diagnosticTest);
            return NotFound();
        }

        [HttpGet]
        [Authorize(Roles = UserRole.Doctor)]
        public async Task<IActionResult> Get()
        {
            List<ReturnDiagnosticTestDto> diagnosticTests = await _diagnosticTestService.GetAllDiagnosticTests();
            if (diagnosticTests != null)
                return Ok(diagnosticTests);
            return NotFound();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = UserRole.Doctor)]
        public async Task<IActionResult> GetByMedicalAppointmentId([FromRoute] int id)
        {
            List<ReturnDiagnosticTestDto> diagnosticTests = await _diagnosticTestService.GetByMedicalAppointmentId(id);
            if (diagnosticTests != null)
                return Ok(diagnosticTests);
            return NotFound();
        }
        
        [HttpPost]
        [Authorize(Roles = UserRole.Doctor)]
        public async Task<IActionResult> Create(CreateDiagnosticTestDto diagnosticTest)
        {            
            var _diagnosticTest = await _diagnosticTestService.CreateDiagnosticTest(diagnosticTest);
            if (_diagnosticTest.Confirmed)
                return Ok(new { message = _diagnosticTest.Response, diagnosticTest = _diagnosticTest.diagnosticTest });
            else return BadRequest(_diagnosticTest.Response);
        }

        [HttpPut]
        [Authorize(Roles = UserRole.Doctor)]
        public async Task<IActionResult> Update([FromBody] UpdateDiagnosticTestDto diagnosticTest)
        {
          
           var _diagnosticTest = await _diagnosticTestService.UpdateDiagnosticTest(diagnosticTest);
           if (_diagnosticTest.Confirmed)
               return Ok(new { message = _diagnosticTest.Response });
            else return BadRequest(_diagnosticTest.Response);
        }
    }
}
