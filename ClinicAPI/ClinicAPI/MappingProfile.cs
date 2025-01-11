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

            CreateMap<LaboratoryAppointment, ReturnLaboratoryAppointmentDto>();
            CreateMap<CreateLaboratoryAppointmentDto, LaboratoryAppointment>();
            CreateMap<UpdateLaboratoryAppointmentDto, LaboratoryAppointment>();

            CreateMap<DateDto, DateTime>().ConstructUsing((src, _) => new DateTime(src.Year, src.Month, src.Day));
                /*.ForMember(d => d.Year, d => d.MapFrom(t => t.Year))
                .ForMember(d => d.Month, d => d.MapFrom(t => t.Month))
                .ForMember(d => d.Day, d => d.MapFrom(t => t.Day));*/

            CreateMap<DateTime, DateDto>()
                .ForMember(d => d.Year, d => d.MapFrom(t => t.Year))
                .ForMember(d => d.Month, d => d.MapFrom(t => t.Month))
                .ForMember(d => d.Day, d => d.MapFrom(t => t.Day));


            CreateMap<TimeDto, DateTime>().ConstructUsing((src, _) => new DateTime(0, 0, 0, src.Hour, src.Minute, 0));
            /*.ForMember(d => d.Hour, d => d.MapFrom(t => t.Hour))
            .ForMember(d => d.Minute, d => d.MapFrom(t => t.Minute)); */

            CreateMap<DateTime, TimeDto>()
                .ForMember(d => d.Hour, d => d.MapFrom(t => t.Hour))
                .ForMember(d => d.Minute, d => d.MapFrom(t => t.Minute));
        }
    }
}
