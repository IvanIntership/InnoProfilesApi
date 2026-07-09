namespace ProfilesApi.Domain.Entities;

public class Specialization : SoftDeletableEntity
{
    public string Name { get; private set; }

    public Specialization(string name)
    {
        if(String.IsNullOrWhiteSpace(name)) 
            throw new ArgumentException("Specialization name is empty", nameof(name));
        
        Name = name;
    }
    
    public void ChangeName(string name)
    {
        if(String.IsNullOrWhiteSpace(name)) 
            throw new ArgumentException("Specialization name is empty", nameof(name));
        
        Name = name;
    }
}