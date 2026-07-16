using FluentValidation;
using ProfilesApi.Application.Interfaces;

namespace ProfilesApi.Application.Validation.Shared;

public class OfficeValidator : AbstractValidator<IOfficeDto>
{
    public OfficeValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches(@"^\+?(\d{1,3})?[-.\s]?(\(?\d{3}\)?[-.\s]?)?(\d[-.\s]?){6,9}\d$");
        
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address must not be empty.")
            .MaximumLength(100).WithMessage("Address must not exceed 100 characters.");
        
        RuleFor(x => x.PhotoId)
            .NotEqual(Guid.Empty).WithMessage("Invalid photo ID format.")
            .When(x => x.PhotoId.HasValue);
    }
}