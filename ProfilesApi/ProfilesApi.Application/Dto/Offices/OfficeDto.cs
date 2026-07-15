using ProfilesApi.Application.Dto.Photos;

namespace ProfilesApi.Application.Dto.Offices;

public record OfficeDto(Guid Id,
    string Address,
    string PhoneNumber,
    PhotoDto? Photo);