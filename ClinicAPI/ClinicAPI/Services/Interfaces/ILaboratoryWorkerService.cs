﻿using ClinicAPI.Dtos;

namespace ClinicAPI.Services.Interfaces
{
    public interface ILaboratoryWorkerService
    {
        public Task<ReturnLaboratoryWorkerDto?> GetLaboratoryWorker(int id);
        public Task<List<ReturnLaboratoryWorkerDto>> GetAllLaboratoryWorkers();
        public Task<List<ReturnLaboratoryWorkerDto>> GetAllAvailableLaboratoryWorkers();
        public Task<(bool Confirmed, string Response, ReturnLaboratoryWorkerDto? laboratoryWorker)> CreateLaboratoryWorker(CreateLaboratoryWorkerDto request);
        public Task<(bool Confirmed, string Response, ReturnLaboratoryWorkerDto? laboratoryWorker)> RegisterLaboratoryWorker(CreateRegisterLaboratoryWorkerDto request);
        public Task<(bool Confirmed, string Response)> UpdateLaboratoryWorker(UpdateLaboratoryWorkerDto request);
        public Task<(bool Confirmed, string Response)> TransferToArchive(int id);
        public Task<(bool Confirmed, string Response)> DeleteLaboratoryWorker(int id);
    }
}
