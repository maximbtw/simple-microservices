using NLog;
using LogLevel = NLog.LogLevel;

namespace Auth.WebApi.Internal.Loggers;

internal static class OperationErrorsLogger
{
    private static readonly Logger Logger = LogManager.GetLogger(nameof(OperationErrorsLogger));
    
    public static void Log(string traceId, Exception exception)
    {
        var logInfo = new LogEventInfo(LogLevel.Info, Logger.Name, message: string.Empty)
        {
            Properties =
            {
                ["TraceId"] = traceId,
                ["Exception"] = exception.ToString().Replace(Environment.NewLine, " ")
            }
        };

        Logger.Log(logInfo);
    }
}