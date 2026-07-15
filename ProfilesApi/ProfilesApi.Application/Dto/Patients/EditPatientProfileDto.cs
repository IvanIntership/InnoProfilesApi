namespace ProfilesApi.Application.Dto.Patients;

public record EditPatientProfileDto(string Firstname,
    string Lastname,
    string PhoneNumber,
    string Email,
    Guid? PhotoId);