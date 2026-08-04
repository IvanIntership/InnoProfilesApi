namespace ProfilesApi.Application.Dto.Patients;

public sealed record EditPatientProfileDto
{
    public Guid Id { get; init; }
    public string Firstname { get; init; }
    public string Lastname { get; init; }
    public DateTime Birthday { get; init; }
    public string PhoneNumber { get; init; }
    public string Email { get; init; }
    public Guid? PhotoId { get; init; }
}