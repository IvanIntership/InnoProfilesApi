namespace ProfilesApi.Application.Dto.Offices;

public sealed record SearchPagedOfficeDto
{
    public string SearchTerm { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}