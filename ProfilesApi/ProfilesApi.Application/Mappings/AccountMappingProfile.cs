using AutoMapper;
using ProfilesApi.Application.Dto.Accounts;
using ProfilesApi.Application.Dto.Administrators;
using ProfilesApi.Application.Dto.Doctors;
using ProfilesApi.Application.Dto.Patients;
using ProfilesApi.Domain.Entities;
using ProfilesApi.Domain.Enums;

namespace ProfilesApi.Application.Mappings;

public class AccountMappingProfile : Profile
{
    public AccountMappingProfile()
    {
        CreateMap<Account, AccountDto>()
            .ForMember(
                dest => dest.PhotoUrl,
                opt => opt.MapFrom(src => src.Photo!= null ? src.Photo.Url : null));
        
        CreateMap<Account, AdministratorDto>()
            .ForMember(
                dest => dest.PhotoUrl,
                opt => opt.MapFrom(src => src.Photo!= null ? src.Photo.Url : null));

        CreateMap<CreateAdministratorDto, Account>()
            .ForMember(
                dest => dest.Role, 
                opt => opt.MapFrom(src => Roles.Administrator)
            );
        
        CreateMap<EditAdministratorProfileDto, Account>()
            .ForMember(
                dest => dest.Role, 
                opt => opt.MapFrom(src => Roles.Administrator)
            );
        
        CreateMap<Account, DoctorDto>()
            .ForMember(
                dest => dest.PhotoUrl,
                opt => opt.MapFrom(src => src.Photo!= null ? src.Photo.Url : null));
        
        CreateMap<CreateDoctorDto, Account>()
            .ForMember(
                dest => dest.Role, 
                opt => opt.MapFrom(src => Roles.Doctor)
            );
        
        CreateMap<EditDoctorProfileDto, Account>()
            .ForMember(
                dest => dest.Role, 
                opt => opt.MapFrom(src => Roles.Doctor)
            );
        
        CreateMap<Account, PatientDto>()
            .ForMember(
                dest => dest.PhotoUrl,
                opt => opt.MapFrom(src => src.Photo!= null ? src.Photo.Url : null));

        CreateMap<EditPatientProfileDto, Account>();
        CreateMap<RegisterPatientDto, Account>();
    }
}