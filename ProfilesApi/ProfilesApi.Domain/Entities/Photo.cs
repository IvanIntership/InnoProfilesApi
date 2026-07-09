namespace ProfilesApi.Domain.Entities;

public class Photo : BaseEntity
{
    public string? Url { get; private set; }
    
    public Photo(string? url)
    {
        Url = url;
    }
}