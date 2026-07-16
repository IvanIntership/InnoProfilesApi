using ProfilesApi.Application.Interfaces;

namespace ProfilesApi.Application.Dto.Offices;

public record CreateOfficeDto(
    string Address, 
    string PhoneNumber, 
    Guid? PhotoId) : IOfficeDto;