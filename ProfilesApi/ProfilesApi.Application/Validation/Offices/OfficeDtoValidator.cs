using FluentValidation;
using ProfilesApi.Application.Dto.Offices;
using ProfilesApi.Application.Validation.Shared;

namespace ProfilesApi.Application.Validation.Offices;

public class OfficeDtoValidator : AbstractValidator<OfficeDto>
{
    public OfficeDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id must not be empty.");
        RuleFor(x => x.PhotoId)
            .NotEmpty().WithMessage("Invalid photo ID format.")
            .When(x => x.PhotoId.HasValue);
        RuleFor(x => x.Address).AddressRules();
        RuleFor(x => x.PhoneNumber).PhoneNumberRules();
    }
}