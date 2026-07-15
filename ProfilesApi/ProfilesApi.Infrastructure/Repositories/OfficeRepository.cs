using Microsoft.EntityFrameworkCore;
using ProfilesApi.Domain.Entities;
using ProfilesApi.Domain.Interfaces;
using ProfilesApi.Infrastructure.Data;

namespace ProfilesApi.Infrastructure.Repositories;

public class OfficeRepository : GenericRepository<Office>, IOfficeRepository
{
    public OfficeRepository(AppDbContext context) : base(context)
    {
    }
}