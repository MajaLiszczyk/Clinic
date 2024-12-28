﻿using ClinicAPI.Dtos;
using ClinicAPI.Models;

namespace ClinicAPI.Repositories.Interfaces
{
    public interface IMedicalSpecialisationRepository
    {

        public Task<MedicalSpecialisation?> GetMedicalSpecialisationById(int id);
        public Task<List<MedicalSpecialisation>> GetAllMedicalSpecialisations();
        public Task<List<MedicalSpecialisation>> GetAllAvailableMedicalSpecialisations();
        public Task<MedicalSpecialisation> CreateMedicalSpecialisation(MedicalSpecialisation medicalSpecialisation);
        public Task<MedicalSpecialisation?> UpdateMedicalSpecialisation(MedicalSpecialisation medicalSpecialisation);
        public Task<bool> DeleteMedicalSpecialisation(int id);
    }
}
