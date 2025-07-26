using Microsoft.EntityFrameworkCore;
using PizzeriaAccounting.Application.Infrastructure.Configuration;
using PizzeriaAccounting.Domain;
using PizzeriaAccounting.WebApi.Providers;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;
using Platform.WebApi.Context;

namespace PizzeriaAccounting.WebApi.Internal;

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
        
        services.AddDbContext<PizzeriaAccountingDbContext>(options =>
        {
            options.UseNpgsql(databaseOptions.ConnectionString);
        });
        
        services.AddScoped<IDbScopeProvider, DbScopeProvider>();
    }
}