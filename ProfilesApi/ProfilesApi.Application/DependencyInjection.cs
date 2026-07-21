using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProfilesApi.Application.Dto.Accounts;
using ProfilesApi.Application.Interfaces;
using ProfilesApi.Application.Mappings;
using ProfilesApi.Application.Services;

namespace ProfilesApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<AccountDto>();
        services.AddAutoMapper(typeof(AccountMappingProfile).Assembly);
        
        services.AddScoped<IAdministratorService, AdministratorService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IOfficeService, OfficeService>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IPhotoService, PhotoService>();
        services.AddScoped<ISpecializationService, SpecializationService>();
        
        return services;
    }
}