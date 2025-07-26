using System.Text.Json.Serialization;
using Catalog.Application;
using Catalog.Application.Infrastructure.Configuration;
using Catalog.Application.Services;
using Catalog.WebApi.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NLog.Web;
using Platform.WebApi;
using Platform.WebApi.Middleware;
using Platform.WebApi.Middleware.OperationLogging;
using Platform.WebApi.NLog;
using Platform.WebApi.OpenTelemetry;
using Platform.WebApi.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Configuration>(builder.Configuration.GetSection(nameof(Configuration)));

builder.Configuration.Sources.Clear();
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Logging.ClearProviders();

builder.Host.ConfigureServices((context, services) =>
{
    var configuration = context.Configuration.GetSection(nameof(Configuration)).Get<Configuration>()!;
    
    
    LoggingExtenstion.AddLogEnvironment(configuration.LogOptions);

    services.BindOptions(builder.Configuration);
    
    services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
    
    services.AddHealthChecks().AddNpgSql(configuration.DatabaseOptions.ConnectionString);
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGlobalOptions>();

    services.AddOpenTelemetryInfrastructure(configuration.OpenTelemetryOptions);
    services.RegisterProviders();
    services.AddDbInfrastructure(configuration.DatabaseOptions);
    services.RegisterClients(configuration.DependenciesOptions);
    services.RegisterServices();
    
    services.AddHostedService<StartupHostedService>();
});

builder.Host.UseNLog(new NLogAspNetCoreOptions { RemoveLoggerFactoryFilter = false });

WebApplication app = builder.Build();

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
        appBuilder.UseMiddleware<CustomAuthorizationMiddleware>();
        appBuilder.UseMiddleware<OperationContextMiddleware>();
        appBuilder.UseMiddleware<OperationLoggingMiddleware>();
    });

app.Run();