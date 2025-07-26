using Platform.WebApi.NLog.Configuration;
using Platform.WebApi.OpenTelemetry;

namespace Media.Application.Infrastructure.Configuration;

public class Configuration
{
    public DatabaseOptions DatabaseOptions { get; set; } = new();
    
    public CloudinaryOptions CloudinaryOptions { get; set; } = new();
    
    public LogOptions LogOptions { get; set; } = new();
    
    public OpenTelemetryOptions OpenTelemetryOptions { get; set; } = new();
}