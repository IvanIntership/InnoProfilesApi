using Microsoft.EntityFrameworkCore;
using ProfilesApi.Domain.Entities;
using ProfilesApi.Domain.Interfaces;
using ProfilesApi.Infrastructure.Data;

namespace ProfilesApi.Infrastructure.Repositories;

public class AdministratorRepository : GenericRepository<Administrator>, IAdministratorRepository
{
    public AdministratorRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Administrator?> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        return await Entities
            .Include(a => a.Account)
            .FirstOrDefaultAsync(d => d.AccountId == accountId, cancellationToken);
    }
}