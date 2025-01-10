using AutoMapper;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Services.Interfaces;
using System.Numerics;

namespace ClinicAPI.Services
{
    public class LaboratoryTestService : ILaboratoryTestService
    {
        private readonly ILaboratoryTestRepository _laboratoryTestRepository;
        private readonly IMapper _mapper;

        public LaboratoryTestService(ILaboratoryTestRepository laboratoryTestRepository, IMapper mapper)
        {
            _laboratoryTestRepository = laboratoryTestRepository;
            _mapper = mapper;

        } 
        public async Task<ReturnLaboratoryTestDto?> GetLaboratoryTest(int id)
        {
            LaboratoryTest laboratoryTest = await _laboratoryTestRepository.GetLaboratoryTestById(id);
            return _mapper.Map<ReturnLaboratoryTestDto>(laboratoryTest);
        }
        public async Task<List<ReturnLaboratoryTestDto>> GetAllLaboratoryTests()
        {
            var laboratoryTests = await _laboratoryTestRepository.GetAllLaboratoryTests();
            return _mapper.Map<List<ReturnLaboratoryTestDto>>(laboratoryTests);
        }
        public async Task<(bool Confirmed, string Response, ReturnLaboratoryTestDto? laboratoryTest)> CreateLaboratoryTest(CreateLaboratoryTestDto laboratoryTest)
        {
            LaboratoryTest _laboratoryTest = new LaboratoryTest
            {
                //MedicalAppointmentId = laboratoryTest.MedicalAppointmentId,
                //date = laboratoryTest.date,
                LaboratoryTestTypeId = laboratoryTest.LaboratoryTestTypeId,
                //LaboratoryWorkerId = laboratoryTest.LaboratoryWorkerId,
                //SupervisorId = laboratoryTest.SupervisorId,
                DoctorNote = laboratoryTest.DoctorNote,
            };
            LaboratoryTest? p = await _laboratoryTestRepository.CreateLaboratoryTest(_laboratoryTest);
            if (p != null)
            {
                ReturnLaboratoryTestDto r = _mapper.Map<ReturnLaboratoryTestDto>(p);
                return await Task.FromResult((true, "LaboratoryTest successfully created.", r));
            }
            else
            {
                ReturnLaboratoryTestDto? k = null;
                return await Task.FromResult((false, "laboratoryTest was not created.", k));

            }
        }
        public async Task<(bool Confirmed, string Response)> UpdateLaboratoryTest(UpdateLaboratoryTestDto laboratoryTest)
        {
            LaboratoryTest? _laboratoryTest = await _laboratoryTestRepository.GetLaboratoryTestById(laboratoryTest.Id);

            if (_laboratoryTest == null)
            {
                return await Task.FromResult((false, "LaboratoryTest with given id does not exist."));
            }
            else
            {
                LaboratoryTest r = _mapper.Map<LaboratoryTest>(laboratoryTest);
                var p = await _laboratoryTestRepository.UpdateLaboratoryTest(r);
                return await Task.FromResult((true, "LaboratoryTest succesfully uptated"));
            }
        }
        public async Task<(bool Confirmed, string Response)> DeleteLaboratoryTest(int id)
        {
            var laboratoryTest = await _laboratoryTestRepository.GetLaboratoryTestById(id);
            if (laboratoryTest == null) return await Task.FromResult((false, "laboratoryTest with given id does not exist."));
            else
            {
                await _laboratoryTestRepository.DeleteLaboratoryTest(id);
                return await Task.FromResult((true, "laboratoryTest successfully deleted."));
            }
        }




       
    }
}
