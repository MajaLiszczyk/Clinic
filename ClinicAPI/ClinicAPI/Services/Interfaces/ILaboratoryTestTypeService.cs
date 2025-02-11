﻿using ClinicAPI.Models;

namespace ClinicAPI.Services.Interfaces
{
    public interface ILaboratoryTestTypeService
    {
        public Task<LaboratoryTestType?> GetLaboratoryTestType(int id);
        public Task<List<LaboratoryTestType>> GetAllLaboratoryTestTypes();
        public Task<(bool Confirmed, string Response, LaboratoryTestType? patient)> CreateLaboratoryTestType(CreateLaboratoryTestTypeDto testType);
        public Task<(bool Confirmed, string Response)> UpdateLaboratoryTestType(UpdateLaboratoryTestTypeDto testType);
        public Task<(bool Confirmed, string Response)> TransferToArchive(int id);
        public Task<(bool Confirmed, string Response)> DeleteLaboratoryTestType(int id);
    }
}
