using AutoMapper;
using ProfilesApi.Application.Dto.Offices;
using ProfilesApi.Application.Dto.Shared;
using ProfilesApi.Application.Interfaces;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Services;

public class OfficeService : IOfficeService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    
    public OfficeService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<OfficeDto> CreateOfficeAsync(CreateOfficeDto createOfficeDto, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(createOfficeDto);
        
        var office = _mapper.Map<Office>(createOfficeDto);
        
        var alreadyExists = await _unitOfWork.Offices.ExistsAsync(o => o.Address.ToLower() == createOfficeDto.Address.ToLower() || o.PhoneNumber == createOfficeDto.PhoneNumber, ct);
        
        if (alreadyExists)
        {
            throw new InvalidOperationException("Office with this address or phone number already exists.");
        }
        
        _unitOfWork.Offices.Add(office);
        await _unitOfWork.CompleteAsync(ct);
        
        return _mapper.Map<OfficeDto>(office);
    }

    public async Task<IEnumerable<OfficeDto>> GetOfficeListAsync(SearchQueryDto? searchQueryDto, CancellationToken ct = default)
    {
        var searchQuery = searchQueryDto?.SearchTerm.Trim().ToLower();
        
        var filteredOffices = await _unitOfWork.Offices.GetAllAsync(o => string.IsNullOrEmpty(searchQuery) 
                                                                  || o.Address.ToLower().Contains(searchQuery) 
                                                                  || o.PhoneNumber.Contains(searchQuery), ct);
        
        return _mapper.Map<IEnumerable<OfficeDto>>(filteredOffices);
    }

    public async Task<OfficeDto> GetOfficeByIdAsync(Guid id, CancellationToken ct = default)
    {
        var office = await _unitOfWork.Offices.GetByIdAsync(id, ct);

        if (office == null)
        {
            throw new ArgumentException($"Office with id {id} does not exist");
        }
        
        return _mapper.Map<OfficeDto>(office);
    }

    public async Task DeleteOfficeAsync(Guid id, CancellationToken ct = default)
    {
        var office = await _unitOfWork.Offices.GetByIdAsync(id, ct);

        if (office == null)
        {
            throw new KeyNotFoundException($"Office with ID '{id}' was not found.");
        }

        var hasAssociatedDoctors = await _unitOfWork.Doctors.ExistsAsync(d => d.OfficeId == id, ct);
        var hasAssociatedAdministrators = await _unitOfWork.Administrators.ExistsAsync(a => a.OfficeId == id, ct);

        if (hasAssociatedDoctors || hasAssociatedAdministrators)
        {
            throw new InvalidOperationException("Cannot delete office because people work here.");
        }
        
        _unitOfWork.Offices.Delete(office);
        await _unitOfWork.CompleteAsync(ct);
    }

    public async Task EditOfficeAsync(EditOfficeInformationDto editOfficeInformationDto, CancellationToken ct = default)
    {
        var existingOffice = await _unitOfWork.Offices.GetByIdAsync(editOfficeInformationDto.Id, ct);
        
        if (existingOffice == null)
        {
            throw new KeyNotFoundException("Office was not found.");
        }
        
        var isDuplicate = await _unitOfWork.Offices.ExistsAsync(
            o => o.Id != editOfficeInformationDto.Id && 
                 (o.Address.ToLower() == editOfficeInformationDto.Address.ToLower() || o.PhoneNumber == editOfficeInformationDto.PhoneNumber), 
            ct
        );

        if (isDuplicate)
        {
            throw new InvalidOperationException("Another office with this address or phone number already exists.");
        }
        
        _mapper.Map(editOfficeInformationDto, existingOffice);
        await _unitOfWork.CompleteAsync(ct);
    }
}