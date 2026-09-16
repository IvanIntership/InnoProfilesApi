using InnoClinic.Shared.Events;
using MassTransit;
using ProfilesApi.Application.Interfaces;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Publishers;

public sealed class StaffCreatedPublisher : IRegistrationPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    public StaffCreatedPublisher(IPublishEndpoint publishEndpoint) => _publishEndpoint = publishEndpoint;

    public Task PublishCreatedAsync(Account account, string password, Roles role, CancellationToken ct)
    {
        return _publishEndpoint.Publish<IStaffCreatedEvent>(new
        {
            AccountId = account.Id,
            Email = account.Email,
            Password = password,
            Firstname = account.Firstname,
            Lastname = account.Lastname,
            Role = role
        }, ct);
    }
}