namespace ProfilesApi.Application.Dto.Doctors;

public record DoctorDto(
    string Firstname,
    string Lastname,
    DateTime Birthday,
    string PhoneNumber,
    string Email,
    string Password,
    string Degree,
    Guid OfficeId,
    Guid SpecializationId,
    DateTime CareerStartDate,
    int GapInMonths);