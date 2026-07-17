using AutoMapper;
using ProfilesApi.Application.Dto.Photos;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Mappings;

public class PhotoMappingProfile : Profile
{
    public PhotoMappingProfile()
    {
        CreateMap<Photo, PhotoDto>();
    }
}