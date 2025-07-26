using Catalog.Application.Infrastructure.Configuration;
using Catalog.Application.Persistence.Ingredient;
using Catalog.Application.Persistence.Pizza;
using Catalog.Application.Services.Ingredient;
using Catalog.Application.Services.Pizza;
using Catalog.Contracts.Ingredient;
using Catalog.Contracts.Persistence.Ingredient;
using Catalog.Contracts.Persistence.Pizza;
using Catalog.Contracts.Pizza;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Platform.WebApi;

namespace Catalog.Application;

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
        services.AddTransient<IIngredientRepository, IngredientRepository>();
        services.AddTransient<IPizzaRepository, PizzaRepository>();
        
        services.AddTransient<IIngredientService, IngredientService>();
        services.AddTransient<IPizzaService, PizzaService>();
    }
    
    public static void RegisterClients(this IServiceCollection services, DependenciesOptions dependenciesOptions)
    {
        services.AddTransient<CustomHeaderHttpMessageHandler>();
        
    }
}