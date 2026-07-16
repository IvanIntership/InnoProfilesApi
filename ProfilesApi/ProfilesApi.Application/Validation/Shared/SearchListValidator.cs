using FluentValidation;
using ProfilesApi.Application.Interfaces;

namespace ProfilesApi.Application.Validation.Shared;

public class SearchListValidator : AbstractValidator<ISearchList>
{
    public SearchListValidator()
    {
        RuleFor(x => x.SearchTerm)
            .MaximumLength(100)
            .WithMessage("The search query is too long.")
            .When(x => !string.IsNullOrEmpty(x.SearchTerm));
        
        RuleFor(x => x.SearchTerm)
            .MinimumLength(2)
            .WithMessage("You need to enter at least 2 characters to search.")
            .When(x => !string.IsNullOrEmpty(x.SearchTerm));
    }
}