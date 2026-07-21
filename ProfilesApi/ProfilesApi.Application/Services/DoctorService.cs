using AutoMapper;
using ProfilesApi.Application.Dto.Doctors;
using ProfilesApi.Application.Interfaces;

namespace ProfilesApi.Application.Services;

public class DoctorService : IDoctorService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    
    public DoctorService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto createDoctorDto, Guid? createdById = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteDoctorAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<EditDoctorProfileDto> EditDoctorProfileAsync(EditDoctorProfileDto editDoctorProfileDto, Guid? editdById = null,
        CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<DoctorDto> GetDoctorAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<DoctorDto>> GetDoctorsAsync(SearchFilteredDoctorListDto filteredDoctorListDto, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}