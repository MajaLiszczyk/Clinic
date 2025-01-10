using ClinicAPI.Dtos;
using ClinicAPI.Models;

namespace ClinicAPI.Services.Interfaces
{
    public interface IDoctorService
    {
        public Task<ReturnDoctorDto?> GetDoctor(int id);
        public Task<List<ReturnDoctorDto>> GetAllDoctors();
        public Task<List<ReturnDoctorDto>> GetAllAvailableDoctors();
        public Task<List<DoctorWithSpecialisations>> GetDoctorsWithSpecialisations();
        public Task<(bool Confirmed, string Response, ReturnDoctorDto? doctor)> CreateDoctor(CreateDoctorDto request, ICollection<MedicalSpecialisation> medicalSpecialisations);
        public Task<(bool Confirmed, string Response, ReturnDoctorDto? doctor)> CreateDoctorWithSpecialisations(CreateDoctorDto request);
        public Task<(bool Confirmed, string Response, ReturnDoctorDto? doctor)> RegisterDoctor(CreateRegisterDoctorDto request);
        public Task<(bool Confirmed, string Response)> UpdateDoctor(UpdateDoctorDto request, ICollection<MedicalSpecialisation> medicalSpecialisations);
        public Task<(bool Confirmed, string Response)> UpdateDoctorWithSpecialisations(UpdateDoctorDto request);
        public Task<(bool Confirmed, string Response)> TransferToArchive(int id);
        public Task<(bool Confirmed, string Response)> DeleteDoctor(int id);
    }
}
