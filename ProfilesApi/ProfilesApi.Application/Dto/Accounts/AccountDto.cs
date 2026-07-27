using ProfilesApi.Domain.Enums;

namespace ProfilesApi.Application.Dto.Accounts;

public record AccountDto
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
}