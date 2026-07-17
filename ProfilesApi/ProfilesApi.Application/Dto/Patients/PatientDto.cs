using ProfilesApi.Domain.Enums;

namespace ProfilesApi.Application.Dto.Patients;

public record PatientDto(
    Guid Id, 
    string Firstname,
    string Lastname,
    DateTime Birthday,
    string PhoneNumber,
    string Email,
    Roles Role,
    string? PhotoUrl,
    Guid? PhotoId);