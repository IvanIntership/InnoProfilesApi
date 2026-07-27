namespace ProfilesApi.Application.Dto.Offices;

public record CreateOfficeDto
{
    public string Address { get; init; }
    public string PhoneNumber { get; init; }
    public Guid? PhotoId { get; init; }
}