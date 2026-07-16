using FluentValidation;
using ProfilesApi.Application.Dto.Administrators;
using ProfilesApi.Application.Validation.Shared;

namespace ProfilesApi.Application.Validation.Administrators;

public class EditAdministratorProfileDtoValidator : AbstractValidator<EditAdministratorProfileDto>
{
    public EditAdministratorProfileDtoValidator()
    {
        Include(new BaseProfileValidator());

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required.");
        
        RuleFor(x => x.PhotoId)
            .NotEqual(Guid.Empty).WithMessage("Invalid photo ID format.")
            .When(x => x.PhotoId.HasValue);

        RuleFor(x => x.OfficeId)
            .NotEmpty().WithMessage("Office ID is required.");

        RuleFor(x => x.CareerStartDate)
            .NotEmpty().WithMessage("Career start date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Career start date cannot be in the future.");

        RuleFor(x => x.GapInMonths)
            .GreaterThanOrEqualTo(0).WithMessage("Gap in months cannot be negative.");
    }
}