using System.Data;
using AutoMapper;
using ProfilesApi.Application.Dto.Shared;
using ProfilesApi.Application.Dto.Specializations;
using ProfilesApi.Application.Interfaces;
using ProfilesApi.Domain.Entities;
using ProfilesApi.Domain.Exceptions;

namespace ProfilesApi.Application.Services;

public class SpecializationService : ISpecializationService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    
    public SpecializationService(IMapper mapper, 
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<SpecializationDto> CreateSpecializationAsync(CreateSpecializationDto createSpecializationDto, CancellationToken ct = default)
    {
        await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, ct);
        try
        {
            var specialization = _mapper.Map<Specialization>(createSpecializationDto);

            bool alreadyExists =
                await _unitOfWork.Specializations.ExistsAsync(
                    s => s.Name.ToLower() == createSpecializationDto.Name.ToLower(), ct);

            if (alreadyExists)
            {
                throw new ConflictException("Specialization already exists");
            }

            _unitOfWork.Specializations.Add(specialization);
            await _unitOfWork.CompleteAsync(ct);

            return _mapper.Map<SpecializationDto>(specialization);
        }
        catch
        {
            await _unitOfWork.CompleteAsync(ct);
            throw;
        }
    }

    public async Task<IEnumerable<SpecializationDto>> GetSpecializationsAsync(
        SearchQueryDto? searchQueryDto = null, 
        CancellationToken ct = default)
    {
        var searchTerm = searchQueryDto?.SearchTerm?.Trim().ToLower();

        IEnumerable<Specialization> specializations;

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            specializations = await _unitOfWork.Specializations.SearchByTerm(searchTerm, ct);
        }
        else
        {
            specializations = await _unitOfWork.Specializations.GetAllAsync(cancellationToken: ct);
        }

        return _mapper.Map<IEnumerable<SpecializationDto>>(specializations);
    }

    public async Task<SpecializationDto> GetSpecializationByIdAsync(Guid id, CancellationToken ct = default)
    {
        var specialization = await _unitOfWork.Specializations.GetByIdAsync(id, ct);
        
        if (specialization == null)
        {
            throw new NotFoundException($"Specialization with ID '{id}' was not found.");
        }
        
        return _mapper.Map<SpecializationDto>(specialization);
    }

    public async Task DeleteSpecializationAsync(Guid id, CancellationToken ct = default)
    {
        await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, ct);
        try
        {
            var specialization = await _unitOfWork.Specializations.GetByIdAsync(id, ct);

            if (specialization == null)
            {
                throw new NotFoundException($"Specialization with ID '{id}' was not found.");
            }

            var hasAssociatedDoctors = await _unitOfWork.Doctors.ExistsAsync(d => d.SpecializationId == id, ct);

            if (hasAssociatedDoctors)
            {
                throw new ConflictException(
                    "Cannot delete specialization because it is assigned to one or more doctors.");
            }

            _unitOfWork.Specializations.Delete(specialization);
            await _unitOfWork.CompleteAsync(ct);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(ct);
            throw;
        }
    }

    public async Task EditSpecializationAsync(EditSpecializationInformationDto editSpecializationInformationDto,
        CancellationToken ct = default)
    {
        await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, ct);
        try
        {
            var existingSpecialization =
                await _unitOfWork.Specializations.GetByIdAsync(editSpecializationInformationDto.Id, ct);

            if (existingSpecialization == null)
            {
                throw new NotFoundException("Specialization was not found.");
            }

            _mapper.Map(editSpecializationInformationDto, existingSpecialization);
            await _unitOfWork.CompleteAsync(ct);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(ct);
            throw;
        }
    }
}