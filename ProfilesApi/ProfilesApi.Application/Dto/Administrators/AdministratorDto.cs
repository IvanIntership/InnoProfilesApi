using ProfilesApi.Application.Dto.Accounts;
using ProfilesApi.Application.Dto.Offices;
using ProfilesApi.Domain.Enums;

namespace ProfilesApi.Application.Dto.Administrators;

public record AdministratorDto(Guid Id,
    string Firstname,
    string Lastname,
    DateTime Birthday,
    string PhoneNumber,
    string Email,
    Roles Role,
    string? PhotoUrl,
    Guid? PhotoId,
    Guid OfficeId,
    int TotalExperience);