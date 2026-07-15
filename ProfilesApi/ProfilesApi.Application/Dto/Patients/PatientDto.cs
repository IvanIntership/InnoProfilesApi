using ProfilesApi.Application.Dto.Accounts;

namespace ProfilesApi.Application.Dto.Patients;

public record PatientDto(Guid Id, AccountDto Account);