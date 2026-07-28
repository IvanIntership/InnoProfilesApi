using ProfilesApi.Domain.Common;

namespace ProfilesApi.Domain.Exceptions;

public class ConflictException : BaseApplicationException
{
    public ConflictException(string message) : base(message) { }
}