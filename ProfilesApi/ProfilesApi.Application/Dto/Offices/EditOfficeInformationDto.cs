namespace ProfilesApi.Application.Dto.Offices;

public record EditOfficeInformationDto(string Address,
    string PhoneNumber,
    Guid? PhotoId);