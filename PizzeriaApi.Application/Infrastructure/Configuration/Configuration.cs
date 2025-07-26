using Platform.Core.DistributedCache.Configuration;
using Platform.WebApi.OpenTelemetry;

namespace PizzeriaApi.Application.Infrastructure.Configuration;

public class Configuration
{
    public JwtOptions JwtOptions { get; set; } = new();
    
    public DependenciesOptions DependenciesOptions { get; set; } = new();
    
    public DistributedCacheOptions DistributedCacheOptions { get; set; } = new();
    
    public OpenTelemetryOptions OpenTelemetryOptions { get; set; } = new();
}