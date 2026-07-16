using FluentValidation;
using ProfilesApi.Application.Dto.Doctors;
using ProfilesApi.Application.Validation.Shared;

namespace ProfilesApi.Application.Validation.Doctors;

public class DoctorDtoValidator : AbstractValidator<DoctorDto>
{
    public DoctorDtoValidator()
    {
        Include(new BaseProfileValidator());
        Include(new PasswordValidator());
        
        RuleFor(x => x.Id).NotEmpty();
        

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

        RuleFor(x => x.SpecializationId)
            .NotEmpty().WithMessage("Specialization is required.");
        
        RuleFor(x => x.Degree)
            .NotEmpty().WithMessage("Degree is required.")
            .MaximumLength(50).WithMessage("Degree cannot be longer than 50 characters.");
    }
}
