using ProfilesApi.Domain.Common;

namespace ProfilesApi.Domain.Exceptions;

public class NotFoundException : BaseApplicationException
{
    public NotFoundException(string message) : base(message) { }
}