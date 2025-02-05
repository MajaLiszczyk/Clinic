using ClinicAPI.DB;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace ClinicAPI.Repositories
{
    public class MedicalAppointmentRepository : IMedicalAppointmentRepository
    {
        private readonly ApplicationDBContext _context;
        public MedicalAppointmentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async  Task<MedicalAppointment?> GetMedicalAppointmentById(int id)
        {
           // Doctor? doctor = null;
            var medicalAppointment = await _context.MedicalAppointment.Where(r => r.Id == id)
                            .FirstOrDefaultAsync();
            return medicalAppointment;
        }


        public async Task<ReturnMedicalAppointmentPatientDto?> GetMedicalAppointmentByIdWithPatient(int id)
        {
            var appointment = await (from ma in _context.MedicalAppointment
                                      join p in _context.Patient on ma.PatientId equals p.Id into patientJoin
                                     from p in patientJoin.DefaultIfEmpty() // LEFT JOIN
                                     where ma.Id == id
                                      select new ReturnMedicalAppointmentPatientDto
                                      {
                                          Id = ma.Id,
                                          DateTime = ma.DateTime,
                                          PatientId = ma.PatientId,
                                          PatientName = p != null ? p.Name : "No patient", // Jeśli pacjent nie istnieje, zwracamy domyślną wartość
                                          PatientSurname = p != null ? p.Surname : "No patient",
                                          PatientPesel = p != null ? p.Pesel : "No patient",
                                          DoctorId = ma.DoctorId,
                                          Interview = ma.Interview,
                                          Diagnosis = ma.Diagnosis,
                                          IsFinished = ma.IsFinished,
                                          IsCancelled = ma.IsCancelled,
                                          CancellingComment = ma.CancellingComment
                                      }).FirstOrDefaultAsync();

            return appointment;

            //ReturnMedicalAppointmentPatientDto
            //return await _context.MedicalAppointment.Where(r => r.Id == id).FirstOrDefaultAsync();           
        }

        public async Task<List<ReturnMedicalAppointmentPatientDoctorDto>> GetAllMedicalAppointmentsPatientsDoctors()
        {
            List<MedicalAppointment> medicalAppointments = new List<MedicalAppointment>();

                var appointments = await (from ma in _context.MedicalAppointment
                                          join p in _context.Patient on ma.PatientId equals p.Id into patientJoin
                                          from p in patientJoin.DefaultIfEmpty() //left join zeby zwracalo tez wizyty bez acjentow (patientId ==0)
                                          join d in _context.Doctor on ma.DoctorId equals d.Id
                                          select new ReturnMedicalAppointmentPatientDoctorDto
                                          {
                                              Id = ma.Id,
                                              DateTime = ma.DateTime,
                                              PatientId = ma.PatientId,
                                              PatientName = p != null ? p.Name : "No patient", // Jeśli pacjent nie istnieje, zwracamy domyślną wartość
                                              PatientSurname = p != null ? p.Surname : "No patient",
                                              PatientPesel = p != null ? p.Pesel : "No patient",
                                              DoctorId = ma.DoctorId,
                                              DoctorName = d.Name,
                                              DoctorSurname = d.Surname,
                                              Interview = ma.Interview,
                                              Diagnosis = ma.Diagnosis,
                                              IsFinished = ma.IsFinished,
                                              IsCancelled = ma.IsCancelled,
                                              CancellingComment = ma.CancellingComment
                                          }).ToListAsync();

                return appointments;

        }
        

        public async Task<List<MedicalAppointment>> GetAllMedicalAppointments()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            List<MedicalAppointment> medicalAppointments = new List<MedicalAppointment>();
            try
            {
                medicalAppointments = await _context.MedicalAppointment.
                    ToListAsync();
                scope.Complete();
            }
            catch (Exception) { }
            return medicalAppointments;
        }


        public async Task<List<ReturnMedicalAppointmentDoctorDto>> GetMedicalAppointmentsBySpecialisation(int specialisationId)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var medicalAppointments = await _context.MedicalAppointment
                .Where(ma => _context.Doctor
                    .Where(d => d.MedicalSpecialisations.Any(ms => ms.Id == specialisationId))
                    .Select(d => d.Id)
                    .Contains(ma.DoctorId))
                .Join(_context.Doctor,
                 ma => ma.DoctorId, // Klucz z MedicalAppointment
                 d => d.Id,         // Klucz z Doctor
                (ma, d) => new ReturnMedicalAppointmentDoctorDto // Tworzymy nowy obiekt z danymi
                {
                    Id = ma.Id,
                    dateTime =  ma.DateTime,
                    PatientId = (int)ma.PatientId,
                    DoctorId = ma.DoctorId,
                    DoctorName = d.Name,
                    DoctorSurname = d.Surname,
                    Interview = ma.Interview,
                    Diagnosis = ma.Diagnosis,
                    IsFinished = ma.IsFinished,
                    IsCancelled = ma.IsCancelled,
                    CancellingComment = ma.CancellingComment
                })
                .ToListAsync();

                return medicalAppointments;


                
            }
            catch (Exception ex)
            {
                // Obsłuż wyjątek (logowanie, etc.)
                return new List<ReturnMedicalAppointmentDoctorDto>();
            }
        }

        public async Task<List<ReturnMedicalAppointmentPatientDoctorDto>> GetMedicalAppointmentsByDoctorId(int doctorId)
        {
            var appointments = await (from ma in _context.MedicalAppointment
                                      join p in _context.Patient on ma.PatientId equals p.Id into patientJoin
                                      from p in patientJoin.DefaultIfEmpty() // LEFT JOIN
                                      join d in _context.Doctor on ma.DoctorId equals d.Id
                                      where ma.DoctorId == doctorId
                                      select new ReturnMedicalAppointmentPatientDoctorDto
                                      {
                                          Id = ma.Id,
                                          DateTime = ma.DateTime,
                                          PatientId = ma.PatientId,
                                          PatientName = p != null ? p.Name : "Brak pacjenta", // Jeśli pacjent nie istnieje, zwracamy domyślną wartość
                                          PatientSurname = p != null ? p.Surname : "Brak pacjenta",
                                          PatientPesel = p != null ? p.Pesel : "Brak danych",
                                          DoctorId = ma.DoctorId,
                                          DoctorName = d.Name,
                                          DoctorSurname = d.Surname,
                                          Interview = ma.Interview,
                                          Diagnosis = ma.Diagnosis,
                                          IsFinished = ma.IsFinished,
                                          IsCancelled = ma.IsCancelled,
                                          CancellingComment = ma.CancellingComment
                                      }).ToListAsync();

            return appointments;



            /*var appointments = await _context.MedicalAppointment
                    .Where(ma => ma.DoctorId == doctorId)
                    .ToListAsync();

                return appointments;*/

                /* Jesli bede chciala wiecej danych o pacjencie:
                   var query = from ma in _context.MedicalAppointments
                    join p in _context.Patients on ma.PatientId equals p.Id
                    where ma.PatientId == patientId
                    select new { Appointment = ma, Patient = p };
                    var result = await query.ToListAsync();
                    scope.Complete();
                 */


        }

        public async Task<List<ReturnMedicalAppointmentPatientDoctorDto>> GetFutureMedicalAppointmentsByPatientUserId(string userId)
        {
            var appointments = await (from ma in _context.MedicalAppointment
                                      join p in _context.Patient on ma.PatientId equals p.Id
                                      join d in _context.Doctor on ma.DoctorId equals d.Id
                                      where p.UserId == userId
                                      where ma.IsFinished != true
                                      where ma.IsCancelled != true
                                      select new ReturnMedicalAppointmentPatientDoctorDto
                                      {
                                          Id = ma.Id,
                                          DateTime = ma.DateTime,
                                          PatientId = ma.PatientId,
                                          PatientName = p.Name,
                                          PatientSurname = p.Surname,
                                          PatientPesel = p.Pesel,
                                          DoctorId = ma.DoctorId,
                                          DoctorName = d.Name,
                                          DoctorSurname = d.Surname,
                                          Interview = ma.Interview,
                                          Diagnosis = ma.Diagnosis,
                                          IsFinished = ma.IsFinished,
                                          IsCancelled = ma.IsCancelled,
                                          CancellingComment = ma.CancellingComment
                                      }).ToListAsync();

            return appointments;
        }

        public async Task<List<ReturnMedicalAppointmentPatientDoctorDto>> GetPastMedicalAppointmentsByPatientUserId(string userId)
        {
            var appointments = await (from ma in _context.MedicalAppointment
                                      join p in _context.Patient on ma.PatientId equals p.Id
                                      join d in _context.Doctor on ma.DoctorId equals d.Id
                                      where p.UserId == userId
                                      where ma.IsFinished == true
                                      select new ReturnMedicalAppointmentPatientDoctorDto
                                      {
                                          Id = ma.Id,
                                          DateTime = ma.DateTime,
                                          PatientId = ma.PatientId,
                                          PatientName = p.Name,
                                          PatientSurname = p.Surname,
                                          PatientPesel = p.Pesel,
                                          DoctorId = ma.DoctorId,
                                          DoctorName = d.Name,
                                          DoctorSurname = d.Surname,
                                          Interview = ma.Interview,
                                          Diagnosis = ma.Diagnosis,
                                          IsFinished = ma.IsFinished,
                                          IsCancelled = ma.IsCancelled,
                                          CancellingComment = ma.CancellingComment
                                      }).ToListAsync();

            return appointments;
        }

        public async Task<List<ReturnMedicalAppointmentPatientDoctorDto>> GetFutureMedicalAppointmentsByPatientId(int patientId)
        {
            var appointments = await (from ma in _context.MedicalAppointment
                                      join p in _context.Patient on ma.PatientId equals p.Id
                                      join d in _context.Doctor on ma.DoctorId equals d.Id
                                      where ma.PatientId == patientId
                                      where ma.IsFinished != true
                                      where ma.IsCancelled != true
                                      select new ReturnMedicalAppointmentPatientDoctorDto
                                      {
                                          Id = ma.Id,
                                          DateTime = ma.DateTime,
                                          PatientId = ma.PatientId,
                                          PatientName = p.Name,
                                          PatientSurname = p.Surname,
                                          PatientPesel = p.Pesel,
                                          DoctorId = ma.DoctorId,
                                          DoctorName = d.Name,
                                          DoctorSurname = d.Surname,
                                          Interview = ma.Interview,
                                          Diagnosis = ma.Diagnosis,
                                          IsFinished = ma.IsFinished,
                                          IsCancelled = ma.IsCancelled,
                                          CancellingComment = ma.CancellingComment
                                      }).ToListAsync();

            return appointments;
        }

        public async Task<List<ReturnMedicalAppointmentPatientDoctorDto>> GetPastMedicalAppointmentsByPatientId(int patientId)
        {
            var appointments = await (from ma in _context.MedicalAppointment
                                      join p in _context.Patient on ma.PatientId equals p.Id
                                      join d in _context.Doctor on ma.DoctorId equals d.Id
                                      where ma.PatientId == patientId
                                      where ma.IsFinished == true
                                      select new ReturnMedicalAppointmentPatientDoctorDto
                                      {
                                          Id = ma.Id,
                                          DateTime = ma.DateTime,
                                          PatientId = ma.PatientId,
                                          PatientName = p.Name,
                                          PatientSurname = p.Surname,
                                          PatientPesel = p.Pesel,
                                          DoctorId = ma.DoctorId,
                                          DoctorName = d.Name,
                                          DoctorSurname = d.Surname,
                                          Interview = ma.Interview,
                                          Diagnosis = ma.Diagnosis,
                                          IsFinished = ma.IsFinished,
                                          IsCancelled = ma.IsCancelled,
                                          CancellingComment = ma.CancellingComment
                                      }).ToListAsync();

            return appointments;
        }




        public async Task<List<ReturnMedicalAppointmentPatientDoctorDto>> GetMedicalAppointmentsByPatientId(int patientId)
        {
            var appointments = await (from ma in _context.MedicalAppointment
                                      join p in _context.Patient on ma.PatientId equals p.Id
                                      join d in _context.Doctor on ma.DoctorId equals d.Id
                                      where ma.PatientId == patientId
                                      select new ReturnMedicalAppointmentPatientDoctorDto
                                      {
                                          Id = ma.Id,
                                          DateTime = ma.DateTime,
                                          PatientId = ma.PatientId,
                                          PatientName = p.Name,
                                          PatientSurname = p.Surname,
                                          PatientPesel = p.Pesel,
                                          DoctorId = ma.DoctorId,
                                          DoctorName = d.Name,
                                          DoctorSurname = d.Surname,
                                          Interview = ma.Interview,
                                          Diagnosis = ma.Diagnosis,
                                          IsFinished = ma.IsFinished,
                                          IsCancelled = ma.IsCancelled,
                                          CancellingComment = ma.CancellingComment
                                      }).ToListAsync();

            return appointments;


            /*var appointments = await _context.MedicalAppointment
                    .Where(ma => ma.PatientId == patientId)
                    .ToListAsync();
                return appointments;*/
        }

        public async Task<bool> HasPatientMedicalAppointments(int patientId)
        {
            return await _context.MedicalAppointment.AnyAsync(ma => ma.PatientId == patientId);
        }

        public async Task<bool> HasDoctorMedicalAppointments(int doctorId)
        {
           return  await _context.MedicalAppointment.AnyAsync(a => a.DoctorId == doctorId);
        }




        public async Task<MedicalAppointment> CreateMedicalAppointment(MedicalAppointment medicalAppointment)
        {
                await _context.AddAsync(medicalAppointment);
                medicalAppointment.DateTime = medicalAppointment.DateTime.ToUniversalTime();
                await _context.SaveChangesAsync();         

            return medicalAppointment;

        }

        public async Task<MedicalAppointment?> UpdateMedicalAppointment(MedicalAppointment medicalAppointment)
        {
            _context.MedicalAppointment.Update(medicalAppointment);
            await _context.SaveChangesAsync();
            return medicalAppointment;
        }

        public async Task<bool> DeleteMedicalAppointment(int id)
        {
            var _medicalAppointment = await _context.MedicalAppointment.FindAsync(id);
            if (_medicalAppointment == null) return false;

            _context.MedicalAppointment.Remove(_medicalAppointment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
