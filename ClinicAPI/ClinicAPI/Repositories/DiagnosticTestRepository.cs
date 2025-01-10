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
        private readonly ApplicationDBContext _context;
        public DiagnosticTestRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<DiagnosticTest?> GetDiagnosticTestById(int id)
        {

            DiagnosticTest? diagnosticTest = null;
            try
            {
                diagnosticTest = await _context.DiagnosticTest.Where(r => r.Id == id)
                            .FirstOrDefaultAsync(); //zwróci null, jesli brak wynikow

                
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
                                                    TransactionScopeAsyncFlowOption.Enabled);  
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
                                                    TransactionScopeAsyncFlowOption.Enabled);  
            try
            {
                var diagnosticTests = await _context.DiagnosticTest
                    .Where(dt => dt.MedicalAppointmentId == medicalAppointmentId)
                    .Join(
                        _context.DiagnosticTestType, 
                        dt => dt.DiagnosticTestTypeId, 
                        dtt => dtt.Id,
                        (dt, dtt) => new ReturnDiagnosticTestDto 
                        {
                            Id = dt.Id,
                            MedicalAppointmentId = dt.MedicalAppointmentId,
                            DiagnosticTestTypeId = dt.DiagnosticTestTypeId,
                            DiagnosticTestTypeName = dtt.Name,
                            Description = dt.Description
                        }
                    )
                    .ToListAsync();

                    scope.Complete();
                    return diagnosticTests;

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

            try
            {
                _diagnosticTest.Description = diagnosticTest.Description;
                _diagnosticTest.MedicalAppointmentId = diagnosticTest.MedicalAppointmentId;

                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                //wyjatek
            }
            return _diagnosticTest;
        }
     
    }
}
