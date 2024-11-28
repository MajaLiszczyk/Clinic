using ClinicAPI.DB;
using ClinicAPI.Models;
using ClinicAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicAPI.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDBContext _context;
        public PatientRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Patient?> GetPatientByIdAsync(int id)
        {
            Patient? patient = null;
            try
            {
                patient = await _context.Patient.Where(r => r.Id == id)
                            .FirstOrDefaultAsync();

                //scope.Complete();
            }
            catch (Exception) { }
            return await Task.Run(() => patient);
        }

        public async Task CreatePatientAsync(Patient patient)
        {
            await _context.AddAsync(patient);
            await _context.SaveChangesAsync();
        }




    }
}
