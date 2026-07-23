using ProfilesApi.Application.Dto.Patients;

namespace ProfilesApi.Application.Interfaces;

public interface IPatientService
{
    Task<PatientDto> CreatePatientAsync(RegisterPatientDto dto, Guid? createdById = null, CancellationToken ct = default);
    
    Task<PatientDto> GetPatientAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<PatientDto>> GetPatientsAsync(SearchFilteredPatientListDto filteredPatientListDto, CancellationToken ct = default);
    
    Task<PatientDto> EditPatientAsync(EditPatientProfileDto dto, Guid? editdById = null, CancellationToken ct = default);
    
    Task DeletePatientAsync(Guid id, CancellationToken ct = default);
    Task<PatientDto> GetByAccountIdAsync(Guid accountId, CancellationToken ct = default);
}