﻿using ClinicAPI.DB;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClinicAPI.Repositories
{
    public class LaboratoryAppointmentRepository : ILaboratoryAppointmentRepository
    {
        private readonly ApplicationDBContext _context;
        public LaboratoryAppointmentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<LaboratoryAppointment?> GetLaboratoryAppointmentById(int id)
        {
            return await _context.LaboratoryAppointment.Where(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<LaboratoryAppointment>> GetAllLaboratoryAppointments()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            List<LaboratoryAppointment> laboratoryAppointments = new List<LaboratoryAppointment>();
            try
            {
                laboratoryAppointments = await _context.LaboratoryAppointment.
                    ToListAsync();
                scope.Complete();
            }
            catch (Exception) { }
            return laboratoryAppointments;
        }

        public async Task<List<ReturnLaboratoryAppointmentWorkerSupervisorDto>> GetAllLaboratoryAppointmentsWorkersSupervisors()
        {
            var appointments = await(from ma in _context.LaboratoryAppointment
                                     join d in _context.LaboratoryWorker on ma.LaboratoryWorkerId equals d.Id
                                     join l in _context.LaboratorySupervisor on ma.SupervisorId equals l.Id
                                     select new ReturnLaboratoryAppointmentWorkerSupervisorDto
                                     {
                                         Id = ma.Id,
                                         DateTime = ma.DateTime,
                                         LaboratoryWorkerId = ma.LaboratoryWorkerId,
                                         LaboratoryWorkerName = d.Name,
                                         LaboratoryWorkerSurname = d.Surname,
                                         SupervisorId = ma.SupervisorId,
                                         SupervisorName = l.Name,
                                         SupervisorSurname = l.Surname,     
                                         State = ma.State,
                                         CancelComment = ma.CancelComment
                                     }).ToListAsync();
            return appointments;
        }

        public async Task<List<LaboratoryAppointment>> GetAvailableLaboratoryAppointments()
        {          
            List<LaboratoryAppointment> laboratoryAppointments = new List<LaboratoryAppointment>();

                laboratoryAppointments = await _context.LaboratoryAppointment.Where(r => r.State == LaboratoryAppointmentState.Empty).
                    ToListAsync();            
            return laboratoryAppointments;
        }

        

        public async Task<List<LaboratoryAppointment>> GetAllAailableLaboratoryAppointments()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            List<LaboratoryAppointment> laboratoryAppointments = new List<LaboratoryAppointment>();
            try
            {
                laboratoryAppointments = await _context.LaboratoryAppointment.Where(r => r.State == LaboratoryAppointmentState.Empty).
                    ToListAsync();
                scope.Complete();
            }
            catch (Exception) { }
            return laboratoryAppointments;
        }

        //DLA PACJENTA
        public async Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithTypeNameWithMedAppDto>> getPlannedLaboratoryAppointmentsByPatientId(int patientId)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var result = await (from labApp in _context.LaboratoryAppointment
                                    where labApp.State == LaboratoryAppointmentState.Reserved
                                    join labGroup in _context.LaboratoryTestsGroup
                                        on labApp.Id equals labGroup.LaboratoryAppointmentId
                                    join medApp in _context.MedicalAppointment
                                        on labGroup.MedicalAppointmentId equals medApp.Id
                                    join patient in _context.Patient
                                        on medApp.PatientId equals patient.Id
                                    join doctor in _context.Doctor
                                        on medApp.DoctorId equals doctor.Id
                                    where patient.Id == patientId
                                    select new
                                    {
                                        LaboratoryAppointment = labApp,
                                        Patient = patient,
                                        MedicalAppointment = medApp,
                                        Doctor = doctor,
                                        Tests = (from labTest in _context.LaboratoryTest
                                                 join testType in _context.LaboratoryTestType
                                                     on labTest.LaboratoryTestTypeId equals testType.Id
                                                 where labTest.LaboratoryTestsGroupId == labGroup.Id
                                                 select new ReturnLaboratoryTestWithTypeName
                                                 {
                                                     Id = labTest.Id,
                                                     LaboratoryTestsGroupId = labTest.LaboratoryTestsGroupId,
                                                     State = labTest.State,
                                                     LaboratoryTestTypeId = labTest.LaboratoryTestTypeId,
                                                     LaboratoryTestTypeName = testType.Name, 
                                                     Result = labTest.Result,
                                                     DoctorNote = labTest.DoctorNote,
                                                     RejectComment = labTest.RejectComment
                                                 }).ToList()
                                    })
                                   .ToListAsync();

                var mappedResult = result.Select(x => new ReturnLaboratoryAppointmentWithPatientWithTestsWithTypeNameWithMedAppDto
                {
                    // Medical appointment
                    MedicalAppointmentId = x.MedicalAppointment.Id,
                    MedicalAppointmentDateTime = x.MedicalAppointment.DateTime,
                    // Doctor
                    DoctorId = x.Doctor.Id,
                    DoctorName = x.Doctor.Name,
                    DoctorSurname = x.Doctor.Surname,
                    // Laboratory appointment
                    LaboratoryAppointmentId = x.LaboratoryAppointment.Id,
                    LaboratoryWorkerId = x.LaboratoryAppointment.LaboratoryWorkerId,
                    SupervisorId = x.LaboratoryAppointment.SupervisorId,
                    State = x.LaboratoryAppointment.State,
                    DateTime = x.LaboratoryAppointment.DateTime,
                    CancelComment = x.LaboratoryAppointment.CancelComment,
                    // Patient
                    PatientId = x.Patient.Id,
                    PatientName = x.Patient.Name,
                    PatientSurname = x.Patient.Surname,
                    PatientPesel = x.Patient.Pesel,
                    // Laboratory tests
                    laboratoryTests = x.Tests
                }).ToList();

                scope.Complete();
                return mappedResult;
            }
            catch (Exception)
            {
                scope.Dispose();
                throw;
            }
        }

        public async Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithTypeNameWithMedAppDto>> getFinishedLaboratoryAppointmentsByPatientId(int patientId)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var result = await (from labApp in _context.LaboratoryAppointment
                                    where labApp.State == LaboratoryAppointmentState.Finished
                                    join labGroup in _context.LaboratoryTestsGroup
                                        on labApp.Id equals labGroup.LaboratoryAppointmentId
                                    join medApp in _context.MedicalAppointment
                                        on labGroup.MedicalAppointmentId equals medApp.Id
                                    join patient in _context.Patient
                                        on medApp.PatientId equals patient.Id
                                    join doctor in _context.Doctor
                                        on medApp.DoctorId equals doctor.Id
                                    where patient.Id == patientId
                                    select new
                                    {
                                        LaboratoryAppointment = labApp,
                                        Patient = patient,
                                        MedicalAppointment = medApp,
                                        Doctor = doctor,
                                        Tests = (from labTest in _context.LaboratoryTest
                                                 join testType in _context.LaboratoryTestType
                                                     on labTest.LaboratoryTestTypeId equals testType.Id
                                                 where labTest.LaboratoryTestsGroupId == labGroup.Id
                                                 select new ReturnLaboratoryTestWithTypeName
                                                 {
                                                     Id = labTest.Id,
                                                     LaboratoryTestsGroupId = labTest.LaboratoryTestsGroupId,
                                                     State = labTest.State,
                                                     LaboratoryTestTypeId = labTest.LaboratoryTestTypeId,
                                                     LaboratoryTestTypeName = testType.Name,
                                                     Result = labTest.Result,
                                                     DoctorNote = labTest.DoctorNote,
                                                     RejectComment = labTest.RejectComment
                                                 }).ToList()
                                    })
                                   .ToListAsync();

                var mappedResult = result.Select(x => new ReturnLaboratoryAppointmentWithPatientWithTestsWithTypeNameWithMedAppDto
                {
                    // Medical appointment
                    MedicalAppointmentId = x.MedicalAppointment.Id,
                    MedicalAppointmentDateTime = x.MedicalAppointment.DateTime,
                    // Doctor
                    DoctorId = x.Doctor.Id,
                    DoctorName = x.Doctor.Name,
                    DoctorSurname = x.Doctor.Surname,
                    // Laboratory appointment
                    LaboratoryAppointmentId = x.LaboratoryAppointment.Id,
                    LaboratoryWorkerId = x.LaboratoryAppointment.LaboratoryWorkerId,
                    SupervisorId = x.LaboratoryAppointment.SupervisorId,
                    State = x.LaboratoryAppointment.State,
                    DateTime = x.LaboratoryAppointment.DateTime,
                    CancelComment = x.LaboratoryAppointment.CancelComment,
                    // Patient
                    PatientId = x.Patient.Id,
                    PatientName = x.Patient.Name,
                    PatientSurname = x.Patient.Surname,
                    PatientPesel = x.Patient.Pesel,
                    // Laboratory tests
                    laboratoryTests = x.Tests
                }).ToList();

                scope.Complete();
                return mappedResult;
            }
            catch (Exception)
            {
                scope.Dispose();
                throw;
            }
        }

        public async Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithTypeNameWithMedAppDto>> getInProcessLaboratoryAppointmentsByPatientId(int patientId)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var result = await (from labApp in _context.LaboratoryAppointment
                                    where (labApp.State == LaboratoryAppointmentState.ToBeCompleted || labApp.State == LaboratoryAppointmentState.WaitingForSupervisor
                                    || labApp.State == LaboratoryAppointmentState.ToBeFixed || labApp.State == LaboratoryAppointmentState.AllAccepted)
                                    join labGroup in _context.LaboratoryTestsGroup
                                        on labApp.Id equals labGroup.LaboratoryAppointmentId
                                    join medApp in _context.MedicalAppointment
                                        on labGroup.MedicalAppointmentId equals medApp.Id
                                    join patient in _context.Patient
                                        on medApp.PatientId equals patient.Id
                                    join doctor in _context.Doctor
                                        on medApp.DoctorId equals doctor.Id
                                    where patient.Id == patientId
                                    select new
                                    {
                                        LaboratoryAppointment = labApp,
                                        Patient = patient,
                                        MedicalAppointment = medApp,
                                        Doctor = doctor,
                                        Tests = (from labTest in _context.LaboratoryTest
                                                 join testType in _context.LaboratoryTestType
                                                     on labTest.LaboratoryTestTypeId equals testType.Id
                                                 where labTest.LaboratoryTestsGroupId == labGroup.Id
                                                 select new ReturnLaboratoryTestWithTypeName
                                                 {
                                                     Id = labTest.Id,
                                                     LaboratoryTestsGroupId = labTest.LaboratoryTestsGroupId,
                                                     State = labTest.State,
                                                     LaboratoryTestTypeId = labTest.LaboratoryTestTypeId,
                                                     LaboratoryTestTypeName = testType.Name, 
                                                     Result = labTest.Result,
                                                     DoctorNote = labTest.DoctorNote,
                                                     RejectComment = labTest.RejectComment
                                                 }).ToList()
                                    })
                                   .ToListAsync();

                var mappedResult = result.Select(x => new ReturnLaboratoryAppointmentWithPatientWithTestsWithTypeNameWithMedAppDto
                {
                    // Medical appointment
                    MedicalAppointmentId = x.MedicalAppointment.Id,
                    MedicalAppointmentDateTime = x.MedicalAppointment.DateTime,
                    // Doctor
                    DoctorId = x.Doctor.Id,
                    DoctorName = x.Doctor.Name,
                    DoctorSurname = x.Doctor.Surname,
                    // Laboratory appointment
                    LaboratoryAppointmentId = x.LaboratoryAppointment.Id,
                    LaboratoryWorkerId = x.LaboratoryAppointment.LaboratoryWorkerId,
                    SupervisorId = x.LaboratoryAppointment.SupervisorId,
                    State = x.LaboratoryAppointment.State,
                    DateTime = x.LaboratoryAppointment.DateTime,
                    CancelComment = x.LaboratoryAppointment.CancelComment,
                    // Patient
                    PatientId = x.Patient.Id,
                    PatientName = x.Patient.Name,
                    PatientSurname = x.Patient.Surname,
                    PatientPesel = x.Patient.Pesel,
                    // Laboratory tests
                    laboratoryTests = x.Tests
                }).ToList();

                scope.Complete();
                return mappedResult;
            }
            catch (Exception)
            {
                scope.Dispose();
                throw;
            }
        }
        //DLA PACJENTA

        //DLA SUPERVISORA
        public async Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> GetSomeLabAppsBySupervisorId(int id, LaboratoryAppointmentState labAppState)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var result = await (from labApp in _context.LaboratoryAppointment
                                    where labApp.State == labAppState
                                    where labApp.SupervisorId == id
                                    join labGroup in _context.LaboratoryTestsGroup
                                        on labApp.Id equals labGroup.LaboratoryAppointmentId
                                    join medApp in _context.MedicalAppointment
                                        on labGroup.MedicalAppointmentId equals medApp.Id
                                    join patient in _context.Patient
                                        on medApp.PatientId equals patient.Id
                                    join doctor in _context.Doctor
                                        on medApp.DoctorId equals doctor.Id
                                    select new
                                    {
                                        LaboratoryAppointment = labApp,
                                        Patient = patient,
                                        MedicalAppointment = medApp,
                                        Doctor = doctor,
                                        Tests = (from labTest in _context.LaboratoryTest
                                                 where labTest.LaboratoryTestsGroupId == labGroup.Id
                                                 select labTest).ToList()
                                    })
                                   .ToListAsync();

                var mappedResult = result.Select(x => new ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto
                {
                    // Medical appointment
                    MedicalAppointmentId = x.MedicalAppointment.Id,
                    MedicalAppointmentDateTime = x.MedicalAppointment.DateTime,
                    // Doctor
                    DoctorId = x.Doctor.Id,
                    DoctorName = x.Doctor.Name,
                    DoctorSurname = x.Doctor.Surname,
                    // Laboratory appointment
                    LaboratoryAppointmentId = x.LaboratoryAppointment.Id,
                    LaboratoryWorkerId = x.LaboratoryAppointment.LaboratoryWorkerId,
                    SupervisorId = x.LaboratoryAppointment.SupervisorId,
                    State = x.LaboratoryAppointment.State,
                    DateTime = x.LaboratoryAppointment.DateTime,
                    CancelComment = x.LaboratoryAppointment.CancelComment,
                    // Patient
                    PatientId = x.Patient.Id,
                    PatientName = x.Patient.Name,
                    PatientSurname = x.Patient.Surname,
                    PatientPesel = x.Patient.Pesel,
                    // Laboratory tests
                    laboratoryTests = x.Tests
                }).ToList();

                scope.Complete();
                return mappedResult;
            }
            catch (Exception)
            {
                scope.Dispose();
                throw;
            }
        }

        public async Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> GetAcceptedAndFinishedLabAppsBySupervisorId(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var result = await (from labApp in _context.LaboratoryAppointment
                                    where labApp.State == LaboratoryAppointmentState.AllAccepted 
                                    || labApp.State == LaboratoryAppointmentState.Finished
                                    where labApp.SupervisorId == id
                                    join labGroup in _context.LaboratoryTestsGroup
                                        on labApp.Id equals labGroup.LaboratoryAppointmentId
                                    join medApp in _context.MedicalAppointment
                                        on labGroup.MedicalAppointmentId equals medApp.Id
                                    join patient in _context.Patient
                                        on medApp.PatientId equals patient.Id
                                    join doctor in _context.Doctor
                                        on medApp.DoctorId equals doctor.Id
                                    select new
                                    {
                                        LaboratoryAppointment = labApp,
                                        Patient = patient,
                                        MedicalAppointment = medApp,
                                        Doctor = doctor,
                                        Tests = (from labTest in _context.LaboratoryTest
                                                 where labTest.LaboratoryTestsGroupId == labGroup.Id
                                                 select labTest).ToList()
                                    })
                                   .ToListAsync();

                var mappedResult = result.Select(x => new ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto
                {
                    // Medical appointment
                    MedicalAppointmentId = x.MedicalAppointment.Id,
                    MedicalAppointmentDateTime = x.MedicalAppointment.DateTime,
                    // Doctor
                    DoctorId = x.Doctor.Id,
                    DoctorName = x.Doctor.Name,
                    DoctorSurname = x.Doctor.Surname,
                    // Laboratory appointment
                    LaboratoryAppointmentId = x.LaboratoryAppointment.Id,
                    LaboratoryWorkerId = x.LaboratoryAppointment.LaboratoryWorkerId,
                    SupervisorId = x.LaboratoryAppointment.SupervisorId,
                    State = x.LaboratoryAppointment.State,
                    DateTime = x.LaboratoryAppointment.DateTime,
                    CancelComment = x.LaboratoryAppointment.CancelComment,
                    // Patient
                    PatientId = x.Patient.Id,
                    PatientName = x.Patient.Name,
                    PatientSurname = x.Patient.Surname,
                    PatientPesel = x.Patient.Pesel,
                    // Laboratory tests
                    laboratoryTests = x.Tests
                }).ToList();

                scope.Complete();
                return mappedResult;
            }
            catch (Exception)
            {
                scope.Dispose();
                throw;
            }
        }
        //DLA SUPERVISORA

        //DLA LABORATORY WORKER
        public async Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getSomeLabAppsByLabWorkerId(int id, LaboratoryAppointmentState labAppState)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var result = await (from labApp in _context.LaboratoryAppointment
                                    where labApp.State == labAppState
                                    where labApp.LaboratoryWorkerId == id
                                    join labGroup in _context.LaboratoryTestsGroup
                                        on labApp.Id equals labGroup.LaboratoryAppointmentId
                                    join medApp in _context.MedicalAppointment
                                        on labGroup.MedicalAppointmentId equals medApp.Id
                                    join patient in _context.Patient
                                        on medApp.PatientId equals patient.Id
                                    join doctor in _context.Doctor
                                        on medApp.DoctorId equals doctor.Id
                                    select new
                                    {
                                        LaboratoryAppointment = labApp,
                                        Patient = patient,
                                        MedicalAppointment = medApp,
                                        Doctor = doctor,
                                        Tests = (from labTest in _context.LaboratoryTest
                                                 where labTest.LaboratoryTestsGroupId == labGroup.Id
                                                 select labTest).ToList()
                                    })
                                   .ToListAsync();

                var mappedResult = result.Select(x => new ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto
                {
                    // Medical appointment
                    MedicalAppointmentId = x.MedicalAppointment.Id,
                    MedicalAppointmentDateTime = x.MedicalAppointment.DateTime,
                    // Doctor
                    DoctorId = x.Doctor.Id,
                    DoctorName = x.Doctor.Name,
                    DoctorSurname = x.Doctor.Surname,
                    // Laboratory appointment
                    LaboratoryAppointmentId = x.LaboratoryAppointment.Id,
                    LaboratoryWorkerId = x.LaboratoryAppointment.LaboratoryWorkerId,
                    SupervisorId = x.LaboratoryAppointment.SupervisorId,
                    State = x.LaboratoryAppointment.State,
                    DateTime = x.LaboratoryAppointment.DateTime,
                    CancelComment = x.LaboratoryAppointment.CancelComment,
                    // Patient
                    PatientId = x.Patient.Id,
                    PatientName = x.Patient.Name,
                    PatientSurname = x.Patient.Surname,
                    PatientPesel = x.Patient.Pesel,
                    // Laboratory tests
                    laboratoryTests = x.Tests
                }).ToList();

                scope.Complete();
                return mappedResult;
            }
            catch (Exception)
            {
                scope.Dispose();
                throw;
            }
        }



        //pobranie wszytskim zarezerwowanych lab appointment dla danego lab worker
        public async Task<List<ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto>> getAllReservededLabApposByLabWorkerId(int laboratoryWorkerId)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var result = await (from labApp in _context.LaboratoryAppointment
                                    where labApp.State == LaboratoryAppointmentState.Reserved
                                    where labApp.LaboratoryWorkerId == laboratoryWorkerId
                                    join labGroup in _context.LaboratoryTestsGroup
                                        on labApp.Id equals labGroup.LaboratoryAppointmentId
                                    join medApp in _context.MedicalAppointment
                                        on labGroup.MedicalAppointmentId equals medApp.Id
                                    join patient in _context.Patient
                                        on medApp.PatientId equals patient.Id
                                    join doctor in _context.Doctor
                                        on medApp.DoctorId equals doctor.Id
                                    select new
                                    {
                                        LaboratoryAppointment = labApp,
                                        Patient = patient,
                                        MedicalAppointment = medApp,
                                        Doctor = doctor,
                                        Tests = (from labTest in _context.LaboratoryTest
                                                 where labTest.LaboratoryTestsGroupId == labGroup.Id
                                                 select labTest).ToList()
                                    })
                                   .ToListAsync();

                var mappedResult = result.Select(x => new ReturnLaboratoryAppointmentWithPatientWithTestsWithMedAppDto
                {
                    // Medical appointment
                    MedicalAppointmentId = x.MedicalAppointment.Id,
                    MedicalAppointmentDateTime = x.MedicalAppointment.DateTime,
                    // Doctor
                    DoctorId = x.Doctor.Id,
                    DoctorName = x.Doctor.Name,
                    DoctorSurname = x.Doctor.Surname,
                    // Laboratory appointment
                    LaboratoryAppointmentId = x.LaboratoryAppointment.Id,
                    LaboratoryWorkerId = x.LaboratoryAppointment.LaboratoryWorkerId,
                    SupervisorId = x.LaboratoryAppointment.SupervisorId,
                    State = x.LaboratoryAppointment.State,
                    DateTime = x.LaboratoryAppointment.DateTime,
                    CancelComment = x.LaboratoryAppointment.CancelComment,
                    // Patient
                    PatientId = x.Patient.Id,
                    PatientName = x.Patient.Name,
                    PatientSurname = x.Patient.Surname,
                    PatientPesel = x.Patient.Pesel,
                    // Laboratory tests
                    laboratoryTests = x.Tests
                }).ToList();

                scope.Complete();
                return mappedResult;
            }
            catch (Exception)
            {
                scope.Dispose();
                throw;
            }
        }

        public async Task<ReturnLaboratoryAppointmentWithPatientWithTestsWithTypeNameWithMedAppDto?> GetLabAppDetailsByLabAppId(int laboratoryAppointmentId)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var result = await (from labApp in _context.LaboratoryAppointment
                                    where labApp.Id == laboratoryAppointmentId
                                    join labGroup in _context.LaboratoryTestsGroup
                                        on labApp.Id equals labGroup.LaboratoryAppointmentId
                                    join medApp in _context.MedicalAppointment
                                        on labGroup.MedicalAppointmentId equals medApp.Id
                                    join patient in _context.Patient
                                        on medApp.PatientId equals patient.Id
                                    join doctor in _context.Doctor
                                        on medApp.DoctorId equals doctor.Id
                                    select new
                                    {
                                        LaboratoryAppointment = labApp,
                                        Patient = patient,
                                        MedicalAppointment = medApp,
                                        Doctor = doctor,
                                        Tests = (from labTest in _context.LaboratoryTest
                                                 where labTest.LaboratoryTestsGroupId == labGroup.Id
                                                 join labType in _context.LaboratoryTestType
                                                    on labTest.LaboratoryTestTypeId equals labType.Id 
                                                 select new ReturnLaboratoryTestWithTypeName
                                                 {
                                                     Id = labTest.Id,
                                                     LaboratoryTestsGroupId = labGroup.Id,
                                                     State = labTest.State,
                                                     Result = labTest.Result,
                                                     DoctorNote = labTest.DoctorNote,
                                                     RejectComment = labTest.RejectComment,
                                                     LaboratoryTestTypeName = labType.Name,
                                                     LaboratoryTestTypeId = labType.Id
                                                 }).ToList()
                                    })
                                   .SingleOrDefaultAsync();

                if (result == null)
                {
                    return null;
                }

                var mappedResult = new ReturnLaboratoryAppointmentWithPatientWithTestsWithTypeNameWithMedAppDto
                {
                    // Medical appointment
                    MedicalAppointmentId = result.MedicalAppointment.Id,
                    MedicalAppointmentDateTime = result.MedicalAppointment.DateTime,
                    // Doctor
                    DoctorId = result.Doctor.Id,
                    DoctorName = result.Doctor.Name,
                    DoctorSurname = result.Doctor.Surname,
                    // Laboratory appointment
                    LaboratoryAppointmentId = result.LaboratoryAppointment.Id,
                    LaboratoryWorkerId = result.LaboratoryAppointment.LaboratoryWorkerId,
                    SupervisorId = result.LaboratoryAppointment.SupervisorId,
                    State = result.LaboratoryAppointment.State,
                    DateTime = result.LaboratoryAppointment.DateTime,
                    CancelComment = result.LaboratoryAppointment.CancelComment,
                    // Patient
                    PatientId = result.Patient.Id,
                    PatientName = result.Patient.Name,
                    PatientSurname = result.Patient.Surname,
                    PatientPesel = result.Patient.Pesel,
                    // Laboratory tests
                    laboratoryTests = result.Tests
                };

                scope.Complete();
                return mappedResult;
            }
            catch (Exception)
            {
                scope.Dispose();
                throw;
            }
        }    

        public async Task<LaboratoryAppointment> CreateLaboratoryAppointment(LaboratoryAppointment laboratoryAppointment)
        {
            await _context.AddAsync(laboratoryAppointment);
            laboratoryAppointment.DateTime = laboratoryAppointment.DateTime.ToUniversalTime();
            await _context.SaveChangesAsync();
            return laboratoryAppointment;
        }

        public async Task<LaboratoryAppointment?> UpdateLaboratoryAppointment(LaboratoryAppointment laboratoryAppointment)
        {
            _context.LaboratoryAppointment.Update(laboratoryAppointment);
            await _context.SaveChangesAsync();
            return laboratoryAppointment;
        }
    }
}
