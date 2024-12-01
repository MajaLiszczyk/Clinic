using AutoMapper;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Services.Interfaces;
using System.Numerics;

namespace ClinicAPI.Services
{
    public class LaboratorySupervisorService : ILaboratorySupervisorService
    {
        private readonly ILaboratorySupervisorRepository _laboratorySupervisorRepository;
        private readonly IMapper _mapper;

        public LaboratorySupervisorService(ILaboratorySupervisorRepository laboratorySupervisorRepository, IMapper mapper)
        {
            _laboratorySupervisorRepository = laboratorySupervisorRepository;
            _mapper = mapper;
        }

        public async Task<ReturnLaboratorySupervisorDto?> GetLaboratorySupervisor(int id)
        {
            var laboratorySupervisor = await _laboratorySupervisorRepository.GetLaboratorySupervisorById(id);
            return _mapper.Map<ReturnLaboratorySupervisorDto>(laboratorySupervisor);
        }

        public async Task<List<ReturnLaboratorySupervisorDto>> GetAllLaboratorySupervisors()
        {
            var laboratorySupervisor = await _laboratorySupervisorRepository.GetAllLaboratorySupervisors();
            return _mapper.Map<List<ReturnLaboratorySupervisorDto>>(laboratorySupervisor);
        }

        public async Task<(bool Confirmed, string Response, ReturnLaboratorySupervisorDto? supervisor)> CreateLaboratorySupervisor(CreateLaboratorySupervisorDto laboratorySupervisor)
        {
            LaboratorySupervisor _laboratorySupervisor = new LaboratorySupervisor()
            {
                Name = laboratorySupervisor.Name,
                Surname = laboratorySupervisor.Surname,
            };
            LaboratorySupervisor? p = await _laboratorySupervisorRepository.CreateLaboratorySupervisor(_laboratorySupervisor);
            if (p != null)
            {
                ReturnLaboratorySupervisorDto r = _mapper.Map<ReturnLaboratorySupervisorDto>(p);
                return await Task.FromResult((true, "Patient successfully created.", r));
            }
            else
            {
                ReturnLaboratorySupervisorDto? k = null;
                return await Task.FromResult((false, "Patient was not created.", k));
            }
        }
       
        public async Task<(bool Confirmed, string Response)> UpdateLaboratorySupervisor(UpdateLaboratorySupervisorDto laboratorySupervisor)
        {
            LaboratorySupervisor? _laboratorySupervisor = await _laboratorySupervisorRepository.GetLaboratorySupervisorById(laboratorySupervisor.Id);

            if (_laboratorySupervisor == null)
            {
                return await Task.FromResult((false, "Patient with given id does not exist."));
            }
            else
            {
                LaboratorySupervisor r = _mapper.Map<LaboratorySupervisor>(laboratorySupervisor);
                var p = await _laboratorySupervisorRepository.UpdateLaboratorySupervisor(r);
                return await Task.FromResult((true, "Patient succesfully uptated"));
            }
        }
        
        public async Task<(bool Confirmed, string Response)> DeleteLaboratorySupervisor(int id)
        {
            var laboratorySupervisor = await _laboratorySupervisorRepository.GetLaboratorySupervisorById(id);
            if (laboratorySupervisor == null) return await Task.FromResult((false, "Patient with given id does not exist."));
            else
            {
                await _laboratorySupervisorRepository.DeleteLaboratorySupervisor(id);
                return await Task.FromResult((true, "Patient successfully deleted."));
            }
        }


       
    }
}
