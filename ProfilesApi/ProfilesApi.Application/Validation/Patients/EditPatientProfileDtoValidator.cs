using FluentValidation;
using ProfilesApi.Application.Dto.Patients;
using ProfilesApi.Application.Validation.Shared;

namespace ProfilesApi.Application.Validation.Patients;

public class EditPatientProfileDtoValidator : AbstractValidator<EditPatientProfileDto>
{
    public EditPatientProfileDtoValidator()
    {
        Include(new BaseProfileValidator());

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Invalid patient ID.");
    }
}