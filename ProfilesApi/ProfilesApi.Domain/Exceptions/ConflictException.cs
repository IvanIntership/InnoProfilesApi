using ProfilesApi.Domain.Common;

namespace ProfilesApi.Domain.Exceptions;

public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
}