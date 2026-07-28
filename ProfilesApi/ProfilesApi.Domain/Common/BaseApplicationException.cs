namespace ProfilesApi.Domain.Common;

public abstract class BaseApplicationException : Exception
{
    protected BaseApplicationException(string message) : base(message) { }
}