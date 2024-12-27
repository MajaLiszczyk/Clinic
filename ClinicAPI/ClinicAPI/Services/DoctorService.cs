using AutoMapper;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Numerics;

namespace ClinicAPI.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public DoctorService(IDoctorRepository doctorRepository, IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;

        }
        public async Task<ReturnDoctorDto?> GetDoctor(int id)
        {

            Doctor doctor = await _doctorRepository.GetDoctorById(id);
            return _mapper.Map<ReturnDoctorDto>(doctor);

        }
        public async Task<List<ReturnDoctorDto>> GetAllDoctors()
        {

            var doctors = await _doctorRepository.GetAllDoctors();
            return _mapper.Map<List<ReturnDoctorDto>>(doctors);
        }

        public async Task<List<ReturnDoctorDto>> GetAllAvailableDoctors()
        {
            var doctors = await _doctorRepository.GetAllAvailableDoctors();
            return _mapper.Map<List<ReturnDoctorDto>>(doctors);
        }

        public async Task<List<DoctorWithSpecialisations>> GetDoctorsWithSpecialisations()
        {

            var doctors = await _doctorRepository.GetDoctorsWithSpecialisations();
            return doctors;
            //return _mapper.Map<List<ReturnDoctorDto>>(doctors);
        }


        public async Task<(bool Confirmed, string Response, ReturnDoctorDto? doctor)> CreateDoctor(CreateDoctorDto request, ICollection<MedicalSpecialisation> medicalSpecialisations)
        {
            Doctor _doctor = new Doctor
            {
                Name = request.Name,
                Surname = request.Surname,
                DoctorNumber = request.DoctorNumber,
                MedicalSpecialisations = medicalSpecialisations
            };
            Doctor? p = await _doctorRepository.CreateDoctor(_doctor);
            if (p != null)
            {
                ReturnDoctorDto r = _mapper.Map<ReturnDoctorDto>(p);
                return await Task.FromResult((true, "doctor successfully created.", r));
            }
            else
            {
                ReturnDoctorDto? k = null;
                return await Task.FromResult((false, "doctor was not created.", k));
            }
        }

        public async Task<(bool Confirmed, string Response)> UpdateDoctor(UpdateDoctorDto doctor)
        {
            //Doctor? _doctor = await _doctorRepository.GetDoctorById(doctor.Id);
            var _doctor = await _doctorRepository.GetDoctorById(doctor.Id);

            if (_doctor == null)
            {
                return await Task.FromResult((false, "doctor with given id does not exist."));
            }
            else
            {
                try
                {
                    Doctor r = new Doctor
                    {
                        Name = doctor.Name,
                        Surname = doctor.Surname,
                        DoctorNumber = doctor.DoctorNumber,
                        MedicalSpecialisations = (ICollection<MedicalSpecialisation>)doctor.SpecialisationsList
                    };

                    //Doctor r = _mapper.Map<Doctor>(doctor);
                    var p = await _doctorRepository.UpdateDoctor(r);
                    return await Task.FromResult((true, "doctor succesfully uptated"));

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return await Task.FromResult((false, "doctor ERROR uptated"));

                }


            }
        }
        
        public async Task<(bool Confirmed, string Response)> TransferToArchive(int id)
        {
            var _doctor = await _doctorRepository.GetDoctorById(id);
            if (_doctor == null)
            {
                return await Task.FromResult((false, "doctor with given id does not exist."));
            }
            else
            {               
                try
                {
                    _doctor.IsAvailable = false;
                   

                    //Doctor r = _mapper.Map<Doctor>(doctor);
                    var p = await _doctorRepository.UpdateDoctor(_doctor);
                    return await Task.FromResult((true, "doctor succesfully uptated"));

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return await Task.FromResult((false, "doctor ERROR uptated"));

                }


            }
        }


        public async Task<(bool Confirmed, string Response)> DeleteDoctor(int id)
        {
            var doctor = await _doctorRepository.GetDoctorById(id);
            if (doctor == null) return await Task.FromResult((false, "doctor with given id does not exist."));
            else
            {
                await _doctorRepository.DeleteDoctor(id);
                return await Task.FromResult((true, "doctor successfully deleted."));
            }

        }
    }
}
