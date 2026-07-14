namespace ProfilesApi.Domain.Interfaces;

public interface IAuditable
{
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; set; }
}