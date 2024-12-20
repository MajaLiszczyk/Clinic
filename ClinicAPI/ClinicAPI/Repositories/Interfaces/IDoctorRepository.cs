using ClinicAPI.Models;

namespace ClinicAPI.Repositories.Interfaces
{
    public interface IDoctorRepository
    {
        public Task<Doctor?> GetDoctorById(int id);
        public Task<List<Doctor>> GetAllDoctors();
        public Task<List<DoctorWithSpecialisations>> GetDoctorsWithSpecialisations();
        public Task<Doctor> CreateDoctor(Doctor doctor);
        public Task<Doctor?> UpdateDoctor(Doctor doctor);
        public Task<bool> DeleteDoctor(int id);

    }
}
