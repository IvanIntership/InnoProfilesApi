using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Domain.Interfaces;

public interface IOfficeRepository : IGenericRepository<Office>
{
    Task<Office?> GetWithDetailsAsync(Guid officeId, CancellationToken cancellationToken = default); //remove
}