using FluentValidation;
using ProfilesApi.Application.Dto.Patients;
using ProfilesApi.Application.Validation.Shared;

namespace ProfilesApi.Application.Validation.Patients;

public class SearchFilteredPatientListDtoValidator : AbstractValidator<SearchFilteredPatientListDto>
{
    public SearchFilteredPatientListDtoValidator()
    {
        Include(new SearchListValidator());
        
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches(@"^\+?(\d{1,3})?[-.\s]?(\(?\d{3}\)?[-.\s]?)?(\d[-.\s]?){6,9}\d$")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .When(x => !string.IsNullOrEmpty(x.Email));
    }
}
