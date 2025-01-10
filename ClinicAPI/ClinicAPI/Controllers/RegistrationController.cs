﻿using ClinicAPI.DB;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Services;
using ClinicAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ClinicAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = (UserRole.Registrant + "," + UserRole.Admin))]
    public class RegistrationController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDBContext dbContext;
        private readonly IMedicalSpecialisationService _medicalSpecialisationService;
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;


        public RegistrationController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager
                                      , ApplicationDBContext dbContext, IMedicalSpecialisationService medicalSpecialisationService
                                      , IPatientService patientService, IDoctorService doctorService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.dbContext = dbContext;
            this._medicalSpecialisationService = medicalSpecialisationService;
            _patientService = patientService;
            _doctorService = doctorService;
        }

        [HttpPost]
        [Authorize(Roles = UserRole.Registrant)]
        public async Task<IActionResult> RegisterPatient([FromBody] CreateRegisterPatientDto request)
        {
            var result = await _patientService.RegisterPatient(request);
            if (!result.Confirmed)
            {
                return BadRequest(new { Message = result.Response });
            }

            //return Ok(new { Message = "Patient created successfully." });
            return Ok(new { message = result.Response });
        }

        [HttpPost]
        [Authorize(Roles = UserRole.Registrant)]
        public async Task<IActionResult> RegisterDoctor(CreateRegisterDoctorDto request)
        {
            var result = await _doctorService.RegisterDoctor(request);
            if (!result.Confirmed)
            {
                return BadRequest(new { Message = result.Response });
            }

            //return Ok(new { Message = "Patient created successfully." });
            return Ok(new { message = result.Response });
        }
    }
}
