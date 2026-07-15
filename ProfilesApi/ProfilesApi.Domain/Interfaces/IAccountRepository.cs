using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Domain.Interfaces;

public interface IAccountRepository : IGenericRepository<Account>
{
    Task<IEnumerable<Account>> SearchByTerm(string name, CancellationToken cancellationToken = default);
    Task<Account?> GetWithDetailsAsync(Guid accountId, CancellationToken cancellationToken = default);
    Task<Account?> GetByEmail(string email, CancellationToken cancellationToken = default);
    Task<Account?> GetByPhoneNumber(string phoneNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<Account>> GetByName(string name, CancellationToken cancellationToken = default);
}