using FluentValidation;
using ProfilesApi.Application.Interfaces;

namespace ProfilesApi.Application.Validation.Shared;

public class PasswordValidator : AbstractValidator<IWithPassword>
{
    public PasswordValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.");
    }
}