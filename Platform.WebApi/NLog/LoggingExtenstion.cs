using Platform.WebApi.NLog.Configuration;

namespace Platform.WebApi.NLog;

public static class LoggingExtenstion
{
    public static void AddLogEnvironment(LogOptions options)
    {
        Environment.SetEnvironmentVariable("Log_Url", options.NetworkUrl);
        Environment.SetEnvironmentVariable("Subsystem", options.Subsystem);
    }
}