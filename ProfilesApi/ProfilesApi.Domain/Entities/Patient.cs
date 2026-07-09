using ProfilesApi.Domain.Common;

namespace ProfilesApi.Domain.Entities;

public class Patient : BaseEntity
{
    public Guid? AccountId { get; set; }
    public bool IsLinked => AccountId != null;
}