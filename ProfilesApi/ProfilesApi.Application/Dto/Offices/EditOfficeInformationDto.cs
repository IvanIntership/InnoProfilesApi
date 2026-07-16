using ProfilesApi.Application.Interfaces;

namespace ProfilesApi.Application.Dto.Offices;

public record EditOfficeInformationDto(
    Guid Id,
    string Address,
    string PhoneNumber,
    Guid? PhotoId) : IOfficeDto;