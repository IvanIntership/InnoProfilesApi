namespace ProfilesApi.Application.Dto.Administrators;

public record EditAdministratorProfileDto(
    Guid AdministratorId,
    string Firstname,
    string Lastname,
    DateTime Birthday,
    string PhoneNumber,
    string Email,
    Guid? PhotoId,
    Guid OfficeId,
    DateTime CareerStartDate,
    int GapInMonths
);