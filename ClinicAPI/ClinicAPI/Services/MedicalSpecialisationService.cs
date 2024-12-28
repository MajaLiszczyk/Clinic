using AutoMapper;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Services.Interfaces;

namespace ClinicAPI.Services
{
    public class MedicalSpecialisationService : IMedicalSpecialisationService
    {
        private readonly IMedicalSpecialisationRepository _medicalSpecialisationRepository;
        private readonly IMapper _mapper;

        public MedicalSpecialisationService(IMedicalSpecialisationRepository medicalSpecialisationRepository, IMapper mapper)
        {
            _medicalSpecialisationRepository = medicalSpecialisationRepository;
            _mapper = mapper;

        }

        public async Task<ReturnMedicalSpecialisationDto?> GetMedicalSpecialisation(int id)
        {
            var medicalSpecialisation = await _medicalSpecialisationRepository.GetMedicalSpecialisationById(id);
            return _mapper.Map<ReturnMedicalSpecialisationDto>(medicalSpecialisation);
        }

        public async Task<MedicalSpecialisation?> GetRawSpecialisation(int id)
        {
            var medicalSpecialisation = await _medicalSpecialisationRepository.GetMedicalSpecialisationById(id);
            return medicalSpecialisation; ;
        }

        public async Task<List<ReturnMedicalSpecialisationDto>> GetAllMedicalSpecialisations()
        {
            var medicalSpecialisations = await _medicalSpecialisationRepository.GetAllMedicalSpecialisations();
            return _mapper.Map<List<ReturnMedicalSpecialisationDto>>(medicalSpecialisations);
        }

        public async Task<List<ReturnMedicalSpecialisationDto>> GetAllAvailableMedicalSpecialisations()
        {
            var medicalSpecialisations = await _medicalSpecialisationRepository.GetAllAvailableMedicalSpecialisations();
            return _mapper.Map<List<ReturnMedicalSpecialisationDto>>(medicalSpecialisations);
        }


        public async Task<(bool Confirmed, string Response, ReturnMedicalSpecialisationDto? medSpecialisation)> CreateMedicalSpecialisation(CreateMedicalSpecialisationDto medicalSpecialisation)
        {
            MedicalSpecialisation _medicalSpecialisation = new MedicalSpecialisation
            {
                Name = medicalSpecialisation.Name,
            };
            MedicalSpecialisation? p = await _medicalSpecialisationRepository.CreateMedicalSpecialisation(_medicalSpecialisation);
            if (p != null)
            {
                ReturnMedicalSpecialisationDto r = _mapper.Map<ReturnMedicalSpecialisationDto>(p);
                return await Task.FromResult((true, "MedicalSpecialisation successfully created.", r));
            }
            else
            {
                ReturnMedicalSpecialisationDto? k = null;
                return await Task.FromResult((false, "MedicalSpecialisation was not created.", k));
            }
        }
        
        public async Task<(bool Confirmed, string Response)> UpdateMedicalSpecialisation(UpdateMedicalSpecialisationDto medicalSpecialisation)
        {
            //MedicalSpecialisation? _medicalSpecialisation = await _medicalSpecialisationRepository.GetMedicalSpecialisationById(medicalSpecialisation.Id);
            var _medicalSpecialisation = await _medicalSpecialisationRepository.GetMedicalSpecialisationById(medicalSpecialisation.Id);

            if (_medicalSpecialisation == null)
            {
                return await Task.FromResult((false, "MedicalSpecialisation with given id does not exist."));
            }
            else
            {
                _medicalSpecialisation.Name = medicalSpecialisation.Name;
                //MedicalSpecialisation r = _mapper.Map<MedicalSpecialisation>(medicalSpecialisation);
                //var p = await _medicalSpecialisationRepository.UpdateMedicalSpecialisation(r);
                var p = await _medicalSpecialisationRepository.UpdateMedicalSpecialisation(_medicalSpecialisation);
                return await Task.FromResult((true, "MedicalSpecialisation succesfully uptated"));
            }
        }

        public async Task<(bool Confirmed, string Response)> TransferToArchive(int id)
        {
            var _medicalSpecialisation = await _medicalSpecialisationRepository.GetMedicalSpecialisationById(id);

            if (_medicalSpecialisation == null)
            {
                return await Task.FromResult((false, "MedicalSpecialisation with given id does not exist."));
            }
            else
            {
                _medicalSpecialisation.IsAvailable = false;
                //MedicalSpecialisation r = _mapper.Map<MedicalSpecialisation>(medicalSpecialisation);
                //var p = await _medicalSpecialisationRepository.UpdateMedicalSpecialisation(r);
                var p = await _medicalSpecialisationRepository.UpdateMedicalSpecialisation(_medicalSpecialisation);
                return await Task.FromResult((true, "MedicalSpecialisation succesfully uptated"));
            }
        }


        public async Task<(bool Confirmed, string Response)> DeleteMedicalSpecialisation(int id)
        {
            var medicalSpecialisation = await _medicalSpecialisationRepository.GetMedicalSpecialisationById(id);
            if (medicalSpecialisation == null) return await Task.FromResult((false, "MedicalSpecialisation with given id does not exist."));
            else
            {
                await _medicalSpecialisationRepository.DeleteMedicalSpecialisation(id);
                return await Task.FromResult((true, "MedicalSpecialisation successfully deleted."));
            }
        }
    }
}
