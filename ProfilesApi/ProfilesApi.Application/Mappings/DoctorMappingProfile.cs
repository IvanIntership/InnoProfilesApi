using AutoMapper;
using ProfilesApi.Application.Dto.Doctors;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Mappings;

public class DoctorMappingProfile : Profile
{
    public DoctorMappingProfile()
    {
        CreateMap<Doctor, DoctorDto>()
            .IncludeMembers(src => src.Account);
        CreateMap<CreateDoctorDto, Doctor>();
        CreateMap<EditDoctorProfileDto, Doctor>()
            .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src));
    }
}