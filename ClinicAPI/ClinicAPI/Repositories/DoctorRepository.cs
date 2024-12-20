using ClinicAPI.DB;
using ClinicAPI.Models;
using ClinicAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Transactions;
using static ClinicAPI.Models.DoctorWithSpecialisations;

namespace ClinicAPI.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly ApplicationDBContext _context;
        public DoctorRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Doctor?> GetDoctorById(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                       new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                       TransactionScopeAsyncFlowOption.Enabled);
            Doctor? doctor = null;
            try
            {
                doctor = await _context.Doctor.Where(r => r.Id == id)
                            .FirstOrDefaultAsync(); //zwróci null, jesli brak wynikow

                scope.Complete();
            }
            catch (Exception) { }
            return doctor;
        }

        public async Task<List<Doctor>> GetAllDoctors()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            List<Doctor> doctors = new List<Doctor>();
            try
            {
                doctors = await _context.Doctor.
                    ToListAsync();
                scope.Complete();
            }
            catch (Exception) { }
            return doctors;
        }

        public async Task<List<DoctorWithSpecialisations>> GetDoctorsWithSpecialisations()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var doctors = await _context.Doctor
                    .Include(d => d.MedicalSpecialisations)
                    .Select(d => new DoctorWithSpecialisations
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Surname = d.Surname,
                        DoctorNumber = d.DoctorNumber,
                        SpecialisationIds = d.MedicalSpecialisations.Select(ms => ms.Id).ToList()
                    })
                    .ToListAsync();
                scope.Complete();
                //return doctors.Cast<object>().ToList();
                return doctors;



                /*var doctors = await _context.Doctor
                    .Where(ma => ma.DoctorId == doctorId)
                    .ToListAsync();

                scope.Complete();
                return doctors; */

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
                //return new List<object>();
                //return doctors.Cast<object>().ToList();
                return new List<Models.DoctorWithSpecialisations>();
            }
        }


        public async Task<Doctor> CreateDoctor(Doctor doctor)
        {
            await _context.AddAsync(doctor);
            await _context.SaveChangesAsync();
            return doctor;
        }

        public async Task<Doctor?> UpdateDoctor(Doctor doctor)
        {
            Doctor? _doctor = _context.Doctor.
              FirstOrDefault(p => p.Id == doctor.Id);

            if (_doctor == null)
            {
                return null;
                //brak pacjenta
            }
            try
            {
                _doctor.Name = doctor.Name;
                _doctor.Surname = doctor.Surname;

                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                //wyjatek
            }
            return _doctor;
        }

        public async Task<bool> DeleteDoctor(int id)
        {
            var _doctor = await _context.Doctor.FindAsync(id);
            if (_doctor == null) return false;

            _context.Doctor.Remove(_doctor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
