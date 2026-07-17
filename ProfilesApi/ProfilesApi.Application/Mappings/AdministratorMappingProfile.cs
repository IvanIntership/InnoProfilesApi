using AutoMapper;
using ProfilesApi.Application.Dto.Administrators;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Mappings;

public class AdministratorMappingProfile : Profile
{
    public AdministratorMappingProfile()
    {
        CreateMap<Administrator, AdministratorDto>();
        CreateMap<CreateAdministratorDto, Administrator>();
        CreateMap<EditAdministratorProfileDto, Administrator>();
    }
}