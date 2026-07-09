using ProfilesApi.Domain.Common;

namespace ProfilesApi.Domain.Entities;

public class Specialization : SoftDeletableEntity
{
    public string Name { get; set; }

    public Specialization(string name)
    {
        Name = name;
    }
}