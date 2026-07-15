using ProfilesApi.Domain.Common;
using System.Linq.Expressions;

namespace ProfilesApi.Domain.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[]? includesProperties);
    
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[]? includesProperties);

    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    
    Task<bool> ExistsAsync(Expression<Func<T, bool>>? filter, CancellationToken cancellationToken = default);
}