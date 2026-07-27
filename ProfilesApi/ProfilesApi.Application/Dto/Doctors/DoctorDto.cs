using ProfilesApi.Domain.Enums;

namespace ProfilesApi.Application.Dto.Doctors;

public record DoctorDto
{
    public Guid Id { get; init; }
    public Guid AccountId { get; init; }
    public string Firstname { get; init; }
    public string Lastname { get; init; }
    public DateTime Birthday { get; init; }
    public string PhoneNumber { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public string Degree { get; init; }
    public Roles Role { get; init; }
    public Guid OfficeId { get; init; }
    public Guid SpecializationId { get; init; }
    public Guid? PhotoId { get; init; }
    public string? PhotoUrl { get; init; }
    public DateTime CareerStartDate { get; init; }
    public int GapInMonths { get; init; }
}