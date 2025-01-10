using ClinicAPI.DB;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClinicAPI.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDBContext _context;
        public PatientRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Patient?> GetPatientById(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                        new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                        TransactionScopeAsyncFlowOption.Enabled);
            Patient? patient = null;
            try
            {
                patient = await _context.Patient.Where(r => r.Id == id)
                            .FirstOrDefaultAsync(); //zwróci null, jesli brak wynikow

                scope.Complete();
            }
            catch (Exception) {
                //_logger.LogError(ex, "Error occurred while fetching patient with ID {Id}", id);
            }
            //return await Task.Run(() => patient);
            return patient;
        }

        public async Task<Patient?> GetPatientByUserId(string id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                        new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                        TransactionScopeAsyncFlowOption.Enabled);
            Patient? patient = null;
            try
            {
                patient = await _context.Patient.Where(r => r.UserId == id)
                            .FirstOrDefaultAsync(); //zwróci null, jesli brak wynikow
                scope.Complete();
            }
            catch (Exception)
            {
                //_logger.LogError(ex, "Error occurred while fetching patient with ID {Id}", id);
            }
            //return await Task.Run(() => patient);
            return patient;
        }

        public async Task<bool> GetPatientWithTheSamePesel(string pesel)
        {
            if (_context.Patient.Any(p => p.Pesel == pesel))
            {
                return true;
            }
            return false;
        }



        public async Task<List<Patient>> GetAllPatients()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);
            List<Patient> patientList = new List<Patient>();
            try
            {
                patientList = await _context.Patient.

                    ToListAsync(); //JESLI BRAK WYNIKOW- ZWROCI PUSTA LISTE
                scope.Complete();
            }
            catch (Exception) { }
            return patientList;
        }

        public async Task<List<Patient>> GetAllAvailablePatients()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);  
            List<Patient> patientList = new List<Patient>();
            try
            {
                patientList = await _context.Patient.Where(r => r.IsAvailable == true).
                    ToListAsync(); //JESLI BRAK WYNIKOW- ZWROCI PUSTA LISTE
                scope.Complete();
            }
            catch (Exception) { }
            return patientList;
        }

        

        public async Task<Patient> CreatePatient(Patient patient)
        {
            try
            {
                await _context.AddAsync(patient);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return patient;
        }


        public async Task<Patient?> UpdatePatient(Patient patient)
        {

            try
            {
                _context.Patient.Update(patient);
                await _context.SaveChangesAsync();

               
            }
            catch (Exception ex)
            {
                //OBSŁUŻYC
                throw new Exception("An error occurred while updating the patient.", ex);

            }
            return patient;
        }

        public async Task<bool> DeletePatient(int id)
        {
            var _patient = await _context.Patient.FindAsync(id);
            if (_patient == null) return false;

            _context.Patient.Remove(_patient);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CanArchivePatient(int patientId)
        {
            // Sprawdzenie, czy pacjent ma jakąkolwiek "otwartą" wizytę
            return !await _context.MedicalAppointment.AnyAsync(ma =>
                ma.PatientId == patientId &&
                ma.IsFinished == false &&
                ma.IsCancelled == false);
        }

    }
}
