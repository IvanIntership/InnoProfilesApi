namespace ProfilesApi.Application.Dto.Patients;

public sealed record SearchFilteredPatientListDto
{
    public string? SearchTerm { get; init; } = null;
    public string? Email { get; init; } = null;
    public string? PhoneNumber { get; init; } = null;
}