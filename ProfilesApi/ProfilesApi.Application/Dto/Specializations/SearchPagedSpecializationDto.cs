namespace ProfilesApi.Application.Dto.Specializations;

public sealed record SearchPagedSpecializationDto
{
    public string SearchTerm { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}