using ProfilesApi.Application.Dto.Offices;
using ProfilesApi.Application.Dto.Shared;

namespace ProfilesApi.Application.Interfaces;

public interface IOfficeService
{
    Task<OfficeDto> CreateOfficeAsync(CreateOfficeDto createOfficeDto, CancellationToken ct = default);
    
    Task<IEnumerable<OfficeDto>> GetOfficeByAddressAsync(SearchQueryDto searchQueryDto, CancellationToken ct = default);
    Task<IEnumerable<OfficeDto>> GetOfficeByPhoneNumberAsync(SearchQueryDto searchQueryDto, CancellationToken ct = default);
    Task<IEnumerable<OfficeDto>> GetOfficeListAsync(CancellationToken ct = default);
    Task<OfficeDto?> GetOfficeByIdAsync(Guid id, CancellationToken ct = default);
    
    Task DeleteOfficeAsync(Guid id, CancellationToken ct = default);
    
    Task UpdateOfficeAsync(EditOfficeInformationDto editOfficeInformationDto, CancellationToken ct = default);
}