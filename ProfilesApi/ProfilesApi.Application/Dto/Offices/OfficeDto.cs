namespace ProfilesApi.Application.Dto.Offices;

public record OfficeDto
{
    public Guid Id { get; init; }
    public string Address { get; init; }
    public string PhoneNumber { get; init; }
    public Guid? PhotoId { get; init; }
}