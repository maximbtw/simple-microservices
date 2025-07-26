using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Platform.WebApi.OpenTelemetry;

public static class DependencyInjectionExtensions
{
    public static void AddOpenTelemetryInfrastructure(this IServiceCollection services, OpenTelemetryOptions options)
    {
        if (options.TracingEnabled)
        {
            services.AddOpenTelemetry()
                .ConfigureResource(resource => resource.AddService(options.Subsystem))
                .WithTracing(tracing => tracing
                    .AddAspNetCoreInstrumentation(op => op.Filter = context => !PathChecker.IsExcludedPath(context))
                    .AddHttpClientInstrumentation()
                    .AddSqlClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddQuartzInstrumentation()
                    .AddNpgsql()
                    .AddOtlpExporter(x =>
                    {
                        x.Endpoint = new Uri(options.Url);
                        x.ExportProcessorType = ExportProcessorType.Batch;
                        x.Protocol = OtlpExportProtocol.Grpc;
                    }));
        }
    }
}