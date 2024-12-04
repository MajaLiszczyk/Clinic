using ClinicAPI.Dtos;
using ClinicAPI.Models;

namespace ClinicAPI.Services.Interfaces
{
    public interface IMedicalSpecialisationService
    {
        public Task<ReturnMedicalSpecialisationDto?> GetMedicalSpecialisation(int id);
        public Task<MedicalSpecialisation?> GetRawSpecialisation(int id);
        public Task<List<ReturnMedicalSpecialisationDto>> GetAllMedicalSpecialisations();
        public Task<(bool Confirmed, string Response, ReturnMedicalSpecialisationDto? medSpecialisation)> CreateMedicalSpecialisation(CreateMedicalSpecialisationDto request);
        public Task<(bool Confirmed, string Response)> UpdateMedicalSpecialisation(UpdateMedicalSpecialisationDto request);
        public Task<(bool Confirmed, string Response)> DeleteMedicalSpecialisation(int id); 
    }
}
