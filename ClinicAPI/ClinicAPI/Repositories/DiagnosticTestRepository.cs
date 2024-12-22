using ClinicAPI.DB;
using ClinicAPI.Dtos;
using ClinicAPI.Models;
using ClinicAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace ClinicAPI.Repositories
{
    public class DiagnosticTestRepository : IDiagnosticTestRepository
    {
        //diagnosticTest
        //REPOZYTORIUM nic nie interesuje. Zwraca dane lub null jeśli ich nie ma. Problemy z bazą, walidacja, to już robota serwisu
        private readonly ApplicationDBContext _context;
        public DiagnosticTestRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<DiagnosticTest?> GetDiagnosticTestById(int id)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                       new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                       TransactionScopeAsyncFlowOption.Enabled);
            DiagnosticTest? diagnosticTest = null;
            try
            {
                diagnosticTest = await _context.DiagnosticTest.Where(r => r.Id == id)
                            .FirstOrDefaultAsync(); //zwróci null, jesli brak wynikow

                scope.Complete();
            }
            catch (Exception)
            {
                //_logger.LogError(ex, "Error occurred while fetching patient with ID {Id}", id);
            }
            //return await Task.Run(() => patient);
            return diagnosticTest;
        }
        public async Task<List<DiagnosticTest>> GetAllDiagnosticTests()
        {
            //TransactionScope tworzy obszar transakcji. Gdy wywołasz scope.Complete(), wszystkie operacje w transakcji zostaną zatwierdzone.
            //W przeciwnym razie zostaną wycofane.
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);  //KOD Z ESERVICE
            List<DiagnosticTest> diagnosticTests = new List<DiagnosticTest>();
            try
            {
                diagnosticTests = await _context.DiagnosticTest.
                    //Include(m => m.  .ReceivingUser). // Include powodują załadowanie danych powiązanych z innymi tabelami (tzw. eager loading).
                    //Include(m => m.SendingUser).      //Bez tego domyślnie EF Core nie załaduje tych danych.
                    ToListAsync(); //JESLI BRAK WYNIKOW- ZWROCI PUSTA LISTE
                scope.Complete();
            }
            catch (Exception) { }
            return diagnosticTests;
            //return await Task.Run(() => arr);
        }

        public async Task<List<ReturnDiagnosticTestDto>> GetByMedicalAppointmentId(int medicalAppointmentId)
        {
            using var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                                    TransactionScopeAsyncFlowOption.Enabled);  //KOD Z ESERVICE
            //List<DiagnosticTest> diagnosticTests = new List<DiagnosticTest>();
            try
            {
                var diagnosticTests = await _context.DiagnosticTest
                    .Where(dt => dt.MedicalAppoitmentId == medicalAppointmentId)
                    .Join(
                        _context.DiagnosticTestType, // Druga tabela do połączenia
                        dt => dt.DiagnosticTestTypeId, // Klucz z DiagnosticTest
                        dtt => dtt.Id, // Klucz z DiagnosticTestType
                        (dt, dtt) => new ReturnDiagnosticTestDto // Projekcja wyniku
                        {
                            Id = dt.Id,
                            MedicalAppoitmentId = dt.MedicalAppoitmentId,
                            DiagnosticTestTypeId = dt.DiagnosticTestTypeId,
                            DiagnosticTestTypeName = dtt.Name,
                            Description = dt.Description
                        }
                    )
                    .ToListAsync();

                    scope.Complete();
                    return diagnosticTests;

                /*var diagnosticTests = await _context.DiagnosticTest
                    .Where(ma => ma.MedicalAppoitmentId == medicalAppointmentId)
                    .ToListAsync();
                scope.Complete();
                return diagnosticTests; */

            }
            catch (Exception) {
                return new List<ReturnDiagnosticTestDto>();
            }
        }


        
        public async Task<DiagnosticTest> CreateDiagnosticTest(DiagnosticTest diagnosticTest)
        {
            await _context.AddAsync(diagnosticTest);
            await _context.SaveChangesAsync();
            return diagnosticTest;
        }
        public async Task<DiagnosticTest?> UpdateDiagnosticTest(DiagnosticTest diagnosticTest)
        {
            var _diagnosticTest = _context.DiagnosticTest.
               FirstOrDefault(p => p.Id == diagnosticTest.Id);

            if (_diagnosticTest == null)
            {
                return null;
                //brak testu
            }
            try
            {
                //_diagnosticTest.date = diagnosticTest.date;
                _diagnosticTest.Description = diagnosticTest.Description;
                //_diagnosticTest.DoctorId = diagnosticTest.DoctorId;
                _diagnosticTest.MedicalAppoitmentId = diagnosticTest.MedicalAppoitmentId;

                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                //wyjatek
            }
            return _diagnosticTest;
        }

        public async Task<bool> DeleteDiagnosticTest(int id)
        {
            var _diagnosticTest = await _context.DiagnosticTest.FindAsync(id);
            if (_diagnosticTest == null) return false;

            _context.DiagnosticTest.Remove(_diagnosticTest);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
