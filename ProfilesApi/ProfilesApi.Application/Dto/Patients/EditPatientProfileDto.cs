using ProfilesApi.Application.Interfaces;

namespace ProfilesApi.Application.Dto.Patients;

public record EditPatientProfileDto(
    Guid Id,
    string Firstname,
    string Lastname,
    DateTime Birthday,
    string PhoneNumber,
    string Email,
    Guid? PhotoId) : IBaseProfileDto;