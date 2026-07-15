using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProfilesApi.Domain.Common;
using ProfilesApi.Domain.Interfaces;
using ProfilesApi.Infrastructure.Data;

namespace ProfilesApi.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext Context;
    protected readonly DbSet<T> Entities;

    public GenericRepository(AppDbContext context)
    {
        Context = context;
        Entities = Context.Set<T>();
    }
    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[]? includesProperties)
    {
        IQueryable<T> query = Entities.AsQueryable();
        if (includesProperties != null)
        {
            foreach (var includeProperty in includesProperties)
            {
                query = query.Include(includeProperty);
            }
        }
        return await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[]? includesProperties)
    {
        IQueryable<T> query = Entities.AsQueryable();
        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includesProperties != null)
        {
            foreach (var includeProperty in includesProperties)
            {
                query = query.Include(includeProperty);
            }
        }
        return await query.ToListAsync(cancellationToken);
    }

    public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        Entities.Remove(entity);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        Entities.Update(entity);
        return Task.CompletedTask;
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await Context.AddAsync(entity, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>>? filter, CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = Entities.AsQueryable();
        if (filter != null)
        {
            query = query.Where(filter);
        }
        return await query.AnyAsync(cancellationToken);
    }
}