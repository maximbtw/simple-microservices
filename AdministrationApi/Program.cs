using System.Text;
using System.Text.Json.Serialization;
using AdministrationApi.Configuration;
using AdministrationApi.Internal;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Platform.Client.Http;
using Platform.WebApi.Middleware;
using Yarp.ReverseProxy.Transforms;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.Sources.Clear();
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Host.ConfigureServices((context, services) =>
{
    var configuration = context.Configuration.GetSection(nameof(Configuration)).Get<Configuration>()!;
    
    services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
    
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
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.JwtOptions.JwtSecretKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true
            };
        });
    
    services.AddReverseProxy()
        .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
        .AddTransforms(builderContext =>
        {
            builderContext.AddRequestTransform(async transformContext =>
            {
                HttpContext httpContext = transformContext.HttpContext;

                string internalToken = TokenParser.CreateInternalTokenFromClaims(httpContext.User);
                if (!string.IsNullOrEmpty(internalToken))
                {
                    transformContext.ProxyRequest.Headers.Add(HeaderConstants.HeaderUser, internalToken);
                }

                await Task.CompletedTask;
            });
        });
});

WebApplication app = builder.Build();

app.MapReverseProxy();

app.UseAuthentication();
app.UseAuthorization();  

app.UseMiddleware<CustomTraceMiddleware>();

app.Run();