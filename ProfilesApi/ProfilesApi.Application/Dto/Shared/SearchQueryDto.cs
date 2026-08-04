namespace ProfilesApi.Application.Dto.Shared;

public sealed record SearchQueryDto
{
    public string SearchTerm { get; init; }
}