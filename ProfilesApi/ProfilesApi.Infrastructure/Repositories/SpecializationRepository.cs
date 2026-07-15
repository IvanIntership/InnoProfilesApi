using Microsoft.EntityFrameworkCore;
using ProfilesApi.Domain.Entities;
using ProfilesApi.Domain.Interfaces;
using ProfilesApi.Infrastructure.Data;

namespace ProfilesApi.Infrastructure.Repositories;

public class SpecializationRepository : GenericRepository<Specialization>, ISpecializationRepository
{
    public SpecializationRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Specialization>> SearchByTerm(string name, CancellationToken cancellationToken = default)
    {
        IQueryable<Specialization> query = Entities.AsQueryable();
        return await query.Where(a => a.Name.Contains(name)).ToListAsync(cancellationToken);
    }
}