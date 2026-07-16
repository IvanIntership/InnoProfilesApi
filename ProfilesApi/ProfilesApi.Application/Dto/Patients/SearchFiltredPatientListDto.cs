using ProfilesApi.Application.Interfaces;

namespace ProfilesApi.Application.Dto.Patients;

public record SearchFilteredPatientListDto(
    string? SearchTerm = null,
    string? Email = null,
    string? PhoneNumber = null) : ISearchList;