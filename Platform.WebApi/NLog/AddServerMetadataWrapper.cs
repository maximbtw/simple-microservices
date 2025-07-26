using System.Net;
using System.Net.Sockets;
using NLog;
using NLog.Common;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace Platform.WebApi.NLog;

[Target("AddServerMetadataWrapper")]
public class AddServerMetadataWrapper : WrapperTargetBase
{
    private readonly string _machineName = Environment.MachineName;
    private readonly string _ipAddress;

    public AddServerMetadataWrapper() => _ipAddress = GetLocalIpAddress();

    protected override void Write(IList<AsyncLogEventInfo> logEvents)
    {
        for (int i = 0; i < logEvents.Count; i++)
        {
            LogEventInfo logEvent = CloneEventInfo(logEvents[i].LogEvent);
            logEvent.Properties["HostName"] = _machineName;
            logEvent.Properties["HostIP"] = _ipAddress;
            logEvents[i] = logEvent.WithContinuation(logEvents[i].Continuation);
        }
        WrappedTarget!.WriteAsyncLogEvents(logEvents);
    }

    protected override void Write(AsyncLogEventInfo logEventInfo)
    {
        LogEventInfo logEvent = CloneEventInfo(logEventInfo.LogEvent);
        logEvent.Properties["HostName"] = _machineName;
        logEvent.Properties["HostIP"] = _ipAddress;
        WrappedTarget!.WriteAsyncLogEvent(logEvent.WithContinuation(logEventInfo.Continuation));
    }

    private LogEventInfo CloneEventInfo(LogEventInfo logEvent)
    {
        var modifiedLogEvent = LogEventInfo.Create(logEvent.Level, logEvent.LoggerName, logEvent.Message);
        
        modifiedLogEvent.TimeStamp = logEvent.TimeStamp;
        modifiedLogEvent.Exception = logEvent.Exception;
        modifiedLogEvent.Parameters = logEvent.Parameters;
        modifiedLogEvent.FormatProvider = logEvent.FormatProvider;
        foreach (KeyValuePair<object, object?> property in logEvent.Properties)
        {
            modifiedLogEvent.Properties[property.Key] = property.Value;
        }
        return modifiedLogEvent;
    }

    private string GetLocalIpAddress()
    {
        try
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            return host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)?.ToString() ?? "unknown";
        }
        catch
        {
            return "unknown";
        }
    }
}