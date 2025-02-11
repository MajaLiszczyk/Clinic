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
    public class MedicalSpecialisationController : ControllerBase
    {
        private readonly IMedicalSpecialisationService _medicalSpecialisationService;

        public MedicalSpecialisationController(IMedicalSpecialisationService service)
        {
            _medicalSpecialisationService = service;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var result = await _medicalSpecialisationService.GetMedicalSpecialisation(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var result = await _medicalSpecialisationService.GetAllMedicalSpecialisations();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAvailable()
        {
            var result = await _medicalSpecialisationService.GetAllAvailableMedicalSpecialisations();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = UserRole.Registrant)]
        public async Task<IActionResult> Create(CreateMedicalSpecialisationDto request)
        {
            var result = await _medicalSpecialisationService.CreateMedicalSpecialisation(request);
            if (result.Confirmed)
                return Ok(new { message = result.Response, doctor = result.medSpecialisation });
            else return BadRequest(result.Response);
        }

        [HttpPut]
        [Authorize(Roles = UserRole.Registrant)]
        public async Task<IActionResult> Update([FromBody] UpdateMedicalSpecialisationDto request)
        {
            var result = await _medicalSpecialisationService.UpdateMedicalSpecialisation(request);
            if (result.Confirmed)
                return Ok(new { message = result.Response });
            else return BadRequest(result.Response);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = UserRole.Registrant)]
        public async Task<IActionResult> TransferToArchive([FromRoute] int id)
        {
            var result = await _medicalSpecialisationService.TransferToArchive(id);
            if (result.Confirmed)
                return Ok(new { message = result.Response });
            else return BadRequest(result.Response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = UserRole.Registrant)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _medicalSpecialisationService.DeleteMedicalSpecialisation(id);
            if (result.Confirmed)
                return Ok(new { message = result.Response });
            else return BadRequest(result.Response);
        }
    }
}
