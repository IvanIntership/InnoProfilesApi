using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProfilesApi.Application.Dto.Accounts;

namespace ProfilesApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<AccountDto>();
        
        return services;
    }
}