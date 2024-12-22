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

                /*if (medicalAppointment.dateTime < DateTime.Now)
                {
                    allAppointments.pastMedicalAppointments.Add(medicalAppointment);
                }

                else
                {
                    allAppointments.futureMedicalAppointments.Add(medicalAppointment);
                }*/

            }
            return allAppointments;
        }

        

        //public async Task<List<ReturnMedicalAppointmentDto>> GetMedicalAppointmentsByPatientId(int id)
        public async Task<MedicalAppointmentsOfPatient> GetMedicalAppointmentsByPatientId(int id)
        {
            MedicalAppointmentsOfPatient allAppointments = new MedicalAppointmentsOfPatient();
            //List<ReturnMedicalAppointmentDto> pastAppointment = new List<ReturnMedicalAppointmentDto>();
            //List<ReturnMedicalAppointmentDto> futureAppointments = new List<ReturnMedicalAppointmentDto>();

            var medicalAppointments = await _medicalAppointmentRepository.GetMedicalAppointmentsByPatientId(id);
            List<ReturnMedicalAppointmentDto> mappedAppointments = _mapper.Map<List<ReturnMedicalAppointmentDto>>(medicalAppointments);
            foreach (ReturnMedicalAppointmentDto medicalAppointment in mappedAppointments)
            {
                if(medicalAppointment.dateTime < DateTime.Now)
                {
                    allAppointments.pastMedicalAppointments.Add(medicalAppointment);
                }

                else
                {
                    allAppointments.futureMedicalAppointments.Add(medicalAppointment);
                }

            }
           // allAppointments.pastMedicalAppointments = pastAppointment;
            //allAppointments.futureMedicalAppointments = futureAppointments;

            return allAppointments;


            //return _mapper.Map<List<ReturnMedicalAppointmentDto>>(medicalAppointments);
        }

        


        public async Task<(bool Confirmed, string Response, ReturnMedicalAppointmentDto? medAppointment)> CreateMedicalAppointment(CreateMedicalAppointmentDto medicalAp)
        {
            //var medicalAppointment = _mapper.Map<MedicalAppointment>(medicalAp);
            MedicalAppointment _medicalAppointment = new MedicalAppointment
            {
                DateTime = new DateTime(medicalAp.Date.Year, medicalAp.Date.Month, medicalAp.Date.Day, medicalAp.Time.Hour, medicalAp.Time.Minute, 0),
                //dateTime = medicalAppointment.dateTime,
                PatientId = 0,
                //DoctorId = medicalAppointment.DoctorId,
                DoctorId = medicalAp.DoctorId,
                Interview = "domyslny interview",
                Diagnosis = "domyslne diagnosis",
                DiseaseUnit = 0,
                IsCancelled = false,
                IsFinished = false
               
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
            //MedicalAppointment? _medicalAppointment = await _medicalAppointmentRepository.GetMedicalAppointmentById(medicalAppointment.Id);
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
                _medicalAppointment.DiseaseUnit = medicalAppointment.DiseaseUnit;
                _medicalAppointment.DoctorId = medicalAppointment.DoctorId;
                _medicalAppointment.IsFinished = medicalAppointment.IsFinished;
                _medicalAppointment.IsCancelled = medicalAppointment.IsCancelled;
                //MedicalAppointment r = _mapper.Map<MedicalAppointment>(medicalAppointment);
                //var p = await _medicalAppointmentRepository.UpdateMedicalAppointment(r);
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
