using ProfilesApi.Domain.Enums;

namespace ProfilesApi.Application.Dto.Administrators;

public record AdministratorDto
{
    public Guid Id { get; init; }
    public string Firstname { get; init; }
    public string Lastname { get; init; }
    public DateTime Birthday { get; init; }
    public string PhoneNumber { get; init; }
    public string Email { get; init; }
    public Roles Role { get; init; }
    public string? PhotoUrl { get; init; }
    public Guid? PhotoId { get; init; }
    public Guid OfficeId { get; init; }
    public int TotalExperience { get; init; }
}