using AutoMapper;
using ProfilesApi.Application.Dto.Administrators;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Mappings;

public class AdministratorMappingProfile : Profile
{
    public AdministratorMappingProfile()
    {
        CreateMap<Administrator, AdministratorDto>()
            .IncludeMembers(src => src.Account);
        CreateMap<CreateAdministratorDto, Administrator>();
        CreateMap<EditAdministratorProfileDto, Administrator>()
            .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src));
    }
}