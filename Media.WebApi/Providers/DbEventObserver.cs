using Media.WebApi.Internal.Loggers;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Platform.Domain.EF;
using Platform.WebApi.Context;

namespace Media.WebApi.Providers;

internal class DbEventObserver : IDbEventObserver
{
    private readonly IOperationContextProvider _contextProvider;

    public DbEventObserver(IOperationContextProvider contextProvider)
    {
        _contextProvider = contextProvider;
    }
    
    public void HandleSql(LogLevel logLevel, TimeSpan duration, string sql)
    {
        OperationContext? context = _contextProvider.GetContext();
        if (context == null)
        {
            return;
        }
        
        SqlRequestsLogger.Log(
            context, 
            MapLogLevel(logLevel), 
            duration,
            sql);  
    }

    public void HandleEndOperationModificationScope(DateTime beginDateTime)
    {
        OperationContext? context = _contextProvider.GetContext();
        if (context == null)
        {
            return;
        }

        OperationModificationScopeLogger.Log(context, beginDateTime);
    }

    public void HandleEndOperationReaderScope(DateTime beginDateTime)
    {
        OperationContext? context = _contextProvider.GetContext();
        if (context == null)
        {
            return;
        }

        OperationReaderScopeLogger.Log(context, beginDateTime);
    }

    public void HandleUnexpectedException(Exception exception)
    {
    }

    private static NLog.LogLevel MapLogLevel(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => NLog.LogLevel.Trace,
            LogLevel.Debug => NLog.LogLevel.Debug,
            LogLevel.Information => NLog.LogLevel.Info,
            LogLevel.Warning => NLog.LogLevel.Warn,
            LogLevel.Error => NLog.LogLevel.Error,
            LogLevel.Critical => NLog.LogLevel.Fatal,
            _ => NLog.LogLevel.Debug
        };
    }
}