namespace ProfilesApi.Application.Dto.Offices;

public record EditOfficeInformationDto
{
    public Guid Id { get; init; }
    public string Address { get; init; }
    public string PhoneNumber { get; init; }
    public Guid? PhotoId { get; init; }
}