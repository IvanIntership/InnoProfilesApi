namespace ProfilesApi.Application.Dto.Patients;

public record EditPatientProfileDto(
    Guid PatientId,
    string Firstname,
    string Lastname,
    string PhoneNumber,
    string Email,
    Guid? PhotoId);