using Media.Application.Infrastructure.Configuration;
using Media.Domain;
using Media.WebApi.Providers;
using Microsoft.EntityFrameworkCore;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;
using Platform.WebApi.Context;

namespace Media.WebApi.Internal;

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
        
        services.AddDbContext<MediaDbContext>(options =>
        {
            options.UseNpgsql(databaseOptions.ConnectionString);
        });
        
        services.AddScoped<IDbScopeProvider, DbScopeProvider>();
    }
}