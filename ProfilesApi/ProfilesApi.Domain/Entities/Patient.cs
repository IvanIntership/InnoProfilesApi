namespace ProfilesApi.Domain.Entities;

public class Patient : BaseEntity
{
    public Guid? AccountId { get; private set; }
    public bool IsLinked => AccountId != null;
    
    public void DeactivateAccount()
    {
        AccountId = null;
    }
    public void SetAccount(Guid accountId)
    {
        AccountId = accountId;
    }
}