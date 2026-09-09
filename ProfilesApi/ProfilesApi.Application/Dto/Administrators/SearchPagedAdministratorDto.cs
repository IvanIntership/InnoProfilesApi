namespace ProfilesApi.Application.Dto.Administrators;

public sealed record SearchPagedAdministratorDto
{
    public string? SearchTerm { get; init; } = null;
    public Guid? OfficeId { get; init; } = null;
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}