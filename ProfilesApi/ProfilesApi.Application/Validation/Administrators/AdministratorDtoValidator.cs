using FluentValidation;
using ProfilesApi.Application.Dto.Administrators;
using ProfilesApi.Application.Validation.Shared;

namespace ProfilesApi.Application.Validation.Administrators;

public class AdministratorDtoValidator : AbstractValidator<AdministratorDto>
{
    public AdministratorDtoValidator()
    {
        Include(new BaseProfileValidator());
        
        RuleFor(x => x.Id).NotEmpty();
        
        RuleFor(x => x.Role)
            .NotEmpty()
            .IsInEnum()
            .WithMessage("Undefined role.");
        
        RuleFor(x => x.PhotoId)
            .NotEqual(Guid.Empty).WithMessage("Invalid photo ID format.")
            .When(x => x.PhotoId.HasValue);
        
        RuleFor(x => x.OfficeId)
            .NotEmpty().WithMessage("Office ID is required.");
        
        RuleFor(x => x.TotalExperience)
            .NotEmpty()
            .GreaterThanOrEqualTo(0)
            .WithMessage("Total experience must be greater than or equal to 0.");
    }
}