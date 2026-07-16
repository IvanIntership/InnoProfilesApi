namespace ProfilesApi.Application.Interfaces;

public interface IBaseProfileDto
{
    string Firstname { get; }
    string Lastname { get; }
    DateTime Birthday { get; }
    string PhoneNumber { get; }
    string Email { get; }
}