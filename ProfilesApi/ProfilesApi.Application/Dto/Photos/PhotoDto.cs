namespace ProfilesApi.Application.Dto.Photos;

public sealed record PhotoDto
{
    public Guid Id { get; init; }
    public string Url { get; init; }
}