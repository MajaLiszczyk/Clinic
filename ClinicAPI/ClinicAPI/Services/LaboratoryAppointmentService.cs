using AutoMapper;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Services.Interfaces;
using System.Transactions;

namespace ClinicAPI.Services
{
    public class LaboratoryAppointmentService : ILaboratoryAppointmentService
    {
        private readonly ILaboratoryAppointmentRepository _laboratoryAppointmentRepository;
        private readonly IMapper _mapper;

        public LaboratoryAppointmentService(ILaboratoryAppointmentRepository laboratoryAppointmentRepository, IMapper mapper)
        {
            _laboratoryAppointmentRepository = laboratoryAppointmentRepository;
            _mapper = mapper;
        }

        public async Task<List<ReturnLaboratoryAppointmentDto>> GetAllLaboratoryAppointments()
        {
            var laboratoryAppointments = await _laboratoryAppointmentRepository.GetAllLaboratoryAppointments();
            return _mapper.Map<List<ReturnLaboratoryAppointmentDto>>(laboratoryAppointments);
        }

        public async Task<(bool Confirmed, string Response, ReturnLaboratoryAppointmentDto? medAppointment)> CreateLaboratoryAppointment(CreateLaboratoryAppointmentDto request)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var appointmentDate = new DateTime(request.Date.Year, request.Date.Month, request.Date.Day, request.Time.Hour, request.Time.Minute, 0);
                var todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                if (appointmentDate < todayDate)
                {
                    return (false, "The appointment date cannot be in the past.", null);
                }
                LaboratoryAppointment _laboratoryAppointment = new LaboratoryAppointment
                {
                    LaboratoryWorkerId = request.LaboratoryWorkerId,
                    SupervisorId = request.SupervisorId,
                    State = LaboratoryAppointmentState.Commissioned,
                    DateTime = new DateTime(request.Date.Year, request.Date.Month, request.Date.Day, request.Time.Hour, request.Time.Minute, 0),
                    CancelComment = ""
                };
                LaboratoryAppointment? p = await _laboratoryAppointmentRepository.CreateLaboratoryAppointment(_laboratoryAppointment);
                if (p == null)
                {
                    ReturnLaboratoryAppointmentDto? k = null;
                    return await Task.FromResult((false, "laboratoryAppointment was not created.", k));

                }
                ReturnLaboratoryAppointmentDto r = _mapper.Map<ReturnLaboratoryAppointmentDto>(p);
                scope.Complete();
                return await Task.FromResult((true, "laboratoryAppointment successfully created.", r));

            }
            catch (Exception ex)
            {
                return (false, $"Error cerating laboratory appointment: {ex.Message}", null);
            }
        }
        public async Task<(bool Confirmed, string Response)> UpdateLaboratoryAppointment(UpdateLaboratoryAppointmentDto request)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var _laboratoryAppointment = await _laboratoryAppointmentRepository.GetLaboratoryAppointmentById(request.Id);

                if (_laboratoryAppointment == null)
                {
                    return await Task.FromResult((false, "laboratoryAppointment with given id does not exist."));
                }
                _laboratoryAppointment.LaboratoryWorkerId = request.Id;
                _laboratoryAppointment.SupervisorId = request.SupervisorId;
                _laboratoryAppointment.State = request.State;
                _laboratoryAppointment.DateTime = request.DateTime;
                _laboratoryAppointment.CancelComment = request.CancelComment;
                var p = await _laboratoryAppointmentRepository.UpdateLaboratoryAppointment(_laboratoryAppointment);
                scope.Complete();
                return await Task.FromResult((true, "laboratoryAppointment succesfully uptated"));

            }
            catch (Exception ex)
            {
                return (false, $"Error updating laboratory appointment: {ex.Message}");
            }

        }
    }
}
