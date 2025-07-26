using System.Net;
using Auth.Client;
using Auth.Contracts.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PizzeriaAccounting.Application.Infrastructure.Configuration;
using PizzeriaAccounting.Application.Persistence.Account;
using PizzeriaAccounting.Application.Persistence.AccountUser;
using PizzeriaAccounting.Application.Services.Account;
using PizzeriaAccounting.Application.Services.User;
using PizzeriaAccounting.Contracts.Account;
using PizzeriaAccounting.Contracts.AccountUser;
using PizzeriaAccounting.Contracts.Persistence.Account;
using PizzeriaAccounting.Contracts.Persistence.AccountUser;
using Platform.WebApi;

namespace PizzeriaAccounting.Application;

public static class DependencyInjectionExtensions
{
    public static void BindOptions(this IServiceCollection services, ConfigurationManager configuration)
    {
        services
            .AddOptions<DatabaseOptions>()
            .Bind(configuration.GetSection("Configuration:DatabaseOptions"))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services
            .AddOptions<DependenciesOptions>()
            .Bind(configuration.GetSection("Configuration:DependenciesOptions"))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
    
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<IAccountRepository, AccountRepository>();
        services.AddTransient<IAccountUserRepository, AccountUserRepository>();

        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<IUserService, UserService>();
    }
    
    public static void RegisterClients(this IServiceCollection services, DependenciesOptions dependenciesOptions)
    {
        services.AddTransient<CustomHeaderHttpMessageHandler>();
        
        services.AddAuthService(dependenciesOptions);
    }

    private static void AddAuthService(this IServiceCollection services, DependenciesOptions dependenciesOptions)
    {
        services.AddHttpClient(dependenciesOptions.AuthUrl,
                (provider, client) =>
                {
                    Configuration configuration = provider.GetRequiredService<IOptions<Configuration>>().Value;

                    client.BaseAddress = new Uri(configuration.DependenciesOptions.AuthUrl);
                    client.DefaultRequestVersion = HttpVersion.Version20;
                    client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher;
                })
            .AddHttpMessageHandler<CustomHeaderHttpMessageHandler>()
            .ConfigurePrimaryHttpMessageHandler(() =>
                new SocketsHttpHandler
                {
                    MaxConnectionsPerServer = 100,
                    PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2),
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                });
        
        services.AddTransient<IAuthService>(provider =>
        {
            var optionMonitor = provider.GetRequiredService<IOptionsMonitor<Configuration>>();

            return new AuthServiceHttpClient(provider.GetRequiredService<IHttpClientFactory>()
                .CreateClient(optionMonitor.CurrentValue.DependenciesOptions.AuthUrl));
        });
    }
}