using ProfilesApi.Application.Interfaces;

namespace ProfilesApi.Application.Dto.Administrators;

public record SearchFilteredAdministratorListDto(
    string? SearchTerm  = null,   
    Guid? OfficeId  = null)  : ISearchList;