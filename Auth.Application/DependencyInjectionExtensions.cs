using Auth.Application.Infrastructure.Configuration;
using Auth.Application.Persistence.User;
using Auth.Application.Services.Auth;
using Auth.Contracts.Auth;
using Auth.Contracts.Persistence.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Application;

public static class DependencyInjectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.BindOptions(configuration);
        services.RegisterRepositories();
        services.RegisterServices();
    }
    
    private static void BindOptions(this IServiceCollection services, ConfigurationManager configuration)
    {
        services
            .AddOptions<JwtOptions>()
            .Bind(configuration.GetSection("Configuration:JwtOptions"))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services
            .AddOptions<DatabaseOptions>()
            .Bind(configuration.GetSection("Configuration:DatabaseOptions"))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
    
    private static void RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<IAuthService, AuthService>();
    }
    
    private static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddTransient<IUserRepository, UserRepository>();
    }
}