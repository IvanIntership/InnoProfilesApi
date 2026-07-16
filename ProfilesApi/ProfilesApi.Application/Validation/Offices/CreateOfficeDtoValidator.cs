using FluentValidation;
using ProfilesApi.Application.Dto.Offices;
using ProfilesApi.Application.Validation.Shared;

namespace ProfilesApi.Application.Validation.Offices;

public class CreateOfficeDtoValidator : AbstractValidator<CreateOfficeDto>
{
    public CreateOfficeDtoValidator()
    {
        Include(new OfficeValidator());
    }
}