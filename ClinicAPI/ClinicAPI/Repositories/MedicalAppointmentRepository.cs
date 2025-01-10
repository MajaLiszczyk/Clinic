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
            return await _context.MedicalAppointment.Where(r => r.Id == id).FirstOrDefaultAsync();           
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
                // Obsłuż wyjątek 
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
                var appointments = await _context.MedicalAppointment
                    .Where(ma => ma.PatientId == patientId)
                    .ToListAsync();
                scope.Complete();
                return appointments;


            }
            catch (Exception ex)
            {
                return new List<MedicalAppointment>();
            }
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
