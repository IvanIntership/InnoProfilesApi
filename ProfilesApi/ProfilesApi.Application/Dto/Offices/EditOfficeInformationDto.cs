namespace ProfilesApi.Application.Dto.Offices;

public record EditOfficeInformationDto(
    Guid OfficeId,
    string Address,
    string PhoneNumber,
    Guid? PhotoId);