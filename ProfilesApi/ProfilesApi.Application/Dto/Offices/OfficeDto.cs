namespace ProfilesApi.Application.Dto.Offices;

public record OfficeDto(
    Guid Id,
    string Address,
    string PhoneNumber,
    Guid? PhotoId,
    string? PhotoUrl);