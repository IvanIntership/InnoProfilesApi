namespace ProfilesApi.Application.Dto.Patients;

public record EditPatientProfileDto(
    Guid Id,
    string Firstname,
    string Lastname,
    string PhoneNumber,
    string Email,
    Guid? PhotoId);