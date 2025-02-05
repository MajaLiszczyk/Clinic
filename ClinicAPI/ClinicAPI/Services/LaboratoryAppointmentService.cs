using AutoMapper;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Services.Interfaces;
using System.Security.Claims;
using System.Transactions;

namespace ClinicAPI.Services
{
    public class LaboratoryAppointmentService : ILaboratoryAppointmentService
    {
        private readonly ILaboratoryAppointmentRepository _laboratoryAppointmentRepository;
        private readonly ILaboratoryTestsGroupRepository _laboratoryTestsGroupRepository;
        private readonly ILaboratoryTestRepository _laboratoryTestsRepository;
        private readonly ILaboratoryWorkerRepository _laboratoryWorkerRepository;
        private readonly ILaboratorySupervisorRepository _laboratorySupervisorRepository;
        private readonly IMapper _mapper;

        public LaboratoryAppointmentService(ILaboratoryAppointmentRepository laboratoryAppointmentRepository
                                            , ILaboratoryTestsGroupRepository laboratoryTestsGroupRepository
                                            , ILaboratoryTestRepository laboratoryTestRepository
                                            , ILaboratoryWorkerRepository laboratoryWorkerRepository
                                            , ILaboratorySupervisorRepository laboratorySupervisorRepository
                                            , IMapper mapper)
        {
            _laboratoryAppointmentRepository = laboratoryAppointmentRepository;
            _laboratoryTestsGroupRepository = laboratoryTestsGroupRepository;
            _laboratoryTestsRepository = laboratoryTestRepository;
            _laboratoryWorkerRepository = laboratoryWorkerRepository;
            _laboratorySupervisorRepository = laboratorySupervisorRepository;
            _mapper = mapper;
        }

        public async Task<List<ReturnLaboratoryAppointmentDto>> GetAllLaboratoryAppointments()
        {
            var laboratoryAppointments = await _laboratoryAppointmentRepository.GetAllLaboratoryAppointments();
            return _mapper.Map<List<ReturnLaboratoryAppointmentDto>>(laboratoryAppointments);
        }

        public async Task<List<ReturnLaboratoryAppointmentWorkerSupervisorDto>> GetAllLaboratoryAppointmentsWorkersSupervisors()
        {
            var laboratoryAppointments = await _laboratoryAppointmentRepository.GetAllLaboratoryAppointmentsWorkersSupervisors();
            return laboratoryAppointments;
        }

        public async Task<ReturnLaboratoryAppointmentWithPatientWithTestsWithTypeNameWithMedAppDto> GetLabAppDetailsByLabAppId(int id, string userId, string role)
        {
            var laboratoryAppointment = await _laboratoryAppointmentRepository.GetLabAppDetailsByLabAppId(id);
            if (laboratoryAppointment == null)
            {
                throw new KeyNotFoundException("Nie znaleziono wizyty laboratoryjnej.");
            }

            if (role == UserRole.LaboratorySupervisor)
            {
                var laboratorySupervisor = await _laboratorySupervisorRepository.GetLaboratorySupervisorByUserId(userId);
                if (laboratorySupervisor == null || laboratorySupervisor.Id != laboratoryAppointment.SupervisorId)
                {
                    throw new UnauthorizedAccessException("Brak dostępu do tej wizyty laboratoryjnej.");
                }
            }
            else if (role == UserRole.LaboratoryWorker)
            {
                var laboratoryWorker = await _laboratoryWorkerRepository.GetLaboratoryWorkerByUserId(userId);
                if (laboratoryWorker == null || laboratoryWorker.Id != laboratoryAppointment.LaboratoryWorkerId)
                {
                    throw new UnauthorizedAccessException("Brak dostępu do tej wizyty laboratoryjnej.");
                }
            }
            else
            {
                throw new UnauthorizedAccessException("Nieznana rola użytkownika.");
            }
            return laboratoryAppointment;
        }



        public async Task<List<ReturnLaboratoryAppointmentDto>> GetAvailableLaboratoryAppointments()
        {
            var laboratoryAppointments = await _laboratoryAppointmentRepository.GetAvailableLaboratoryAppointments();
            return _mapper.Map<List<ReturnLaboratoryAppointmentDto>>(laboratoryAppointments);
        }   

        public async Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithTypeNameWithMedAppDto>> getPlannedLaboratoryAppointmentsByPatientId(int id)
        {
            var laboratoryAppointments = await _laboratoryAppointmentRepository.getPlannedLaboratoryAppointmentsByPatientId(id);
            return laboratoryAppointments;
        }

        public async Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithTypeNameWithMedAppDto>> getFinishedLaboratoryAppointmentsByPatientId(int id)
        {
            var laboratoryAppointments = await _laboratoryAppointmentRepository.getFinishedLaboratoryAppointmentsByPatientId(id);
            return laboratoryAppointments;
        }

