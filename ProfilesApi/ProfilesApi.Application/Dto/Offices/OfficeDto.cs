using ProfilesApi.Application.Dto.Photos;
using ProfilesApi.Application.Interfaces;

namespace ProfilesApi.Application.Dto.Offices;

public record OfficeDto(
    Guid Id,
    string Address,
    string PhoneNumber,
    Guid? PhotoId,
    string? PhotoUrl) : IOfficeDto;