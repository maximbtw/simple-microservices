using Catalog.Application.Infrastructure.Configuration;
using Catalog.Domain;
using Catalog.WebApi.Providers;
using Microsoft.EntityFrameworkCore;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;
using Platform.WebApi.Context;

namespace Catalog.WebApi.Internal;

internal static class DependencyInjectionExtensions
{
    public static void RegisterProviders(this IServiceCollection services)
    {
        services.AddScoped<IOperationContextProvider, OperationContextProvider>();
    }
    
    public static void AddDbInfrastructure(this IServiceCollection services, DatabaseOptions databaseOptions)
    {
        services.AddSingleton<IAsyncLockProvider, ReaderWriterLockProvider>();
        services.AddScoped<IDbEventObserver, DbEventObserver>();
        
        services.AddDbContext<CatalogDbContext>(options =>
        {
            options.UseNpgsql(databaseOptions.ConnectionString);
        });
        
        services.AddScoped<IDbScopeProvider, DbScopeProvider>();
    }
}