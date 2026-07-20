using ProfilesApi.Application.Dto.Doctors;

namespace ProfilesApi.Application.Interfaces;

public interface IDoctorService
{
    Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto createDoctorDto, CancellationToken ct = default);
    
    Task DeleteDoctorAsync(Guid id, CancellationToken ct = default);
    
    Task<EditDoctorProfileDto> UpdateDoctorProfileAsync(EditDoctorProfileDto editDoctorProfileDto, CancellationToken ct = default);
    
    Task<DoctorDto> GetDoctorAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<DoctorDto>> GetDoctorsAsync(SearchFilteredDoctorListDto filteredDoctorListDto, CancellationToken ct = default);
}