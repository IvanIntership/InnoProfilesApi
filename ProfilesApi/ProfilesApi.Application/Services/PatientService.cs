using AutoMapper;
using ProfilesApi.Application.Dto.Patients;
using ProfilesApi.Application.Interfaces;

namespace ProfilesApi.Application.Services;

public class PatientService : IPatientService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    
    public PatientService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<PatientDto> CreatePatientAsync(RegisterPatientDto dto, Guid? createdById = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<PatientDto> GetPatientAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<PatientDto>> GetPatientsAsync(SearchFilteredPatientListDto filteredPatientListDto, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<PatientDto> EditPatientAsync(EditPatientProfileDto dto, Guid? editdById = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task DeletePatientAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}