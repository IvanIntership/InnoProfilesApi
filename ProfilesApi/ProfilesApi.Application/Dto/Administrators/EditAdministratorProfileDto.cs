namespace ProfilesApi.Application.Dto.Administrators;

public sealed record EditAdministratorProfileDto
{
    public Guid Id { get; init; }
    public string Firstname { get; init; }
    public string Lastname { get; init; }
    public DateTime Birthday { get; init; }
    public string PhoneNumber { get; init; }
    public string Email { get; init; }
    public Guid? PhotoId { get; init; }
    public Guid OfficeId { get; init; }
    public DateTime CareerStartDate { get; init; }
    public int GapInMonths { get; init; }
}