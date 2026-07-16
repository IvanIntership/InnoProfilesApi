using FluentValidation;
using ProfilesApi.Application.Dto.Offices;
using ProfilesApi.Application.Validation.Shared;

namespace ProfilesApi.Application.Validation.Offices;

public class EditOfficeInformationDtoValidator : AbstractValidator<EditOfficeInformationDto>
{
    public EditOfficeInformationDtoValidator()
    {
        Include(new OfficeValidator());

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id must not be empty.");
    }
}