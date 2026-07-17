using AutoMapper;
using ProfilesApi.Application.Dto.Specializations;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Mappings;

public class SpecializationMappingProfile : Profile
{
    public SpecializationMappingProfile()
    {
        CreateMap<Specialization, SpecializationDto>();
        CreateMap<CreateSpecializationDto, Specialization>();
        CreateMap<EditSpecializationInformationDto, Specialization>();
    }
}