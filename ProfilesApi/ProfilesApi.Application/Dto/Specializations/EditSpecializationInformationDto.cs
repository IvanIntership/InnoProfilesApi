namespace ProfilesApi.Application.Dto.Specializations;

public record EditSpecializationInformationDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}