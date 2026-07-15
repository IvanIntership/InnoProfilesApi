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
        if (string.IsNullOrWhiteSpace(name)) return new List<Specialization>();

        name = name.Trim();
        IQueryable<Specialization> query = Entities.AsQueryable();
        query = query.Where(a => a.Name.Contains(name));
        return await query.ToListAsync(cancellationToken);
    }
}