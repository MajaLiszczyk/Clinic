using ClinicAPI.DB;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace ClinicAPI.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        //REPOZYTORIUM nic nie interesuje. Zwraca dane lub null jeśli ich nie ma. Problemy z bazą, walidacja, to już robota serwisu
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

        

        public async Task<List<Patient>> GetAllPatients()
        {
            //TransactionScope tworzy obszar transakcji. Gdy wywołasz scope.Complete(), wszystkie operacje w transakcji zostaną zatwierdzone.
            //W przeciwnym razie zostaną wycofane.
            using var scope = new TransactionScope(TransactionScopeOption.Required, 
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);  //KOD Z ESERVICE
            List<Patient> patientList = new List<Patient>();
            try
            {
                patientList = await _context.Patient.
                    //Include(m => m.  .ReceivingUser). // Include powodują załadowanie danych powiązanych z innymi tabelami (tzw. eager loading).
                    //Include(m => m.SendingUser).      //Bez tego domyślnie EF Core nie załaduje tych danych.
                    ToListAsync(); //JESLI BRAK WYNIKOW- ZWROCI PUSTA LISTE
                scope.Complete();
            }
            catch (Exception) { }
            return patientList;
            //return await Task.Run(() => arr);
        }

        public async Task<List<Patient>> GetAllAvailablePatients()
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);  //KOD Z ESERVICE
            List<Patient> patientList = new List<Patient>();
            try
            {
                patientList = await _context.Patient.Where(r => r.IsAvailable == true).
                    ToListAsync(); //JESLI BRAK WYNIKOW- ZWROCI PUSTA LISTE
                scope.Complete();
            }
            catch (Exception) { }
            return patientList;
            //return await Task.Run(() => arr);
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
            /*var _patient = _context.Patient.
                FirstOrDefault(p => p.Id == patient.Id);

            if (_patient == null)
            {
                return null;
                //brak pacjenta
            } */
            try
            {
                _context.Patient.Update(patient);
                await _context.SaveChangesAsync();
                /*_patient.Surname = patient.Surname;
                _patient.Name = patient.Name;
                _patient.Pesel = patient.Pesel;            
                _context.SaveChanges(); */
               
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

    }
}
