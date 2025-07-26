using System.Text.Json;
using NLog;
using Platform.WebApi.Context;
using LogLevel = NLog.LogLevel;

namespace Platform.WebApi.Middleware.OperationLogging;

internal static class OperationRequestsLogger
{
    private static readonly Logger Logger = LogManager.GetLogger(nameof(OperationRequestsLogger));
    
    private class StatusOperation
    {
        public bool Ok { get; set; }
    }

    public static void Log(
        OperationContext context, 
        string jsonRequest,
        string jsonResponse,
        string exception,
        string? ip,
        long memoryBeforeRequest,
        int generation0CollectionCountOnStart,
        int generation1CollectionCountOnStart,
        int generation2CollectionCountOnStart)
    {
        DateTime operationEndDateTimeUtc = DateTime.UtcNow;
        TimeSpan duration = operationEndDateTimeUtc - context.OperationInfo.OperationStartDateTimeUtc;
        
        long memoryAfterRequest = GC.GetTotalMemory(forceFullCollection: false);

        int gen0CollectionCountOnEnd = GC.CollectionCount(generation: 0);
        int gen1CollectionCountOnEnd = GC.CollectionCount(generation: 1);
        int gen2CollectionCountOnEnd = GC.CollectionCount(generation: 2);

        int generation0CollectionCount = gen0CollectionCountOnEnd - generation0CollectionCountOnStart;
        int generation1CollectionCount = gen1CollectionCountOnEnd - generation1CollectionCountOnStart;
        int generation2CollectionCount = gen2CollectionCountOnEnd - generation2CollectionCountOnStart;
        int gcCount = gen0CollectionCountOnEnd + gen1CollectionCountOnEnd + gen2CollectionCountOnEnd;

        string statusCode = "OK";
        if (!string.IsNullOrEmpty(jsonResponse))
        {
            bool ok = JsonSerializer.Deserialize<StatusOperation>(jsonResponse, JsonSerializerOptions.Web)!.Ok;
            statusCode = ok ? "OK" : "FAIL";
        }
        
        statusCode = string.IsNullOrEmpty(exception) ? statusCode : "Exception";

        var logInfo = new LogEventInfo(LogLevel.Info, Logger.Name, message: string.Empty)
        {
            Properties =
            {
                ["BeginTime"] = context.OperationInfo.OperationStartDateTimeUtc,
                ["EndTime"] = operationEndDateTimeUtc,
                ["Duration"] = duration.TotalMilliseconds.ToString("N2"),
                ["Status"] = statusCode,
                ["TraceId"] = context.TraceId,
                ["User"] = context.User?.Login,
                ["Operation"] = context.OperationInfo.OperationName,
                ["IP"] = ip, 
                ["Request"] = jsonRequest,
                ["Response"] = jsonResponse,
                ["Exception"] = exception, 
                ["MemoryBefore"] = memoryBeforeRequest / 1024d / 1024d,
                ["MemoryAfter"] = memoryAfterRequest / 1024d / 1024d,
                ["MemoryDifference"] = (memoryAfterRequest - memoryBeforeRequest) / 1024d / 1024d,
                ["Generation0CollectionCount"] = generation0CollectionCount,
                ["Generation1CollectionCount"] = generation1CollectionCount,
                ["Generation2CollectionCount"] = generation2CollectionCount,
                ["GCCount"] = gcCount,
            }
        };

        Logger.Log(logInfo);
    }
}