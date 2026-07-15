using Microsoft.EntityFrameworkCore;
using ProfilesApi.Domain.Entities;
using ProfilesApi.Domain.Interfaces;
using ProfilesApi.Infrastructure.Data;

namespace ProfilesApi.Infrastructure.Repositories;

public class PatientRepository : GenericRepository<Patient>, IPatientRepository
{
    public PatientRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Patient?> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        return await Context.Patients
            .Include(p => p.Account)
            .FirstOrDefaultAsync(p => p.AccountId == accountId, cancellationToken);
    }
}