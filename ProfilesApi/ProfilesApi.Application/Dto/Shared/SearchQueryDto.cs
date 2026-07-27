namespace ProfilesApi.Application.Dto.Shared;

public record SearchQueryDto
{
    public string SearchTerm { get; init; }
}