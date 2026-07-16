namespace ProfilesApi.Application.Interfaces;

public interface IOfficeDto
{
    string Address { get; }
    string PhoneNumber { get; }
    Guid? PhotoId { get; }
}