using FluentValidation;
using ProfilesApi.Application.Interfaces;

namespace ProfilesApi.Application.Validation.Shared;

public class BaseProfileValidator : AbstractValidator<IBaseProfileDto>
{
    public BaseProfileValidator()
    {
        RuleFor(x => x.Firstname)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(x => x.Lastname)
            .NotEmpty()
            .MaximumLength(50);
        
        RuleFor(x => x.Birthday)
            .NotEmpty()
            .LessThan(DateTime.UtcNow)
            .WithMessage("A birthday cannot be in the future.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches(@"^\+?(\d{1,3})?[-.\s]?(\(?\d{3}\)?[-.\s]?)?(\d[-.\s]?){6,9}\d$");
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}