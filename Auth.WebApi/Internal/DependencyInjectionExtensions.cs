using System.Text;
using Auth.Application.Infrastructure.Configuration;
using Auth.Contracts;
using Auth.Domain;
using Auth.WebApi.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;
using Platform.WebApi.Context;

namespace Auth.WebApi.Internal;

internal static class DependencyInjectionExtensions
{
    public static void AddJwtAuthentication(this IServiceCollection services, JwtOptions jwtOptions)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.JwtSecretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };
            });
    }

    public static void RegisterProviders(this IServiceCollection services)
    {
        services.AddTransient<ITokenProvider, TokenProvider>();
        services.AddScoped<IOperationContextProvider, OperationContextProvider>();
    }
    
    public static void AddDbInfrastructure(this IServiceCollection services, DatabaseOptions databaseOptions)
    {
        services.AddSingleton<IAsyncLockProvider, ReaderWriterLockProvider>();
        services.AddScoped<IDbEventObserver, DbEventObserver>();
        
        services.AddDbContext<AuthDbContext>(options =>
        {
            options.UseNpgsql(databaseOptions.ConnectionString);
        });
        
        services.AddScoped<IDbScopeProvider, DbScopeProvider>();
    }
}