namespace ProfilesApi.Application.Dto.Offices;

public record CreateOfficeDto(
    string Address, 
    string PhoneNumber, 
    string? PhotoUrl);