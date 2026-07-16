using ProfilesApi.Application.Dto.Photos;
using ProfilesApi.Domain.Enums;

namespace ProfilesApi.Application.Dto.Accounts;

public record AccountDto(Guid Id,
    string Firstname,
    string Lastname,
    DateTime Birthday,
    string PhoneNumber,
    string Email,
    Roles Role,
    string? PhotoUrl,
    Guid? PhotoId);