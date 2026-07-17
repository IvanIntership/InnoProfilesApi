using FluentValidation;

namespace ProfilesApi.Application.Validation.Shared;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, string?> SearchTermRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(100).WithMessage("The search query is too long.")
            .MinimumLength(2).WithMessage("You need to enter at least 2 characters to search.")
            .When(x => !string.IsNullOrEmpty(x as string));
    }

    public static IRuleBuilderOptions<T, string> PasswordRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.");
    }

    public static IRuleBuilderOptions<T, string> AddressRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Address must not be empty.")
            .MaximumLength(100).WithMessage("Address must not exceed 100 characters.");
    }

    public static IRuleBuilderOptions<T, string> FirstnameRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .MaximumLength(50);
    }

    public static IRuleBuilderOptions<T, string> LastnameRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .MaximumLength(50);
    }

    public static IRuleBuilderOptions<T, DateTime> BirthdayRules<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .LessThan(DateTime.UtcNow).WithMessage("A birthday cannot be in the future.");
    }

    public static IRuleBuilderOptions<T, string> PhoneNumberRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .Matches(ValidationConstants.PhoneNumberPattern);
    }

    public static IRuleBuilderOptions<T, string> EmailRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .EmailAddress();
    }
}