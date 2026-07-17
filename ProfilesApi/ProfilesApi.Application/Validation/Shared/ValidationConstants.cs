namespace ProfilesApi.Application.Validation.Shared;

public static class ValidationConstants
{
    public const string PhoneNumberPattern = @"^\+?(\d{1,3})?[-.\s]?(\(?\d{3}\)?[-.\s]?)?(\d[-.\s]?){6,9}\d$";
}