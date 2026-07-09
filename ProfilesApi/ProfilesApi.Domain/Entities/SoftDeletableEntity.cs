namespace ProfilesApi.Domain.Entities;

public abstract class SoftDeletableEntity : BaseEntity
{
    public bool IsActive { get; private set; } =  true;
    public void Deactivate()
    {
        IsActive = false;
    }
    public void Activate()
    {
        IsActive = true;
    }
}