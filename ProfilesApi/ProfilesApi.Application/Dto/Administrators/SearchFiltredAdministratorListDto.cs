namespace ProfilesApi.Application.Dto.Administrators;

public record SearchFilteredAdministratorListDto
{
    public string? SearchTerm { get; init; } = null;
    public Guid? OfficeId { get; init; } = null;
}