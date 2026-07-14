using ProfilesApi.Domain.Common;

namespace ProfilesApi.Domain.Entities;

public class Specialization : BaseEntity
{
    public string Name { get; set; }

    public Specialization(string name)
    {
        Name = name;
    }
}