using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Domain.Interfaces;

public interface ISpecializationRepository : IGenericRepository<Specialization>
{
    Task<IEnumerable<Specialization>> SearchByTerm(string name, CancellationToken cancellationToken = default);
}