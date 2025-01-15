using AutoMapper;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Services.Interfaces;
using System.Transactions;

namespace ClinicAPI.Services
{
    public class LaboratoryTestsGroupService : ILaboratoryTestsGroupService
    {
        private readonly ILaboratoryTestsGroupRepository _laboratoryTestsGroupRepository;
        private readonly ILaboratoryAppointmentRepository _laboratoryAppointmentRepository;
        private readonly IMapper _mapper;

        public LaboratoryTestsGroupService(ILaboratoryTestsGroupRepository laboratoryTestsGroupRepository
                                           , ILaboratoryAppointmentRepository laboratoryAppointmentRepository, IMapper mapper)
        {

            _mapper = mapper;
            _laboratoryTestsGroupRepository = laboratoryTestsGroupRepository;
            _laboratoryAppointmentRepository = laboratoryAppointmentRepository;

        }


        public async Task<(bool Confirmed, string Response)> UpdateMedicalSpecialisation(int groupId, int laboratoryAppointmentId)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                               new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                               TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                //sprawdzic czy istnieje grupa
                var updatedGroup = await _laboratoryTestsGroupRepository.GetTestsGroupById(groupId);
                if (updatedGroup == null)
                {
                    return (false, "Tests Group with this id does not exists.");
                }
                //sprawdzic czy do grupy nie ma juz jakiegos lab app przypisanego?
                if (updatedGroup.LaboratoryAppointmentId != null)
                {
                    return (false, "Tests Group already has laboratory appointment assigned.");
                }
                //sprawdzic czy istnieje lab app
                var labApp = await _laboratoryAppointmentRepository.GetLaboratoryAppointmentById(laboratoryAppointmentId);
                if (labApp == null)
                {
                    return (false, "Laboratory appointment with this id does not exists.");
                }
                updatedGroup.LaboratoryAppointmentId = laboratoryAppointmentId;
                var p = await _laboratoryTestsGroupRepository.UpdateLaboratoryTestsGroup(updatedGroup);
                labApp.State = LaboratoryAppointmentState.Reserved;
                var q = await _laboratoryAppointmentRepository.UpdateLaboratoryAppointment(labApp);


                scope.Complete();
                return await Task.FromResult((true, "Tests group succesfully uptated"));
            }
            catch (Exception ex)
            {
                return (false, $"Error updating Tests group: {ex.Message}");
            }

        }
    }
}
