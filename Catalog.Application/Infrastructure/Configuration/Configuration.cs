using Platform.WebApi.NLog.Configuration;
using Platform.WebApi.OpenTelemetry;

namespace Catalog.Application.Infrastructure.Configuration;

public class Configuration
{
    public DatabaseOptions DatabaseOptions { get; set; } = new();
    
    public DependenciesOptions DependenciesOptions { get; set; } = new();
    
    public LogOptions LogOptions { get; set; } = new();
    
    public OpenTelemetryOptions OpenTelemetryOptions { get; set; } = new();
}