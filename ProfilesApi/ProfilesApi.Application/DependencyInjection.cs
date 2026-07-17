using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProfilesApi.Application.Dto.Accounts;
using ProfilesApi.Application.Mappings;

namespace ProfilesApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<AccountDto>();
        services.AddAutoMapper(typeof(AccountMappingProfile).Assembly);
        
        return services;
    }
}