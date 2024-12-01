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

        public async Task<MedicalAppointment> CreateMedicalAppointment(MedicalAppointment medicalAppointment)
        {
            await _context.AddAsync(medicalAppointment);
            await _context.SaveChangesAsync();
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
                _medicalAppointment.dateTime = medicalAppointment.dateTime;
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
