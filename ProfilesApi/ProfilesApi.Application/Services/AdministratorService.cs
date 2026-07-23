using AutoMapper;
using FluentValidation;
using ProfilesApi.Application.Dto.Administrators;
using ProfilesApi.Application.Interfaces;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Services;

public class AdministratorService : IAdministratorService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IValidator<CreateAdministratorDto> _createAdministratorDtoValidator;
    private readonly IValidator<EditAdministratorProfileDto> _editAdministratorProfileDtoValidator;
    private readonly IValidator<SearchFilteredAdministratorListDto> _searchFilteredAdministratorListDtoValidator;

    public AdministratorService(
        IMapper mapper, 
        IUnitOfWork unitOfWork, 
        IPasswordHasher passwordHasher, 
        IValidator<CreateAdministratorDto> createAdministratorDtoValidator, 
        IValidator<EditAdministratorProfileDto> editAdministratorProfileDtoValidator, 
        IValidator<SearchFilteredAdministratorListDto> searchFilteredAdministratorListDtoValidator)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _editAdministratorProfileDtoValidator = editAdministratorProfileDtoValidator ?? throw new ArgumentNullException(nameof(editAdministratorProfileDtoValidator));
        _createAdministratorDtoValidator = createAdministratorDtoValidator ?? throw new ArgumentNullException(nameof(createAdministratorDtoValidator));
        _searchFilteredAdministratorListDtoValidator = searchFilteredAdministratorListDtoValidator ?? throw new ArgumentNullException(nameof(searchFilteredAdministratorListDtoValidator));
    }

    public async Task<AdministratorDto> CreateAdministratorAsync(CreateAdministratorDto createAdministratorDto, Guid createdById,
        CancellationToken ct = default)
    {
        var validationResult = await _createAdministratorDtoValidator.ValidateAsync(createAdministratorDto, ct);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        var emailExists = await _unitOfWork.Accounts.ExistsAsync(a => a.Email == createAdministratorDto.Email, ct);
        var numberExists = await _unitOfWork.Accounts.ExistsAsync(a=> a.PhoneNumber == createAdministratorDto.PhoneNumber, ct);

        if (emailExists)
        {
            throw new InvalidOperationException("Email is already in use by another account.");
        }

        if (numberExists)
        {
            throw new InvalidOperationException("Phone number is already in use by another account.");
        }
        
        var officeExists = await _unitOfWork.Offices.ExistsAsync(o => o.Id == createAdministratorDto.OfficeId, ct);
        if (!officeExists)
        {
            throw new KeyNotFoundException($"Office with ID '{createAdministratorDto.OfficeId}' was not found.");
        }
        
        var account = _mapper.Map<Account>(createAdministratorDto);
        account.CreatedBy = createdById;
        account.UpdatedBy = createdById;
        account.PasswordHash = _passwordHasher.HashPassword(createAdministratorDto.Password);

        var administrator = _mapper.Map<Administrator>(createAdministratorDto);

        administrator.AccountId = account.Id;
        administrator.Account = account;
        
        _unitOfWork.Accounts.Add(account);
        _unitOfWork.Administrators.Add(administrator);

        await _unitOfWork.CompleteAsync(ct);
        
        return _mapper.Map<AdministratorDto>(administrator);
    }

    public async Task DeleteAdministratorAsync(Guid id, CancellationToken ct = default)
    {
        var administrator = await _unitOfWork.Administrators.GetWithDetailsAsync(id, ct);

        if (administrator == null)
        {
            throw new KeyNotFoundException($"Administrator with ID '{id}' was not found.");
        }
        
        var totalAdminsCount = await _unitOfWork.Administrators.ExistsAsync(a => a.Id != id, ct);
        if (!totalAdminsCount)
        {
            throw new InvalidOperationException("Cannot delete the last administrator in the system.");
        }
        
        _unitOfWork.Administrators.Delete(administrator);
        _unitOfWork.Accounts.Delete(administrator.Account);

        await _unitOfWork.CompleteAsync(ct);
    }

    public async Task<AdministratorDto> EditAdministratorProfileAsync(EditAdministratorProfileDto editAdministratorProfileDto, Guid editedById,
        CancellationToken ct = default)
    {
        var validationResult = await _editAdministratorProfileDtoValidator.ValidateAsync(editAdministratorProfileDto, ct);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        var administrator = await _unitOfWork.Administrators.GetWithDetailsAsync(editAdministratorProfileDto.Id, ct);

        if (administrator == null)
        {
            throw new KeyNotFoundException($"Administrator with ID '{editAdministratorProfileDto.Id}' was not found.");
        }
        
        var officeExists = await _unitOfWork.Offices.ExistsAsync(o => o.Id == editAdministratorProfileDto.OfficeId, ct);
        if (!officeExists)
        {
            throw new KeyNotFoundException($"Office with ID '{editAdministratorProfileDto.OfficeId}' was not found.");
        }
        
        var phoneExists = await _unitOfWork.Accounts.ExistsAsync(
            a => a.Id != administrator.AccountId && a.PhoneNumber == editAdministratorProfileDto.PhoneNumber, ct);
        if (phoneExists)
        {
            throw new InvalidOperationException("Phone number is already in use by another account.");
        }
        
        var emailExists = await _unitOfWork.Accounts.ExistsAsync(
            a => a.Id != administrator.AccountId && a.Email == editAdministratorProfileDto.Email, ct);
        if (emailExists)
        {
            throw new InvalidOperationException("Email is already in use by another account.");
        }

        _mapper.Map(editAdministratorProfileDto, administrator);

        administrator.Account.UpdatedBy = editedById;
        
        await _unitOfWork.CompleteAsync(ct);
        return _mapper.Map<AdministratorDto>(administrator);
    }

    public async Task<AdministratorDto> GetAdministratorAsync(Guid id, CancellationToken ct = default)
    {
        var administrator = await _unitOfWork.Administrators.GetWithDetailsAsync(id, ct);
    
        if (administrator == null)
        {
            throw new KeyNotFoundException($"Administrator with ID '{id}' was not found.");
        }
    
        return _mapper.Map<AdministratorDto>(administrator);
    }

    public async Task<IEnumerable<AdministratorDto>> GetAdministratorsAsync(
        SearchFilteredAdministratorListDto? filteredAdministratorListDto,
        CancellationToken ct = default)
    {
        if (filteredAdministratorListDto != null)
        {
            var validationResult = await _searchFilteredAdministratorListDtoValidator.ValidateAsync(filteredAdministratorListDto, ct);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }

        var searchTerm = filteredAdministratorListDto?.SearchTerm?.Trim().ToLower();
        var officeId = filteredAdministratorListDto?.OfficeId;
    
        var administrators = await _unitOfWork.Administrators.GetAllAsync(
            filter: a => 
                (!officeId.HasValue || a.OfficeId == officeId.Value) &&
                (string.IsNullOrWhiteSpace(searchTerm) ||
                 a.Account.Firstname.ToLower().Contains(searchTerm) ||
                 a.Account.Lastname.ToLower().Contains(searchTerm) ||
                 (a.Account.Firstname + " " + a.Account.Lastname).ToLower().Contains(searchTerm) ||
                 (a.Account.Lastname + " " + a.Account.Firstname).ToLower().Contains(searchTerm)),
    
            cancellationToken: ct,

            includesProperties:
            [
                a => a.Account,
                a => a.Office
            ]
        );
    
        return _mapper.Map<IEnumerable<AdministratorDto>>(administrators);
    }

    public async Task<AdministratorDto> GetByAccountIdAsync(Guid accountId, CancellationToken ct = default)
    {
        var administrator = await _unitOfWork.Administrators.GetByAccountIdAsync(accountId, ct);
    
        if (administrator == null)
        {
            throw new KeyNotFoundException($"Administrator with account ID '{accountId}' was not found.");
        }
    
        return _mapper.Map<AdministratorDto>(administrator);
    }
}