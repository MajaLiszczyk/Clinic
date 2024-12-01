using AutoMapper;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Services.Interfaces;
using System.Numerics;

namespace ClinicAPI.Services
{
    public class LaboratoryWorkerService : ILaboratoryWorkerService
    {
        private readonly ILaboratoryWorkerRepository _laboratoryWorkerRepository;
        private readonly IMapper _mapper;

        public LaboratoryWorkerService(ILaboratoryWorkerRepository laboratoryWorkerRepository, IMapper mapper)
        {
            _laboratoryWorkerRepository = laboratoryWorkerRepository;
            _mapper = mapper;

        }

        public async Task<ReturnLaboratoryWorkerDto?> GetLaboratoryWorker(int id)
        {
            var laboratoryWorker = await _laboratoryWorkerRepository.GetLaboratoryWorkerById(id);
            return _mapper.Map<ReturnLaboratoryWorkerDto>(laboratoryWorker);
        }

        public async Task<List<ReturnLaboratoryWorkerDto>> GetAllLaboratoryWorkers()
        {
            var laboratoryWorkers = await _laboratoryWorkerRepository.GetAllLaboratoryWorkers();
            return _mapper.Map<List<ReturnLaboratoryWorkerDto>>(laboratoryWorkers);
        }

        public async Task<(bool Confirmed, string Response, ReturnLaboratoryWorkerDto? laboratoryWorker)> CreateLaboratoryWorker(CreateLaboratoryWorkerDto laboratoryWorker)
        {
            LaboratoryWorker _laboratoryWorker = new LaboratoryWorker
            {
                Name = laboratoryWorker.Name,
                Surname = laboratoryWorker.Surname,
            };
            LaboratoryWorker? p = await _laboratoryWorkerRepository.CreateLaboratoryWorker(_laboratoryWorker);
            if (p != null)
            {
                ReturnLaboratoryWorkerDto r = _mapper.Map<ReturnLaboratoryWorkerDto>(p);
                return await Task.FromResult((true, "Patient successfully created.", r));
            }
            else
            {
                ReturnLaboratoryWorkerDto? k = null;
                return await Task.FromResult((false, "Patient was not created.", k));
            }
        }

        public async Task<(bool Confirmed, string Response)> UpdateLaboratoryWorker(UpdateLaboratoryWorkerDto laboratoryWorker)
        {
            LaboratoryWorker? _laboratoryWorker = await _laboratoryWorkerRepository.GetLaboratoryWorkerById(laboratoryWorker.Id);

            if (_laboratoryWorker == null)
            {
                return await Task.FromResult((false, "Patient with given id does not exist."));
            }
            else
            {
                LaboratoryWorker r = _mapper.Map<LaboratoryWorker>(laboratoryWorker);
                var p = await _laboratoryWorkerRepository.UpdateLaboratoryWorker(r);
                return await Task.FromResult((true, "Patient succesfully uptated"));
            }
        }

        public async Task<(bool Confirmed, string Response)> DeleteLaboratoryWorker(int id)
        {
            var laboratoryWorker = await _laboratoryWorkerRepository.GetLaboratoryWorkerById(id);
            if (laboratoryWorker == null) return await Task.FromResult((false, "Patient with given id does not exist."));
            else
            {
                await _laboratoryWorkerRepository.DeleteLaboratoryWorker(id);
                return await Task.FromResult((true, "Patient successfully deleted."));
            }
        }
    }
}
