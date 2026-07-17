using ProfilesApi.Application.Interfaces;

namespace ProfilesApi.Application.Dto.Administrators;

public record EditAdministratorProfileDto(
    Guid Id,
    string Firstname,
    string Lastname,
    DateTime Birthday,
    string PhoneNumber,
    string Email,
    Guid? PhotoId,
    Guid OfficeId,
    DateTime CareerStartDate,
    int GapInMonths
) ;