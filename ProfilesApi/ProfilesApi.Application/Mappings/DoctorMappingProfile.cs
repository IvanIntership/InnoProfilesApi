using AutoMapper;
using ProfilesApi.Application.Dto.Doctors;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Mappings;

public class DoctorMappingProfile : Profile
{
    public DoctorMappingProfile()
    {
        CreateMap<Doctor, DoctorDto>();
        CreateMap<CreateDoctorDto, Doctor>();
        CreateProjection<EditDoctorProfileDto, Doctor>();
    }
}