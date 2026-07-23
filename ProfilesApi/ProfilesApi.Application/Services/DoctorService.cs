using AutoMapper;
using FluentValidation;
using ProfilesApi.Application.Dto.Doctors;
using ProfilesApi.Application.Interfaces;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Services;

public class DoctorService : IDoctorService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IValidator<CreateDoctorDto> _createDoctorDtoValidator;
    private readonly IValidator<EditDoctorProfileDto> _editDoctorProfileDtoValidator;
    private readonly IValidator<SearchFilteredDoctorListDto> _searchFilteredDoctorListDtoValidator;
    
    public DoctorService(IMapper mapper,
        IUnitOfWork unitOfWork, 
        IPasswordHasher passwordHasher,
        IValidator<CreateDoctorDto> createDoctorDtoValidator,
        IValidator<EditDoctorProfileDto> editDoctorProfileDtoValidator,
        IValidator<SearchFilteredDoctorListDto> searchFilteredDoctorListDtoValidator)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _createDoctorDtoValidator = createDoctorDtoValidator ?? throw new ArgumentNullException(nameof(createDoctorDtoValidator));
        _editDoctorProfileDtoValidator = editDoctorProfileDtoValidator ?? throw new ArgumentNullException(nameof(editDoctorProfileDtoValidator));
        _searchFilteredDoctorListDtoValidator = searchFilteredDoctorListDtoValidator ?? throw new ArgumentNullException(nameof(searchFilteredDoctorListDtoValidator));
    }

    public async Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto createDoctorDto, Guid? createdById = null, CancellationToken ct = default)
    {
        var validationResult = await _createDoctorDtoValidator.ValidateAsync(createDoctorDto, ct);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        var emailExists = await _unitOfWork.Accounts.ExistsAsync(a => a.Email == createDoctorDto.Email, ct);
        var numberExists = await _unitOfWork.Accounts.ExistsAsync(a=> a.PhoneNumber == createDoctorDto.PhoneNumber, ct);

        if (emailExists)
        {
            throw new InvalidOperationException("Email is already in use by another account.");
        }

        if (numberExists)
        {
            throw new InvalidOperationException("Phone number is already in use by another account.");
        }
        
        var officeExists = await _unitOfWork.Offices.ExistsAsync(o => o.Id == createDoctorDto.OfficeId, ct);
        if (!officeExists)
        {
            throw new KeyNotFoundException($"Office with ID '{createDoctorDto.OfficeId}' was not found.");
        }
        
        var specializationExists = await _unitOfWork.Specializations.ExistsAsync(o => o.Id == createDoctorDto.SpecializationId, ct);
        if (!specializationExists)
        {
            throw new KeyNotFoundException($"Specialization with ID '{createDoctorDto.SpecializationId}' was not found.");
        }
        
        var account = _mapper.Map<Account>(createDoctorDto);
        
        createdById ??= account.Id;
        account.CreatedBy = createdById.Value;
        account.UpdatedBy = createdById.Value;
        account.PasswordHash = _passwordHasher.HashPassword(createDoctorDto.Password);

        var doctor = _mapper.Map<Doctor>(createDoctorDto);

        doctor.AccountId = account.Id;
        doctor.Account = account;
        
        _unitOfWork.Accounts.Add(account);
        _unitOfWork.Doctors.Add(doctor);

        await _unitOfWork.CompleteAsync(ct);
        
        return _mapper.Map<DoctorDto>(doctor);
    }

    public async Task DeleteDoctorAsync(Guid id, CancellationToken ct = default)
    {
        var doctor = await _unitOfWork.Doctors.GetWithDetailsAsync(id, ct);

        if (doctor == null)
        {
            throw new KeyNotFoundException($"Doctor with ID '{id}' was not found.");
        }
        
        _unitOfWork.Doctors.Delete(doctor);
        _unitOfWork.Accounts.Delete(doctor.Account);

        await _unitOfWork.CompleteAsync(ct);
    }

    public async Task<DoctorDto> EditDoctorProfileAsync(EditDoctorProfileDto editDoctorProfileDto, Guid? editedById = null,
        CancellationToken ct = default)
    {
        var validationResult = await _editDoctorProfileDtoValidator.ValidateAsync(editDoctorProfileDto, ct);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        var doctor = await _unitOfWork.Doctors.GetWithDetailsAsync(editDoctorProfileDto.Id, ct);

        if (doctor == null)
        {
            throw new KeyNotFoundException($"Doctor with ID '{editDoctorProfileDto.Id}' was not found.");
        }
        
        var officeExists = await _unitOfWork.Offices.ExistsAsync(o => o.Id == editDoctorProfileDto.OfficeId, ct);
        if (!officeExists)
        {
            throw new KeyNotFoundException($"Office with ID '{editDoctorProfileDto.OfficeId}' was not found.");
        }
        
        var specializationExists = await _unitOfWork.Specializations.ExistsAsync(o => o.Id == editDoctorProfileDto.SpecializationId, ct);
        if (!specializationExists)
        {
            throw new KeyNotFoundException($"Specialization with ID '{editDoctorProfileDto.SpecializationId}' was not found.");
        }
        
        var phoneExists = await _unitOfWork.Accounts.ExistsAsync(
            a => a.Id != doctor.AccountId && a.PhoneNumber == editDoctorProfileDto.PhoneNumber, ct);
        if (phoneExists)
        {
            throw new InvalidOperationException("Phone number is already in use by another account.");
        }
        
        var emailExists = await _unitOfWork.Accounts.ExistsAsync(
            a => a.Id != doctor.AccountId && a.Email == editDoctorProfileDto.Email, ct);
        if (emailExists)
        {
            throw new InvalidOperationException("Email is already in use by another account.");
        }

        _mapper.Map(editDoctorProfileDto, doctor);

        doctor.Account.UpdatedBy = editedById ?? doctor.Account.Id;
        
        await _unitOfWork.CompleteAsync(ct);
        return _mapper.Map<DoctorDto>(doctor);
    }

    public async Task<DoctorDto> GetDoctorAsync(Guid id, CancellationToken ct = default)
    {
        var doctor = await _unitOfWork.Doctors.GetWithDetailsAsync(id, ct);
    
        if (doctor == null)
        {
            throw new KeyNotFoundException($"Doctor with ID '{id}' was not found.");
        }
    
        return _mapper.Map<DoctorDto>(doctor);
    }

    public async Task<IEnumerable<DoctorDto>> GetDoctorsAsync(SearchFilteredDoctorListDto? filteredDoctorListDto, CancellationToken ct = default)
    {
        if (filteredDoctorListDto != null)
        {
            var validationResult = await _searchFilteredDoctorListDtoValidator.ValidateAsync(filteredDoctorListDto, ct);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }

        var searchTerm = filteredDoctorListDto?.SearchTerm?.Trim().ToLower();
        var officeId = filteredDoctorListDto?.OfficeId;
        var specializationId = filteredDoctorListDto?.SpecializationId;
        var minExperienceInYears = filteredDoctorListDto?.MinExperienceYears;
    
        DateTime? maxCareerStartDate = minExperienceInYears.HasValue 
            ? DateTime.UtcNow.AddYears(-minExperienceInYears.Value) 
            : null;

        var doctors = await _unitOfWork.Doctors.GetAllAsync(
            filter: d => 
                (!maxCareerStartDate.HasValue || d.CareerStartDate.AddMonths(d.GapInMonths) <= maxCareerStartDate.Value) &&
                (!specializationId.HasValue || d.SpecializationId == specializationId.Value) &&
                (!officeId.HasValue || d.OfficeId == officeId.Value) &&
                (string.IsNullOrWhiteSpace(searchTerm) ||
                 d.Account.Firstname.ToLower().Contains(searchTerm) ||
                 d.Account.Lastname.ToLower().Contains(searchTerm) ||
                 (d.Account.Firstname + " " + d.Account.Lastname).ToLower().Contains(searchTerm) ||
                 (d.Account.Lastname + " " + d.Account.Firstname).ToLower().Contains(searchTerm)),

            cancellationToken: ct,

            includesProperties:
            [
                d => d.Account,
                d => d.Office,
                d => d.Specialization
            ]
        );
    
        return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
    }

    public async Task<DoctorDto> GetByAccountIdAsync(Guid accountId, CancellationToken ct = default)
    {
        var doctor = await _unitOfWork.Doctors.GetByAccountIdAsync(accountId, ct);
    
        if (doctor == null)
        {
            throw new KeyNotFoundException($"Doctor with account ID '{accountId}' was not found.");
        }
    
        return _mapper.Map<DoctorDto>(doctor);
    }
}