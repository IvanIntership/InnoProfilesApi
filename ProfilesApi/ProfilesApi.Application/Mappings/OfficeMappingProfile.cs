using AutoMapper;
using ProfilesApi.Application.Dto.Doctors;
using ProfilesApi.Application.Dto.Offices;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Mappings;

public class OfficeMappingProfile : Profile
{
    public OfficeMappingProfile()
    {
        CreateMap<Office, OfficeDto>();
        CreateMap<CreateOfficeDto, Office>();
        CreateMap<EditOfficeInformationDto, Office>();
    }
}