        public async Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithTypeNameWithMedAppDto>> getInProcessLaboratoryAppointmentsByPatientId(int id)
        {
            var laboratoryAppointments = await _laboratoryAppointmentRepository.getInProcessLaboratoryAppointmentsByPatientId(id);
            return laboratoryAppointments;
        }

        //LAB WORKER
        public async Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getFutureLabAppsByLabWorkerId(int id)
        {
            var laboratoryAppointments = await _laboratoryAppointmentRepository.getSomeLabAppsByLabWorkerId(id, LaboratoryAppointmentState.Reserved);
            return laboratoryAppointments;
        }
        public async Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getWaitingForFillLabAppsByLabWorkerId(int id)
        {
            var laboratoryAppointments = await _laboratoryAppointmentRepository.getSomeLabAppsByLabWorkerId(id, LaboratoryAppointmentState.ToBeCompleted);
            return laboratoryAppointments;
        }
        public async Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getWaitingForSupervisorLabAppsByLabWorkerId(int id)
        {
            var laboratoryAppointments = await _laboratoryAppointmentRepository.getSomeLabAppsByLabWorkerId(id, LaboratoryAppointmentState.WaitingForSupervisor);
            return laboratoryAppointments;
        }
        public async Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getReadyForPatientLabAppsByLabWorkerId(int id)
        {
            var laboratoryAppointments = await _laboratoryAppointmentRepository.getSomeLabAppsByLabWorkerId(id, LaboratoryAppointmentState.AllAccepted);
            return laboratoryAppointments;
        }
        public async Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getToBeFixedLabAppsByLabWorkerId(int id)
        {
            var laboratoryAppointments = await _laboratoryAppointmentRepository.getSomeLabAppsByLabWorkerId(id, LaboratoryAppointmentState.ToBeFixed);
            return laboratoryAppointments;
        }
        public async Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getSentToPatientLabAppsByLabWorkerId(int id)
        {
            var laboratoryAppointments = await _laboratoryAppointmentRepository.getSomeLabAppsByLabWorkerId(id, LaboratoryAppointmentState.Finished);
            return laboratoryAppointments;
        }
        public async Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getCancelledLabAppsByLabWorkerId(int id)
        {
            var laboratoryAppointments = await _laboratoryAppointmentRepository.getSomeLabAppsByLabWorkerId(id, LaboratoryAppointmentState.Cancelled);
            return laboratoryAppointments;
        }
        //LAB WORKER

        //LAB SUPERVISOR
        public async Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> GetWaitingForReviewLabAppsBySupervisorId(int id)
        {
            var laboratoryAppointments = await _laboratoryAppointmentRepository.GetSomeLabAppsBySupervisorId(id, LaboratoryAppointmentState.WaitingForSupervisor);
            return laboratoryAppointments;
        }
        public async Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> GetAcceptedAndFinishedLabAppsBySupervisorId(int id)
        {
            var laboratoryAppointments = await _laboratoryAppointmentRepository.GetAcceptedAndFinishedLabAppsBySupervisorId(id);
            return laboratoryAppointments;
        }
        public async Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> GetSentBackLabAppsBySupervisorId(int id)
        {
            var laboratoryAppointments = await _laboratoryAppointmentRepository.GetSomeLabAppsBySupervisorId(id, LaboratoryAppointmentState.ToBeFixed);
            return laboratoryAppointments;
        }
        //LAB SUPERVISOR

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
                    State = LaboratoryAppointmentState.Empty,
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

        public async Task<(bool Confirmed, string Response)> MakeCancelledLaboratoryAppointment(int id, string cancelComment)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var _laboratoryAppointment = await _laboratoryAppointmentRepository.GetLaboratoryAppointmentById(id);

