using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Services;
using ClinicAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly IMedicalSpecialisationService _medicalSpecialisationService;

        public DoctorController(IDoctorService doctorService, IMedicalSpecialisationService medicalSpecialisationService)
        {
            _doctorService = doctorService;
            _medicalSpecialisationService = medicalSpecialisationService;
        }

        //[HttpGet("{id}"), Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var result = await _doctorService.GetDoctor(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        //[HttpGet, Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _doctorService.GetAllDoctors();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailable()
        {
            var result = await _doctorService.GetAllAvailableDoctors();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetWithSpecialisations()
        {
            var result = await _doctorService.GetDoctorsWithSpecialisations();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        //[HttpPost, Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateDoctorDto request)
        {
            ICollection<int> medicalSpecialisationsIds = request.MedicalSpecialisationsIds;
            ICollection<MedicalSpecialisation> medicalSpecialisations = new List<MedicalSpecialisation>();
            MedicalSpecialisation specialisation;
            foreach(int id in medicalSpecialisationsIds)
            {
                specialisation = await _medicalSpecialisationService.GetRawSpecialisation(id);
                medicalSpecialisations.Add(specialisation);
            }
            var result = await _doctorService.CreateDoctor(request, medicalSpecialisations);
            if (result.Confirmed)
                return Ok(new { message = result.Response, doctor = result.doctor });
                //return Ok(result.Response); //return Ok(new { message = result.Response }); //jeśli chcę zamienić stringa na json
            else return BadRequest(result.Response);
        }

        //[HttpPut("{id}"), Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateDoctorDto request)
        {

            ICollection<int> medicalSpecialisationsIds = request.MedicalSpecialisationsIds;
            ICollection<MedicalSpecialisation> medicalSpecialisations = new List<MedicalSpecialisation>();
            MedicalSpecialisation specialisation;
            foreach (int id in medicalSpecialisationsIds)
            {
                specialisation = await _medicalSpecialisationService.GetRawSpecialisation(id);
                medicalSpecialisations.Add(specialisation);
            }

            var result = await _doctorService.UpdateDoctor(request, medicalSpecialisations);
            if (result.Confirmed)
                //return Ok(result.Response);
                return Ok(new { message = result.Response });
            else return BadRequest(result.Response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> TransferToArchive([FromRoute] int id)
        {
            var result = await _doctorService.TransferToArchive(id);
            if (result.Confirmed)
                //return Ok(result.Response); // z tego nie zrobi sie json  -a tego oczekuje angular
                return Ok(new { message = result.Response }); //tu tworzy sie json
            else return BadRequest(result.Response);
        }

        //[HttpDelete("{id}"), Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _doctorService.DeleteDoctor(id);
            if (result.Confirmed)
                //return Ok(result.Response); // z tego nie zrobi sie json  -a tego oczekuje angular
                return Ok(new { message = result.Response }); //tu tworzy sie json
            else return BadRequest(result.Response);
        }


    }
}
