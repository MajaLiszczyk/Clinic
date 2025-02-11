﻿using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Services;
using ClinicAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService service)
        {
            _patientService = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {

            ReturnPatientDto? patient = await _patientService.GetPatient(id);
            if (patient != null)
                return Ok(patient);
             return NotFound();          
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<ReturnPatientDto> patients = await _patientService.GetAllPatients();
            if (patients != null)
                return Ok(patients);
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailable()
        {
            List<ReturnPatientDto> patients = await _patientService.GetAllAvailablePatients();
            if (patients != null)
                return Ok(patients);
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePatientDto patient)
        {
            var _patient = await _patientService.CreatePatient(patient);
            if (_patient.Confirmed)
                return Ok(new { message = _patient.Response });
            else return BadRequest(_patient.Response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatePatientDto patient)
        {
            var _patient = await _patientService.UpdatePatient(patient);
            if (_patient.Confirmed)
                return Ok(new { message = _patient.Response });
            else return BadRequest(_patient.Response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> TransferToArchive([FromRoute] int id)
        {
            var result = await _patientService.TransferToArchive(id);
            if (result.Confirmed)
                return Ok(new { message = result.Response });
            else return BadRequest(result.Response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _patientService.DeletePatient(id);
            if (result.Confirmed)
                return Ok(new { message = result.Response });
            else return BadRequest(result.Response);
        }
    }
}
