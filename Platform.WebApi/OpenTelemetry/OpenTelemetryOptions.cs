namespace Platform.WebApi.OpenTelemetry;

public class OpenTelemetryOptions
{
    public bool TracingEnabled { get; set; } = true;
    
    public bool MetricsEnabled { get; set; } = true;
    
    public string Url { get; set; } = string.Empty;
    
    public string Subsystem { get; set; } = string.Empty;
}