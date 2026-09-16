using InnoClinic.Shared.Events;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Interfaces;

public interface IRegistrationPublisher
{
    Task PublishCreatedAsync(Account account, string password, Roles role, CancellationToken ct);
}