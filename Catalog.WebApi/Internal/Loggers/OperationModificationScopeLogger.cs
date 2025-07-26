using NLog;
using Platform.WebApi.Context;
using LogLevel = NLog.LogLevel;

namespace Catalog.WebApi.Internal.Loggers;

public static class OperationModificationScopeLogger
{
    private static readonly Logger Logger = LogManager.GetLogger(nameof(OperationModificationScopeLogger));

    public static void Log(OperationContext context, DateTime beginDateTime)
    {
        DateTime endDateTime = DateTime.UtcNow;
        TimeSpan duration = endDateTime - beginDateTime;
        
        var logInfo = new LogEventInfo(LogLevel.Info, Logger.Name, message: string.Empty)
        {
            Properties =
            {
                ["BeginTime"] = beginDateTime,
                ["EndTime"] = endDateTime,
                ["Duration"] = duration.TotalMilliseconds.ToString("N2"),
                ["TraceId"] = context.TraceId,
                ["User"] = context.User?.Login,
                ["Operation"] = context.OperationInfo.OperationName
            }
        };

        Logger.Log(logInfo);
    }
}