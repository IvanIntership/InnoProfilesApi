using FluentValidation;
using ProfilesApi.Application.Dto.Administrators;
using ProfilesApi.Application.Validation.Shared;

namespace ProfilesApi.Application.Validation.Administrators;

public class SearchFilteredAdministratorValidator : AbstractValidator<SearchFilteredAdministratorListDto>
{
    public SearchFilteredAdministratorValidator()
    {
        RuleFor(x => x.OfficeId)
            .NotEmpty().WithMessage("Invalid photo ID format.")
            .When(x => x.OfficeId.HasValue);

        RuleFor(x => x.SearchTerm).SearchTermRules();
    }
}