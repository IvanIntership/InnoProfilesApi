using ProfilesApi.Application.Interfaces;

namespace ProfilesApi.Application.Dto.Doctors;

public record DoctorDto(
    Guid Id,
    string Firstname,
    string Lastname,
    DateTime Birthday,
    string PhoneNumber,
    string Email,
    string Password,
    string Degree,
    Guid OfficeId,
    Guid SpecializationId,
    Guid? PhotoId,
    string? PhotoUrl,
    DateTime CareerStartDate,
    int GapInMonths);