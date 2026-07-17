using AutoMapper;
using ProfilesApi.Application.Dto.Patients;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Mappings;

public class PatientMappingProfile : Profile
{
    public PatientMappingProfile()
    {
        CreateMap<Patient, PatientDto>();
        CreateMap<EditPatientProfileDto, Patient>();
    }
}