using ProfilesApi.Application;
using ProfilesApi.Infrastructure;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ProfilesApi.API.Middleware;
using Serilog;
using Serilog.Events;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("LuckyPennySoftware.AutoMapper", LogEventLevel.Error)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Serilog.AspNetCore.RequestLoggingMiddleware", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore.HttpsPolicy", LogEventLevel.Error)
    .WriteTo.Console()
    .WriteTo.MongoDB(builder.Configuration.GetConnectionString("MongoLogging") 
                     ?? throw new InvalidOperationException("Mongodb connection string is missing in configuration"))
    .CreateLogger();
builder.Host.UseSerilog();

try
{
    Log.Information("Starting web host...");
    builder.Configuration.AddEnvironmentVariables();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication();
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddOpenApi();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    var app = builder.Build();

    app.UseExceptionHandler();
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();
    app.UseSerilogRequestLogging();
    app.UseStaticFiles();
    app.MapControllers();

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ProfilesApi.Infrastructure.Data.AppDbContext>();
            context.Database.Migrate(); 
            Log.Information("Database migrated successfully.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while migrating the database.");
        }
    }
    
    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}