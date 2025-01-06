using ClinicAPI.DB;
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


        public RegistrationController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager
                                      , ApplicationDBContext dbContext, IMedicalSpecialisationService medicalSpecialisationService
                                      , IPatientService patientService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.dbContext = dbContext;
            this._medicalSpecialisationService = medicalSpecialisationService;
            _patientService = patientService;
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
            // Tworzenie użytkownika
            var user = new User
            {
                UserName = request.Email,
                Email = request.Email
            };

            var createUserResult = await userManager.CreateAsync(user, request.Password);
            if (!createUserResult.Succeeded)
            {
                return BadRequest(createUserResult.Errors);
            }

            // Przypisanie roli Doctor do użytkownika
            var addToRoleResult = await userManager.AddToRoleAsync(user, UserRole.Doctor);
            if (!addToRoleResult.Succeeded)
            {
                return BadRequest(addToRoleResult.Errors);
            }

            ICollection<int> medicalSpecialisationsIds = request.MedicalSpecialisationsIds;
            ICollection<MedicalSpecialisation> medicalSpecialisations = new List<MedicalSpecialisation>();
            MedicalSpecialisation specialisation;
            foreach (int id in medicalSpecialisationsIds)
            {
                specialisation = await _medicalSpecialisationService.GetRawSpecialisation(id);
                medicalSpecialisations.Add(specialisation);
            }


            var doctor = new Doctor
            {
                UserId = user.Id,
                Name = request.Name,
                Surname = request.Surname,
                DoctorNumber = request.DoctorNumber,
                MedicalSpecialisations = medicalSpecialisations
            };

            dbContext.Doctor.Add(doctor);
            await dbContext.SaveChangesAsync();

            return Ok(new { Message = "Doctor registered successfully", DoctorId = doctor.Id });


        }
    }
}
