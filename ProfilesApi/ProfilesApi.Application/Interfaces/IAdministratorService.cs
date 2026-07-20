using ProfilesApi.Application.Dto.Administrators;

namespace ProfilesApi.Application.Interfaces;

public interface IAdministratorService
{
    Task<AdministratorDto> CreateAdministratorAsync(CreateAdministratorDto createAdministratorDto, Guid? createdById = null, CancellationToken ct = default);
    
    Task DeleteAdministratorAsync(Guid id, CancellationToken ct = default);
    
    Task<EditAdministratorProfileDto> EditAdministratorProfileAsync(EditAdministratorProfileDto editAdministratorProfileDto, Guid? editdById = null, CancellationToken ct = default);
    
    Task<AdministratorDto> GetAdministratorAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<AdministratorDto>> GetAdministratorsAsync(SearchFilteredAdministratorListDto filteredAdministratorListDto, CancellationToken ct = default);
}