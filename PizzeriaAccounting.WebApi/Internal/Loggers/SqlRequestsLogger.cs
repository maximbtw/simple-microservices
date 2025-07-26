using NLog;
using Platform.WebApi.Context;
using LogLevel = NLog.LogLevel;

namespace PizzeriaAccounting.WebApi.Internal.Loggers;

internal static class SqlRequestsLogger
{
    private static readonly Logger Logger = LogManager.GetLogger(nameof(SqlRequestsLogger));
    
    public static void Log(OperationContext context, LogLevel logLevel, TimeSpan duration, string sql)
    {
        var logInfo = new LogEventInfo(logLevel, Logger.Name, message: string.Empty)
        {
            Properties =
            {
                ["Duration"] = duration.TotalMilliseconds.ToString("N2"),
                ["TraceId"] = context.TraceId,
                ["User"] = context.User?.Login,
                ["Command"] = sql.Replace("\n"," "),
            }
        };

        Logger.Log(logInfo);
    }
}