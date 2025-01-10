using AutoMapper;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Services.Interfaces;

namespace ClinicAPI.Services
{
    public class MedicalAppointmentService : IMedicalAppointmentService
    {
        private readonly IMedicalAppointmentRepository _medicalAppointmentRepository;
        private readonly IMapper _mapper;

        public MedicalAppointmentService(IMedicalAppointmentRepository medicalAppointmentRepository, IMapper mapper)
        {
            _medicalAppointmentRepository = medicalAppointmentRepository;
            _mapper = mapper;

        }
        public async Task<ReturnMedicalAppointmentDto?> GetMedicalAppointment(int id)
        {
            var medicalAppointment = await _medicalAppointmentRepository.GetMedicalAppointmentById(id);
            return _mapper.Map<ReturnMedicalAppointmentDto>(medicalAppointment);
        }
        
        public async Task<List<ReturnMedicalAppointmentDto>> GetAllMedicalAppointments()
        {
            var medicalAppointments = await _medicalAppointmentRepository.GetAllMedicalAppointments();
            return _mapper.Map<List<ReturnMedicalAppointmentDto>>(medicalAppointments);
        }

        public async Task<List<ReturnMedicalAppointmentDto>> GetMedicalAppointmentsBySpecialisation(int id)
        {
            var medicalAppointments = await _medicalAppointmentRepository.GetMedicalAppointmentsBySpecialisation(id);
            return _mapper.Map<List<ReturnMedicalAppointmentDto>>(medicalAppointments);
        }

        public async Task<MedicalAppointmentsOfPatient> GetMedicalAppointmentsByDoctorId(int id)
        {
            MedicalAppointmentsOfPatient allAppointments = new MedicalAppointmentsOfPatient();
            var medicalAppointments = await _medicalAppointmentRepository.GetMedicalAppointmentsByDoctorId(id);
            List<ReturnMedicalAppointmentDto> mappedAppointments = _mapper.Map<List<ReturnMedicalAppointmentDto>>(medicalAppointments);
            foreach (ReturnMedicalAppointmentDto medicalAppointment in mappedAppointments)
            {
                if(medicalAppointment.IsFinished || medicalAppointment.IsCancelled)
                {
                    allAppointments.pastMedicalAppointments.Add(medicalAppointment);

                }
                else
                {
                    allAppointments.futureMedicalAppointments.Add(medicalAppointment);
                }

            }
            return allAppointments;
        }

        

        public async Task<MedicalAppointmentsOfPatient> GetMedicalAppointmentsByPatientId(int id)
        {
            MedicalAppointmentsOfPatient allAppointments = new MedicalAppointmentsOfPatient();


            var medicalAppointments = await _medicalAppointmentRepository.GetMedicalAppointmentsByPatientId(id);
            List<ReturnMedicalAppointmentDto> mappedAppointments = _mapper.Map<List<ReturnMedicalAppointmentDto>>(medicalAppointments);
            foreach (ReturnMedicalAppointmentDto medicalAppointment in mappedAppointments)
            {
                if (medicalAppointment.IsFinished)
                {
                    allAppointments.pastMedicalAppointments.Add(medicalAppointment);

                }
                else
                {
                    allAppointments.futureMedicalAppointments.Add(medicalAppointment);
                }

            }


            return allAppointments;


            //return _mapper.Map<List<ReturnMedicalAppointmentDto>>(medicalAppointments);
        }

        


        public async Task<(bool Confirmed, string Response, ReturnMedicalAppointmentDto? medAppointment)> CreateMedicalAppointment(CreateMedicalAppointmentDto medicalAp)
        {
            var appointmentDate = new DateTime(medicalAp.Date.Year, medicalAp.Date.Month, medicalAp.Date.Day, medicalAp.Time.Hour, medicalAp.Time.Minute, 0);
            var todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            if (appointmentDate < todayDate) 
            {
                return (false, "The appointment date cannot be in the past.", null);
            }
            //var medicalAppointment = _mapper.Map<MedicalAppointment>(medicalAp);
            MedicalAppointment _medicalAppointment = new MedicalAppointment
            {
                DateTime = new DateTime(medicalAp.Date.Year, medicalAp.Date.Month, medicalAp.Date.Day, medicalAp.Time.Hour, medicalAp.Time.Minute, 0),
                PatientId = 0,
                DoctorId = medicalAp.DoctorId,
                Interview = "domyslny interview",
                Diagnosis = "domyslne diagnosis",
                IsCancelled = false,
                IsFinished = false,
                CancellingComment = ""
               
            };
            MedicalAppointment? p = await _medicalAppointmentRepository.CreateMedicalAppointment(_medicalAppointment);
            if (p != null)
            {
                ReturnMedicalAppointmentDto r = _mapper.Map<ReturnMedicalAppointmentDto>(p);
                return await Task.FromResult((true, "medicalAppointment successfully created.", r));
            }
            else
            {
                ReturnMedicalAppointmentDto? k = null;
                return await Task.FromResult((false, "medicalAppointment was not created.", k));
            }
        }
        
        public async Task<(bool Confirmed, string Response)> UpdateMedicalAppointment(UpdateMedicalAppointmentDto medicalAppointment)
        {
            var _medicalAppointment = await _medicalAppointmentRepository.GetMedicalAppointmentById(medicalAppointment.Id);

            if (_medicalAppointment == null)
            {
                return await Task.FromResult((false, "medicalAppointment with given id does not exist."));
            }
            else
            {
                _medicalAppointment.DateTime = medicalAppointment.DateTime;
                _medicalAppointment.PatientId = medicalAppointment.PatientId;
                _medicalAppointment.Interview = medicalAppointment.Interview;
                _medicalAppointment.Diagnosis = medicalAppointment.Diagnosis;
                _medicalAppointment.DoctorId = medicalAppointment.DoctorId;
                _medicalAppointment.IsFinished = medicalAppointment.IsFinished;
                _medicalAppointment.IsCancelled = medicalAppointment.IsCancelled;
                _medicalAppointment.CancellingComment = medicalAppointment.CancellingComment;
                var p = await _medicalAppointmentRepository.UpdateMedicalAppointment(_medicalAppointment);
                return await Task.FromResult((true, "medicalAppointment succesfully uptated"));
            }
        }
        
        public async Task<(bool Confirmed, string Response)> DeleteMedicalAppointment(int id)
        {
            var medicalAppointment = await _medicalAppointmentRepository.GetMedicalAppointmentById(id);
            if (medicalAppointment == null) return await Task.FromResult((false, "medicalAppointment with given id does not exist."));
            else
            {
                await _medicalAppointmentRepository.DeleteMedicalAppointment(id);
                return await Task.FromResult((true, "medicalAppointment successfully deleted."));
            }
        }
    }
}
