namespace ProfilesApi.Application.Dto.Specializations;

public sealed record SpecializationDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}