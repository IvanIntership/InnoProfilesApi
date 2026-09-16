using InnoClinic.Shared.Events;
using ProfilesApi.Application.Interfaces;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Publishers;

public sealed class NullRegistrationPublisher : IRegistrationPublisher
{
    public Task PublishCreatedAsync(Account account, string password, Roles role, CancellationToken ct) => Task.CompletedTask;
}