using ClinicAPI.Dtos;
using ClinicAPI.Models;

namespace ClinicAPI.Services.Interfaces
{
    public interface IDoctorService
    {
        public Task<ReturnDoctorDto?> GetDoctor(int id);
        public Task<List<ReturnDoctorDto>> GetAllDoctors();
        public Task<List<DoctorWithSpecialisations>> GetDoctorsWithSpecialisations();
        public Task<(bool Confirmed, string Response, ReturnDoctorDto? doctor)> CreateDoctor(CreateDoctorDto request, ICollection<MedicalSpecialisation> medicalSpecialisations);
        public Task<(bool Confirmed, string Response)> UpdateDoctor(UpdateDoctorDto request);
        public Task<(bool Confirmed, string Response)> DeleteDoctor(int id);
    }
}
