using Catalog.Client;
using Catalog.Contracts.Ingredient;
using Catalog.Contracts.Pizza;
using Media.Client;
using Media.Contracts.Image;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PizzeriaAccounting.Client;
using PizzeriaAccounting.Contracts.Account;
using PizzeriaAccounting.Contracts.AccountUser;
using PizzeriaApi.Application.Infrastructure.Configuration;
using PizzeriaApi.Application.Providers;
using PizzeriaApi.Application.Services;
using PizzeriaApi.Contracts.AccountApi;
using PizzeriaApi.Contracts.AuthApi;
using PizzeriaApi.Contracts.IngredientApi;
using PizzeriaApi.Contracts.PizzaApi;
using PizzeriaApi.Contracts.Providers;
using Platform.Core.DistributedCache.Configuration;
using Platform.WebApi;
using System.Net;

namespace PizzeriaApi.Application;

public static class DependencyInjectionExtensions
{
    public static void BindOptions(this IServiceCollection services, ConfigurationManager configuration)
    {
        services
            .AddOptions<JwtOptions>()
            .Bind(configuration.GetSection("Configuration:JwtOptions"))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services
            .AddOptions<DependenciesOptions>()
            .Bind(configuration.GetSection("Configuration:DependenciesOptions"))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services
            .AddOptions<DistributedCacheOptions>()
            .Bind(configuration.GetSection("Configuration:DistributedCacheOptions"))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }

    public static void AddDistCache(this IServiceCollection services, DistributedCacheOptions options)
    {
        services.AddStackExchangeRedisCache(
            cacheOptions =>
            {
                cacheOptions.Configuration = options.RedisSettings.Url;
                cacheOptions.InstanceName = "dist-cache";
            }
        );
    }
    
    public static void RegisterProviders(this IServiceCollection services)
    {
        services.AddTransient<IAccountUserProvider, AccountUserProvider>();
    }
    
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<IApiPizzaService, ApiPizzaService>();
        services.AddTransient<IApiIngredientService, ApiIngredientService>();
        services.AddTransient<IApiAuthService, ApiAuthService>();
        services.AddTransient<IApiAccountService, ApiAccountService>();
    }

    public static void RegisterClients(this IServiceCollection services, DependenciesOptions dependenciesOptions)
    {
        services.AddTransient<CustomHeaderHttpMessageHandler>();
        
        services.RegisterPizzeriaAccountingServices(dependenciesOptions);
        services.RegisterCatalogServices(dependenciesOptions);
        services.RegisterMediaServices(dependenciesOptions);
    }

    private static void RegisterPizzeriaAccountingServices(
        this IServiceCollection services,
        DependenciesOptions dependenciesOptions)
    {
        services.AddHttpClient(dependenciesOptions.PizzeriaAccountingUrl,
                (provider, client) =>
                {
                    DependenciesOptions configuration = provider.GetRequiredService<IOptions<DependenciesOptions>>().Value;

                    client.BaseAddress = new Uri(configuration.PizzeriaAccountingUrl);
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

        services.AddTransient<IAccountService>(provider =>
        {
            var optionMonitor = provider.GetRequiredService<IOptionsMonitor<DependenciesOptions>>();

            return new AccountServiceHttpClient(provider.GetRequiredService<IHttpClientFactory>()
                .CreateClient(optionMonitor.CurrentValue.PizzeriaAccountingUrl));
        });

        services.AddTransient<IUserService>(provider =>
        {
            var optionMonitor = provider.GetRequiredService<IOptionsMonitor<DependenciesOptions>>();

            return new UserServiceHttpClient(provider.GetRequiredService<IHttpClientFactory>()
                .CreateClient(optionMonitor.CurrentValue.PizzeriaAccountingUrl));
        });
    }
    
    private static void RegisterCatalogServices(
        this IServiceCollection services,
        DependenciesOptions dependenciesOptions)
    {
        services.AddHttpClient(dependenciesOptions.CatalogUrl,
                (provider, client) =>
                {
                    DependenciesOptions configuration =
                        provider.GetRequiredService<IOptions<DependenciesOptions>>().Value;

                    client.BaseAddress = new Uri(configuration.CatalogUrl);
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

        services.AddTransient<IIngredientService>(provider =>
        {
            var optionMonitor = provider.GetRequiredService<IOptionsMonitor<DependenciesOptions>>();

            return new IngredientServiceHttpClient(provider.GetRequiredService<IHttpClientFactory>()
                .CreateClient(optionMonitor.CurrentValue.CatalogUrl));
        });
        
        services.AddTransient<IPizzaService>(provider =>
        {
            var optionMonitor = provider.GetRequiredService<IOptionsMonitor<DependenciesOptions>>();

            return new PizzaServiceHttpClient(provider.GetRequiredService<IHttpClientFactory>()
                .CreateClient(optionMonitor.CurrentValue.CatalogUrl));
        });
    }
    
    private static void RegisterMediaServices(
        this IServiceCollection services,
        DependenciesOptions dependenciesOptions)
    {
        services.AddHttpClient(dependenciesOptions.MediaUrl,
                (provider, client) =>
                {
                    DependenciesOptions configuration =
                        provider.GetRequiredService<IOptions<DependenciesOptions>>().Value;

                    client.BaseAddress = new Uri(configuration.MediaUrl);
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

        services.AddTransient<IImageService>(provider =>
        {
            var optionMonitor = provider.GetRequiredService<IOptionsMonitor<DependenciesOptions>>();

            return new ImageServiceHttpClient(provider.GetRequiredService<IHttpClientFactory>()
                .CreateClient(optionMonitor.CurrentValue.MediaUrl));
        });
    }
}