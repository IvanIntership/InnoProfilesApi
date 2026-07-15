using ProfilesApi.Domain.Common;

namespace ProfilesApi.Domain.Entities;

public class Patient : SoftDeletableEntity
{
    public Guid AccountId { get; set; }
    public virtual Account Account { get; set; }
}