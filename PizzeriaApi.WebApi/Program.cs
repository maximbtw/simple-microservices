using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NLog.Web;
using PizzeriaApi.Application;
using PizzeriaApi.Application.Infrastructure.Configuration;
using PizzeriaApi.WebApi.Internal;
using PizzeriaApi.WebApi.Internal.Swagger;
using PizzeriaApi.WebApi.Middleware;
using Platform.WebApi;
using Platform.WebApi.Middleware;
using Platform.WebApi.OpenTelemetry;
using Swashbuckle.AspNetCore.SwaggerGen;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.Sources.Clear();
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Logging.ClearProviders();

builder.Host.ConfigureServices((context, services) =>
{
    var configuration = context.Configuration.GetSection(nameof(Configuration)).Get<Configuration>()!;

    services.BindOptions(builder.Configuration);
    
    services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

    services.AddHealthChecks().AddRedis(configuration.DistributedCacheOptions.RedisSettings.Url);
    services.AddJwtAuthentication(configuration.JwtOptions);
    
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerApiOptions>();
    
    services.AddOpenTelemetryInfrastructure(configuration.OpenTelemetryOptions);
    services.RegisterServices();
    services.RegisterProviders();
    services.RegisterClients(configuration.DependenciesOptions);
    services.AddDistCache(configuration.DistributedCacheOptions);
});

builder.Host.UseNLog(new NLogAspNetCoreOptions { RemoveLoggerFactoryFilter = false });

WebApplication app = builder.Build();

app.Lifetime.ApplicationStarted.Register(() =>
{
    foreach (string url in app.Urls)
    {
        Console.WriteLine($"Host is listening on: {url}");   
    }
});

app.UseCors(x=> x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());    

app.UseExceptionHandler(
    exceptionHandlerApp =>
    {
        exceptionHandlerApp.Run(
            context => ExceptionHandler.Handle(
                context,
                exceptionHandlerApp.ApplicationServices.GetRequiredService<IOptions<JsonOptions>>().Value
            )
        );
    }
);

app.UseAuthentication();
app.UseAuthorization();  

app.MapHealthChecks("/health");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();   
}

app.MapControllers();

app.UseWhen(
    context => !PathChecker.IsExcludedPath(context),
    appBuilder =>
    {
        appBuilder.UseMiddleware<CustomTraceMiddleware>();
        appBuilder.UseMiddleware<ApiAuthorizationMiddleware>();
        appBuilder.UseMiddleware<RequestLocalizationMiddleware>();
    });

app.Run();
