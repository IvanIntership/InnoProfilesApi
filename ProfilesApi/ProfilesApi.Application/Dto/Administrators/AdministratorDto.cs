using ProfilesApi.Application.Dto.Accounts;
using ProfilesApi.Application.Dto.Offices;

namespace ProfilesApi.Application.Dto.Administrators;

public record AdministratorDto(Guid Id,
    AccountDto Account,
    OfficeDto Office,
    int TotalExperience);