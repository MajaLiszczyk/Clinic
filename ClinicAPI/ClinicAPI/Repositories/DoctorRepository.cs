using ClinicAPI.DB;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories.Interfaces;
using MediatR;
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
            Doctor? doctor = null;
            doctor = await _context.Doctor.Where(r => r.Id == id)
                            .FirstOrDefaultAsync(); 
            return doctor;
        }

        public async Task<bool> GetDoctorWithTheSameNumber(string number)
        {
            if (_context.Doctor.Any(p => p.DoctorNumber == number))
            {
                return true;
            }
            return false;
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

        public async Task<List<Doctor>> GetAllAvailableDoctors()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                   TransactionScopeAsyncFlowOption.Enabled);
            List<Doctor> doctors = new List<Doctor>();
            try
            {
                doctors = await _context.Doctor.Where(r => r.IsAvailable == true )
                    .ToListAsync();
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
                        SpecialisationIds = d.MedicalSpecialisations.Select(ms => ms.Id).ToList(),
                        IsAvailable = d.IsAvailable
                    })
                    .ToListAsync();
                scope.Complete();
                return doctors;
            }
            catch (Exception ex)
            {
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
            _context.Doctor.Update(doctor);
            await _context.SaveChangesAsync();
            return doctor;
        }

        public async Task<bool> DeleteDoctor(int id)
        {
            var _doctor = await _context.Doctor.FindAsync(id);
            if (_doctor == null) return false;
            _doctor.MedicalSpecialisations.Clear();
            _context.Doctor.Remove(_doctor); 
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CanArchiveDoctor(int doctorId)
        {
            // Sprawdzenie, czy lekarz ma jakąkolwiek "otwartą" wizytę
            return !await _context.MedicalAppointment.AnyAsync(ma =>
                ma.DoctorId == doctorId &&
                ma.IsFinished == false &&
                ma.IsCancelled == false);
        }
    }
}
