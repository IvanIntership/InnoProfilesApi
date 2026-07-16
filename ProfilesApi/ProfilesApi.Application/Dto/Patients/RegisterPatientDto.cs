using ProfilesApi.Application.Interfaces;

namespace ProfilesApi.Application.Dto.Patients;

public record RegisterPatientDto(
    string Firstname,
    string Lastname,
    DateTime Birthday,
    string PhoneNumber,
    string Email,
    Guid? PhotoId,
    string Password) : IBaseProfileDto, IWithPassword;