                if (_laboratoryAppointment == null)
                {
                    return await Task.FromResult((false, "laboratoryAppointment with given id does not exist."));
                }
                _laboratoryAppointment.State = LaboratoryAppointmentState.Cancelled;
                _laboratoryAppointment.CancelComment = cancelComment;
                var p = await _laboratoryAppointmentRepository.UpdateLaboratoryAppointment(_laboratoryAppointment);
                scope.Complete();
                return await Task.FromResult((true, "laboratoryAppointment succesfully uptated"));

            }
            catch (Exception ex)
            {
                return (false, $"Error updating laboratory appointment: {ex.Message}");
            }
        }

        public async Task<(bool Confirmed, string Response)> FinishLaboratoryAppointment(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var _laboratoryAppointment = await _laboratoryAppointmentRepository.GetLaboratoryAppointmentById(id);

                if (_laboratoryAppointment == null)
                {
                    return await Task.FromResult((false, "laboratoryAppointment with given id does not exist."));
                }
                _laboratoryAppointment.State = LaboratoryAppointmentState.ToBeCompleted;
                var p = await _laboratoryAppointmentRepository.UpdateLaboratoryAppointment(_laboratoryAppointment);
                var r = await _laboratoryTestsRepository.ChangeLaboratoryTestsStateByLabAppId(id, LaboratoryTestState.ToBeCompleted);
                scope.Complete();
                return await Task.FromResult((true, "laboratoryAppointment succesfully uptated"));
            }
            catch (Exception ex)
            {
                return (false, $"Error updating laboratory appointment: {ex.Message}");
            }
        }

        public async Task<(bool Confirmed, string Response)> SendLaboratoryTestsToSupervisor(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var _laboratoryAppointment = await _laboratoryAppointmentRepository.GetLaboratoryAppointmentById(id);

                if (_laboratoryAppointment == null)
                {
                    return await Task.FromResult((false, "laboratoryAppointment with given id does not exist."));
                }
                //TO DO : sprawdzic czy wszystkie wyniki są niepuste. Mozna sprawdzić stanami testów
                _laboratoryAppointment.State = LaboratoryAppointmentState.WaitingForSupervisor;
                var p = await _laboratoryAppointmentRepository.UpdateLaboratoryAppointment(_laboratoryAppointment);
                var r = await _laboratoryTestsRepository.ChangeLaboratoryTestsStateByLabAppId(id, LaboratoryTestState.WaitingForSupervisor);
                //zmienic status testow
                scope.Complete();
                return await Task.FromResult((true, "laboratoryAppointment succesfully uptated"));

            }
            catch (Exception ex)
            {
                return (false, $"Error updating laboratory appointment: {ex.Message}");
            }
        }

        public async Task<(bool Confirmed, string Response)> SendLaboratoryTestsToLaboratoryWorker(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var _laboratoryAppointment = await _laboratoryAppointmentRepository.GetLaboratoryAppointmentById(id);

                if (_laboratoryAppointment == null)
                {
                    return await Task.FromResult((false, "laboratoryAppointment with given id does not exist."));
                }
                var labTests = await _laboratoryTestsRepository.GetLaboratoryTestsByLabAppId(id);
                //TO DO : sprawdzic czy wszystkie wyniki są niepuste. Mozna sprawdzić stanami testów
                //sprawdzic czy wszystkie wyniki sa accpted, czy cos jest rejected
                int acceptedTestsCounter = 0;
                foreach (var test in labTests)
                {
                    if (test.State == LaboratoryTestState.Accepted)
                    {
                        acceptedTestsCounter++;
                    }
                    else if (test.State == LaboratoryTestState.Rejected)
                    {
                    }
                    else 
                    {
                        return await Task.FromResult((false, "laboratory test have not accetable state to send to supervisor"));
                    }
                }
                if(acceptedTestsCounter == labTests.Count())
                {
                    _laboratoryAppointment.State = LaboratoryAppointmentState.AllAccepted;
                }
                else
                {
                    _laboratoryAppointment.State = LaboratoryAppointmentState.ToBeFixed;
                }
                var p = await _laboratoryAppointmentRepository.UpdateLaboratoryAppointment(_laboratoryAppointment);
                scope.Complete();
                return await Task.FromResult((true, "laboratoryAppointment succesfully uptated"));
            }
            catch (Exception ex)
            {
                return (false, $"Error updating laboratory appointment: {ex.Message}");
            }
        }      

        public async Task<(bool Confirmed, string Response)> SendLaboratoryTestsResultsToPatient(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var _laboratoryAppointment = await _laboratoryAppointmentRepository.GetLaboratoryAppointmentById(id);

                if (_laboratoryAppointment == null)
                {
                    return await Task.FromResult((false, "laboratoryAppointment with given id does not exist."));
                }
                _laboratoryAppointment.State = LaboratoryAppointmentState.Finished; //wysłane do pacjenta
                var p = await _laboratoryAppointmentRepository.UpdateLaboratoryAppointment(_laboratoryAppointment);
                scope.Complete();
                return await Task.FromResult((true, "laboratoryAppointment succesfully uptated"));
            }
            catch (Exception ex)
            {
                return (false, $"Error updating laboratory appointment: {ex.Message}");
            }
        }

        public async Task<(bool Confirmed, string Response)> CancelPlannedAppointment(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var _laboratoryAppointment = await _laboratoryAppointmentRepository.GetLaboratoryAppointmentById(id);

                if (_laboratoryAppointment == null)
                {
                    return await Task.FromResult((false, "laboratoryAppointment with given id does not exist."));
                }
                _laboratoryAppointment.State = LaboratoryAppointmentState.Empty;
                var p = await _laboratoryAppointmentRepository.UpdateLaboratoryAppointment(_laboratoryAppointment);

                var _labTestsGroup = await _laboratoryTestsGroupRepository.getGroupByLabAppId(id);
                if (_labTestsGroup == null)
                {
                    return await Task.FromResult((false, "_labTestsGroup with given id does not exist."));
                }
                _labTestsGroup.LaboratoryAppointmentId = null;
                var d = await _laboratoryTestsGroupRepository.UpdateLaboratoryTestsGroup(_labTestsGroup);

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
