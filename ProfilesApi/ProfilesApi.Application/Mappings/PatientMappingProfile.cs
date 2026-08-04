using AutoMapper;
using ProfilesApi.Application.Dto.Patients;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Mappings;

public sealed class PatientMappingProfile : Profile
{
    public PatientMappingProfile()
    {
        CreateMap<Patient, PatientDto>()
            .IncludeMembers(src => src.Account);
        CreateMap<EditPatientProfileDto, Patient>()
            .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src));
        CreateMap<RegisterPatientDto, Patient>();
    }
}