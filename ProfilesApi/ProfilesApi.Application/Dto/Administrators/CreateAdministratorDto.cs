using ProfilesApi.Application.Interfaces;

namespace ProfilesApi.Application.Dto.Administrators;

public record CreateAdministratorDto(
    string Firstname,
    string Lastname,
    DateTime Birthday,
    string PhoneNumber,
    string Email,
    string Password,
    Guid? PhotoId,
    Guid OfficeId,
    DateTime CareerStartDate,
    int GapInMonths
) : IBaseProfileDto, IWithPassword;