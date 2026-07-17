using FluentValidation;
using ProfilesApi.Application.Dto.Offices;
using ProfilesApi.Application.Validation.Shared;

namespace ProfilesApi.Application.Validation.Offices;

public class EditOfficeInformationDtoValidator : AbstractValidator<EditOfficeInformationDto>
{
    public EditOfficeInformationDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id must not be empty.");
        RuleFor(x => x.PhotoId)
            .NotEqual(Guid.Empty).WithMessage("Invalid photo ID format.")
            .When(x => x.PhotoId.HasValue);
        RuleFor(x => x.Address).AddressRules();
        RuleFor(x => x.PhoneNumber).PhoneNumberRules();
    }
}