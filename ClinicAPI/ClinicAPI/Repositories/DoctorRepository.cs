using ClinicAPI.DB;
using ClinicAPI.Models;
using ClinicAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Transactions;

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
