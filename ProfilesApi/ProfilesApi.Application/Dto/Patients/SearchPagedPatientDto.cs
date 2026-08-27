namespace ProfilesApi.Application.Dto.Patients;

public sealed record SearchPagedPatientDto
{
    public string? SearchTerm { get; init; } = null;
    public string? Email { get; init; } = null;
    public string? PhoneNumber { get; init; } = null;
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}