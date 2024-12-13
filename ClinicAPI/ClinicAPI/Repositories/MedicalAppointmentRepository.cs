using ClinicAPI.DB;
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

        public async Task<MedicalAppointment?> GetMedicalAppointmentById(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                      new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                      TransactionScopeAsyncFlowOption.Enabled);
            MedicalAppointment? medicalAppointment = null;
            try
            {
                medicalAppointment = await _context.MedicalAppointment.Where(r => r.Id == id)
                            .FirstOrDefaultAsync();

                scope.Complete();
            }
            catch (Exception) { }
            return medicalAppointment;
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


        public async Task<List<MedicalAppointment>> GetMedicalAppointmentsBySpecialisation(int specialisationId)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                // Pobierz listę MedicalAppointment, w której doktor ma daną specjalizację
                var medicalAppointments = await _context.MedicalAppointment
                    .Where(ma => _context.Doctor
                        .Where(d => d.MedicalSpecialisations.Any(ms => ms.Id == specialisationId)) // Filtr lekarzy według specjalizacji
                        .Select(d => d.Id) // Pobierz ID lekarzy
                        .Contains(ma.DoctorId)) // Sprawdź, czy DoctorId w MedicalAppointment pasuje
                    .ToListAsync();

                scope.Complete();
                return medicalAppointments;
            }
            catch (Exception ex)
            {
                // Obsłuż wyjątek (logowanie, etc.)
                return new List<MedicalAppointment>();
            }
        }

        public async Task<List<MedicalAppointment>> GetMedicalAppointmentsByDoctorId(int doctorId)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var appointments = await _context.MedicalAppointment
                    .Where(ma => ma.DoctorId == doctorId)
                    .ToListAsync();

                scope.Complete();
                return appointments;

                /* Jesli bede chciala wiecej danych o pacjencie:
                   var query = from ma in _context.MedicalAppointments
                    join p in _context.Patients on ma.PatientId equals p.Id
                    where ma.PatientId == patientId
                    select new { Appointment = ma, Patient = p };
                    var result = await query.ToListAsync();
                    scope.Complete();
                 */

            }
            catch (Exception ex)
            {
                // Obsłuż wyjątek (logowanie, etc.)
                return new List<MedicalAppointment>();
            }
        }
        

        public async Task<List<MedicalAppointment>> GetMedicalAppointmentsByPatientId(int patientId)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {

                // Filtruj wizyty na podstawie patientId
                var appointments = await _context.MedicalAppointment
                    .Where(ma => ma.PatientId == patientId)
                    .ToListAsync();

                scope.Complete();
                return appointments;

                /* Jesli bede chciala wiecej danych o pacjencie:
                   var query = from ma in _context.MedicalAppointments
                    join p in _context.Patients on ma.PatientId equals p.Id
                    where ma.PatientId == patientId
                    select new { Appointment = ma, Patient = p };
                    var result = await query.ToListAsync();
                    scope.Complete();
                 */

            }
            catch (Exception ex)
            {
                // Obsłuż wyjątek (logowanie, etc.)
                return new List<MedicalAppointment>();
            }
        }
        




        public async Task<MedicalAppointment> CreateMedicalAppointment(MedicalAppointment medicalAppointment)
        {
            try
            {
               /* var doctor = await _context.MedicalAppointment
                .Include(d => d.DoctorId)
                .FirstOrDefaultAsync(d => d.Id == medicalAppointment.Id);*/
                await _context.AddAsync(medicalAppointment);
                medicalAppointment.DateTime = medicalAppointment.DateTime.ToUniversalTime();
                await _context.SaveChangesAsync();
                

            }

            catch (DbUpdateException ex)
{
                Console.WriteLine(ex.InnerException?.Message);
            }
            return medicalAppointment;

        }

        public async Task<MedicalAppointment?> UpdateMedicalAppointment(MedicalAppointment medicalAppointment)
        {
            var _medicalAppointment = _context.MedicalAppointment.
               FirstOrDefault(p => p.Id == medicalAppointment.Id);

            if (_medicalAppointment == null)
            {
                return null;
            }
            try
            {
                _medicalAppointment.DateTime = medicalAppointment.DateTime;
                _medicalAppointment.PatientId = medicalAppointment.PatientId;
                _medicalAppointment.Interview = medicalAppointment.Interview;
                _medicalAppointment.Diagnosis = medicalAppointment.Diagnosis;
                _medicalAppointment.DiseaseUnit = medicalAppointment.DiseaseUnit;
                _medicalAppointment.DoctorId = medicalAppointment.DoctorId;

                _context.SaveChanges();

            }
            catch (Exception ex) { }
            return _medicalAppointment;
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
