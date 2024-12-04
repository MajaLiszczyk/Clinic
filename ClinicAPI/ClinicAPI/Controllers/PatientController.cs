using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Services;
using ClinicAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    //CONTROLLER
    //Przetwarzanie żądań HTTP i zwracanie odpowiednich odpowiedzi HTTP.
    //Obsługa kodów błędów(np. 404, 500) i komunikacja z serwisem.
    //Nie powinna zawierać logiki biznesowej ani mapowania danych.

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService service)
        {
            _patientService = service;
        }

        //[HttpGet("{id}"), Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {

            ReturnPatientDto? patient = await _patientService.GetPatient(id);
            if (patient != null)
                return Ok(patient);
             return NotFound();
              
        }

        //[HttpGet, Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            /*var measuresDtos = _patientService.Get();

            if (measuresDtos == null)
            {
                return new ForbidResult();
            }
            return Ok(measuresDtos); */
            //Console.WriteLine("ok");
            //return Ok();

            List<ReturnPatientDto> patients = await _patientService.GetAllPatients();
            if (patients != null)
                return Ok(patients);
            return NotFound();
        }

        //[HttpPost, Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePatientDto patient)
        //public async Task<IActionResult> Create(CreatePatientDto request)
        {
            //return Ok();

            var _patient = await _patientService.CreatePatient(patient);
            if (_patient.Confirmed)
                return Ok(new { message = _patient.Response });
                //return Ok(_patient.Response);
            else return BadRequest(_patient.Response);
        }

        //[HttpPut("{id}"), Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdatePatientDto patient)
        {
            var _patient = await _patientService.UpdatePatient(patient);
            if (_patient.Confirmed)
                return Ok(_patient.Response);
            else return BadRequest(_patient.Response);
        }

        //[HttpDelete("{id}"), Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _patientService.DeletePatient(id);
            if (result.Confirmed)
                return Ok(result.Response);
            else return BadRequest(result.Response);
        }
    }


}
