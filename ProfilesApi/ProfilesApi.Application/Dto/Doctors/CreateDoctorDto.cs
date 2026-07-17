using ProfilesApi.Application.Interfaces;

namespace ProfilesApi.Application.Dto.Doctors;

public record CreateDoctorDto(
    string Firstname,
    string Lastname,
    DateTime Birthday,
    string PhoneNumber,
    string Email,
    string Password,
    Guid OfficeId,
    Guid SpecializationId,
    DateTime CareerStartDate,
    int GapInMonths,
    Guid? PhotoId,
    string Degree);