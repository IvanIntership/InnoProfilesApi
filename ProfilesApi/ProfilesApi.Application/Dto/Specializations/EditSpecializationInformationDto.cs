namespace ProfilesApi.Application.Dto.Specializations;

public record EditSpecializationInformationDto(
    Guid SpecializationId,
    string Name);