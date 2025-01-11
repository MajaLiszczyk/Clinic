using AutoMapper;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Services.Interfaces;
using System.Numerics;
using System.Transactions;

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

        //public async Task<List<ReturnLaboratoryTestDto>> GetLaboratoryTestsByPatientId(int id)
        public async Task<List<IGrouping<int, LaboratoryTest>>> GetLaboratoryTestsByPatientId(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                List<IGrouping<int, LaboratoryTest>> laboratoryTests = await _laboratoryTestRepository.GetLaboratoryTestsByPatientId(id);
                return laboratoryTests;
            }
            catch (Exception ex)
            {
                return new List<IGrouping<int, LaboratoryTest>>();
                //return (false, $"Error cerating laboratory appointment: {ex.Message}", null);
            }

            /*List<IGrouping<int, ReturnLaboratoryTestDto>> laboratoryTestsDto;
            foreach(var a in laboratoryTests)
            {

            }
            _mapper.Map<List<ReturnLaboratoryTestDto>>(laboratoryTests);*/
        }

        public async Task<List<ReturnLaboratoryTestDto>> GetLaboratoryTestsByMedicalAppointmentId(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var laboratoryTests = await _laboratoryTestRepository.GetLaboratoryTestsByMedicalAppointmentId(id);
                return _mapper.Map<List<ReturnLaboratoryTestDto>>(laboratoryTests);
            }
            catch (Exception ex)
            {
                return new List<ReturnLaboratoryTestDto>();
                //return (false, $"Error cerating laboratory appointment: {ex.Message}", null);
            }

        }


        public async Task<List<ReturnLaboratoryTestDto>> GetAllLaboratoryTests()
        {
            var laboratoryTests = await _laboratoryTestRepository.GetAllLaboratoryTests();
            return _mapper.Map<List<ReturnLaboratoryTestDto>>(laboratoryTests);
        }

        public async Task<(bool Confirmed, string Response, ReturnLaboratoryTestDto? laboratoryTest)> CreateLaboratoryTest(CreateLaboratoryTestDto laboratoryTest)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                LaboratoryTest _laboratoryTest = new LaboratoryTest
                {
                    LaboratoryTestTypeId = laboratoryTest.LaboratoryTestTypeId,
                    DoctorNote = laboratoryTest.DoctorNote,
                };
                LaboratoryTest? p = await _laboratoryTestRepository.CreateLaboratoryTest(_laboratoryTest);
                if (p == null)
                {
                    ReturnLaboratoryTestDto? k = null;
                    return await Task.FromResult((false, "laboratoryTest was not created.", k));

                }
                ReturnLaboratoryTestDto r = _mapper.Map<ReturnLaboratoryTestDto>(p);
                return await Task.FromResult((true, "LaboratoryTest successfully created.", r));
            }
            catch (Exception ex)
            {
                return (false, $"Error cerating laboratory test: {ex.Message}", null);
            }
            
        }
        public async Task<(bool Confirmed, string Response)> UpdateLaboratoryTest(UpdateLaboratoryTestDto laboratoryTest)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                LaboratoryTest? _laboratoryTest = await _laboratoryTestRepository.GetLaboratoryTestById(laboratoryTest.Id);

                if (_laboratoryTest == null)
                {
                    return await Task.FromResult((false, "LaboratoryTest with given id does not exist."));
                }
                LaboratoryTest r = _mapper.Map<LaboratoryTest>(laboratoryTest);
                var p = await _laboratoryTestRepository.UpdateLaboratoryTest(r);
                return await Task.FromResult((true, "LaboratoryTest succesfully uptated"));
            }
            catch (Exception ex)
            {
                return (false, $"Error updating laboratory test: {ex.Message}");
            }
            
        }
        public async Task<(bool Confirmed, string Response)> DeleteLaboratoryTest(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var laboratoryTest = await _laboratoryTestRepository.GetLaboratoryTestById(id);
                if (laboratoryTest == null)
                {
                    return await Task.FromResult((false, "laboratoryTest with given id does not exist."));
                }
                await _laboratoryTestRepository.DeleteLaboratoryTest(id);
                return await Task.FromResult((true, "laboratoryTest successfully deleted."));
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting laboratory test: {ex.Message}");
            }

        }




       
    }
}
