using AutoMapper;
using FluentValidation;
using ProfilesApi.Application.Dto.Patients;
using ProfilesApi.Application.Interfaces;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Services;

public class PatientService : IPatientService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IValidator<RegisterPatientDto> _registerPatientDtoValidator;
    private readonly IValidator<EditPatientProfileDto> _editPatientProfileDtoValidator;
    private readonly IValidator<SearchFilteredPatientListDto> _searchFilteredPatientListDtoValidator;
    
    public PatientService(IMapper mapper, 
        IUnitOfWork unitOfWork, 
        IPasswordHasher passwordHasher,
        IValidator <RegisterPatientDto> registerPatientDtoValidator,
        IValidator <EditPatientProfileDto> editPatientProfileDtoValidator,
        IValidator <SearchFilteredPatientListDto> searchFilteredPatientListDtoValidator)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _registerPatientDtoValidator = registerPatientDtoValidator ?? throw new ArgumentNullException(nameof(registerPatientDtoValidator));
        _editPatientProfileDtoValidator = editPatientProfileDtoValidator ?? throw new ArgumentNullException(nameof(editPatientProfileDtoValidator));
        _searchFilteredPatientListDtoValidator =  searchFilteredPatientListDtoValidator ?? throw new ArgumentNullException(nameof(searchFilteredPatientListDtoValidator));
    }

    public async Task<PatientDto> CreatePatientAsync(RegisterPatientDto registerPatientDto, Guid? createdById = null, CancellationToken ct = default)
    {
        var validationResult = await _registerPatientDtoValidator.ValidateAsync(registerPatientDto, ct);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        var emailExists = await _unitOfWork.Accounts.ExistsAsync(a => a.Email == registerPatientDto.Email, ct);
        var numberExists = await _unitOfWork.Accounts.ExistsAsync(a=> a.PhoneNumber == registerPatientDto.PhoneNumber, ct);

        if (emailExists)
        {
            throw new InvalidOperationException("Email is already in use by another account.");
        }

        if (numberExists)
        {
            throw new InvalidOperationException("Phone number is already in use by another account.");
        }

        var patient = _mapper.Map<Patient>(registerPatientDto);
        var account = _mapper.Map<Account>(registerPatientDto);
        
        createdById ??= account.Id;
        
        account.CreatedBy = createdById.Value;
        account.UpdatedBy = createdById.Value;
        account.PasswordHash = _passwordHasher.HashPassword(registerPatientDto.Password);
        
        patient.AccountId = account.Id;
        patient.Account = account;
        
        _unitOfWork.Accounts.Add(account);
        _unitOfWork.Patients.Add(patient);
        await _unitOfWork.CompleteAsync(ct);
        
        return _mapper.Map<PatientDto>(patient);
    }

    public async Task<PatientDto> GetPatientAsync(Guid id, CancellationToken ct = default)
    {
        var patient = await _unitOfWork.Patients.GetWithDetailsAsync(id, ct);
        
        if (patient == null)
        {
            throw new KeyNotFoundException($"Patient with ID '{id}' was not found.");
        }
        
        return _mapper.Map<PatientDto>(patient);
    }

    public async Task<IEnumerable<PatientDto>> GetPatientsAsync(SearchFilteredPatientListDto? filteredPatientListDto, CancellationToken ct = default)
    {
        if (filteredPatientListDto != null)
        {
            var validationResult = await _searchFilteredPatientListDtoValidator.ValidateAsync(filteredPatientListDto, ct);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }

        var searchTerm = filteredPatientListDto?.SearchTerm?.Trim().ToLower();
        var phoneNumber = filteredPatientListDto?.PhoneNumber?.Trim();
        var email = filteredPatientListDto?.Email?.Trim();
    
        var patients = await _unitOfWork.Patients.GetAllAsync(
            filter: a => 
                (string.IsNullOrWhiteSpace(phoneNumber) || a.Account.PhoneNumber.Contains(phoneNumber)) &&
                (string.IsNullOrWhiteSpace(email) || a.Account.Email.Contains(email)) &&
                (string.IsNullOrWhiteSpace(searchTerm) ||
                 a.Account.Firstname.ToLower().Contains(searchTerm) ||
                 a.Account.Lastname.ToLower().Contains(searchTerm) ||
                 (a.Account.Firstname + " " + a.Account.Lastname).ToLower().Contains(searchTerm) ||
                 (a.Account.Lastname + " " + a.Account.Firstname).ToLower().Contains(searchTerm)),
    
            cancellationToken: ct,

            includesProperties:
            [
                a => a.Account,
            ]
        );
    
        return _mapper.Map<IEnumerable<PatientDto>>(patients);
    }

    public async Task<PatientDto> EditPatientAsync(EditPatientProfileDto editPatientProfileDto, Guid? editdById = null, CancellationToken ct = default)
    {
        var validationResult = await _editPatientProfileDtoValidator.ValidateAsync(editPatientProfileDto, ct);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        var patient = await _unitOfWork.Patients.GetWithDetailsAsync(editPatientProfileDto.Id, ct);

        if (patient == null)
        {
            throw new KeyNotFoundException($"Patient with ID '{editPatientProfileDto.Id}' was not found.");
        }
        
        var phoneExists = await _unitOfWork.Accounts.ExistsAsync(
            a => a.Id != patient.AccountId && a.PhoneNumber == editPatientProfileDto.PhoneNumber, ct);
        if (phoneExists)
        {
            throw new InvalidOperationException("Phone number is already in use by another account.");
        }
        
        var emailExists = await _unitOfWork.Accounts.ExistsAsync(
            a => a.Id != patient.AccountId && a.Email == editPatientProfileDto.Email, ct);
        if (emailExists)
        {
            throw new InvalidOperationException("Email is already in use by another account.");
        }

        _mapper.Map(editPatientProfileDto, patient);

        patient.Account.UpdatedBy = editdById ?? patient.Account.Id;
        
        await _unitOfWork.CompleteAsync(ct);
        return _mapper.Map<PatientDto>(patient);
    }

    public async Task DeletePatientAsync(Guid id, CancellationToken ct = default)
    {
        var patient = await _unitOfWork.Patients.GetWithDetailsAsync(id, ct);

        if (patient == null)
        {
            throw new KeyNotFoundException($"Patient with ID '{id}' was not found.");
        }
        
        _unitOfWork.Patients.Delete(patient);
        _unitOfWork.Accounts.Delete(patient.Account);

        await _unitOfWork.CompleteAsync(ct);
    }

    public async Task<PatientDto> GetByAccountIdAsync(Guid accountId, CancellationToken ct = default)
    {
        var patient = await _unitOfWork.Patients.GetByAccountIdAsync(accountId, ct);
        
        if (patient == null)
        {
            throw new KeyNotFoundException($"Patient with account ID '{accountId}' was not found.");
        }
        
        return _mapper.Map<PatientDto>(patient);
    }
}