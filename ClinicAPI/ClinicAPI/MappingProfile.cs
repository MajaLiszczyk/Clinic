using AutoMapper;
using ClinicAPI.Dtos;
using ClinicAPI.Models;

namespace ClinicAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, ReturnPatientDto>();
            CreateMap<CreatePatientDto, Patient>();
            CreateMap<UpdatePatientDto, Patient>();

            CreateMap<DiagnosticTest, ReturnDiagnosticTestDto>();
            CreateMap<CreateDiagnosticTestDto, DiagnosticTest>();
            CreateMap<UpdateDiagnosticTestDto, DiagnosticTest>();

            CreateMap<LaboratoryTest, ReturnLaboratoryTestDto>();
            CreateMap<CreateLaboratoryTestDto, LaboratoryTest>();
            CreateMap<UpdateLaboratoryTestDto, LaboratoryTest>();

            CreateMap<Registrant, ReturnRegistrantDto>();
            CreateMap<CreateRegistrantDto, Registrant>();
            CreateMap<UpdateRegistrantDto, Registrant>();

            //CreateMap<Doctor, ReturnDoctorDto>();
            //jesli chce mapowac nazwy , a nie id:
            /*CreateMap<Doctor, ReturnDoctorDto>()
                .ForMember(dest => dest.MedicalSpecialisationsIds,
                    opt => opt.MapFrom(src => src.MedicalSpecialisations.Select(ms => ms.Name)));*/
            CreateMap<Doctor, ReturnDoctorDto>()
                .ForMember(dest => dest.MedicalSpecialisationsIds,
                    opt => opt.MapFrom(src => src.MedicalSpecialisations.Select(ms => ms.Id)));
            CreateMap<CreateDoctorDto, Doctor>();
            CreateMap<UpdateDoctorDto, Doctor>();

            CreateMap<LaboratorySupervisor, ReturnLaboratorySupervisorDto>();
            CreateMap<CreateLaboratorySupervisorDto, LaboratorySupervisor>();
            CreateMap<UpdateLaboratorySupervisorDto, LaboratorySupervisor>();

            CreateMap<LaboratoryWorker, ReturnLaboratoryWorkerDto>();
            CreateMap<CreateLaboratoryWorkerDto, LaboratoryWorker>();
            CreateMap<UpdateLaboratoryWorkerDto, LaboratoryWorker>();

            CreateMap<MedicalAppointment, ReturnMedicalAppointmentDto>();
            CreateMap<CreateMedicalAppointmentDto, MedicalAppointment>();
            CreateMap<UpdateMedicalAppointmentDto, MedicalAppointment>();

            CreateMap<MedicalSpecialisation, ReturnMedicalSpecialisationDto>();
            CreateMap<CreateMedicalSpecialisationDto, MedicalSpecialisation>();
            CreateMap<UpdateMedicalSpecialisationDto, MedicalSpecialisation>();
        }
    }
}
