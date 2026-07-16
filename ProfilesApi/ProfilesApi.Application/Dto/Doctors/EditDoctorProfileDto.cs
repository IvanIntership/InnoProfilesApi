namespace ProfilesApi.Application.Dto.Doctors;

public record EditDoctorProfileDto(
    Guid Id,
    string Firstname,
    string Lastname,
    DateTime Birthday,
    string PhoneNumber,
    string Email,
    Guid? PhotoId,
    Guid? SpecializationId,
    Guid OfficeId,
    DateTime CareerStartDate,
    int GapInMonths,
    string Degree);