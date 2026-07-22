using AutoMapper;
using ProfilesApi.Application.Dto.Patients;
using ProfilesApi.Application.Interfaces;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Services;

public class PatientService : IPatientService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    
    public PatientService(IMapper mapper, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    }

    public async Task<PatientDto> CreatePatientAsync(RegisterPatientDto registerPatientDto, Guid? createdById = null, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(registerPatientDto);
        
        var emailExists = await _unitOfWork.Accounts.GetByEmail(registerPatientDto.Email, ct);
        var numberExists = await _unitOfWork.Accounts.GetByPhoneNumber(registerPatientDto.PhoneNumber, ct);

        if (emailExists != null || numberExists != null)
        {
            throw new InvalidOperationException("Account already exists");
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
        var searchTerm = filteredPatientListDto?.SearchTerm?.Trim();
        var phoneNumber = filteredPatientListDto?.PhoneNumber?.Trim();
        var email = filteredPatientListDto?.Email?.Trim();
    
        var patients = await _unitOfWork.Patients.GetAllAsync(
            filter: a => 
                (string.IsNullOrWhiteSpace(phoneNumber) || a.Account.PhoneNumber.Contains(phoneNumber)) &&
                (string.IsNullOrWhiteSpace(email) || a.Account.Email.Contains(email)) &&
                (string.IsNullOrWhiteSpace(searchTerm) ||
                 a.Account.Firstname.Contains(searchTerm) ||
                 a.Account.Lastname.Contains(searchTerm) ||
                 (a.Account.Firstname + " " + a.Account.Lastname).Contains(searchTerm) ||
                 (a.Account.Lastname + " " + a.Account.Firstname).Contains(searchTerm)),
    
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
        ArgumentNullException.ThrowIfNull(editPatientProfileDto);
        
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
        _mapper.Map(editPatientProfileDto, patient.Account);

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