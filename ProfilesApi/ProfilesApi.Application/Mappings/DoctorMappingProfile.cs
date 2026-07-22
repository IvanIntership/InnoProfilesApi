using AutoMapper;
using ProfilesApi.Application.Dto.Doctors;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Mappings;

public class DoctorMappingProfile : Profile
{
    public DoctorMappingProfile()
    {
        CreateMap<Doctor, DoctorDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.Account.Firstname))
            .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => src.Account.Lastname))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Account.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Account.PhoneNumber))
            .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Account.Birthday))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Account.Role))
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Account.Photo != null ? src.Account.Photo.Url : null));
        CreateMap<CreateDoctorDto, Doctor>();
        CreateMap<EditDoctorProfileDto, Doctor>();
    }
}