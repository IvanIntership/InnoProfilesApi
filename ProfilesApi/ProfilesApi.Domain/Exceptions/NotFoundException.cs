using ProfilesApi.Domain.Common;

namespace ProfilesApi.Domain.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}