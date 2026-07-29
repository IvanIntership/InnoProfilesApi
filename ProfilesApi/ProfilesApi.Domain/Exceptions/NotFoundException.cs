using ProfilesApi.Domain.Common;

namespace ProfilesApi.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}