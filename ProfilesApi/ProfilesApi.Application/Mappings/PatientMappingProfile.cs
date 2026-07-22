using AutoMapper;
using ProfilesApi.Application.Dto.Patients;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Mappings;

public class PatientMappingProfile : Profile
{
    public PatientMappingProfile()
    {
        CreateMap<Patient, PatientDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.Account.Firstname))
            .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => src.Account.Lastname))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Account.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Account.PhoneNumber))
            .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Account.Birthday))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Account.Role))
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Account.Photo != null ? src.Account.Photo.Url : null));
        CreateMap<EditPatientProfileDto, Patient>();
    }
}