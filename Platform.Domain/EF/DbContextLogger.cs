using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Platform.Domain.EF;

internal class DbContextLogger : ILogger
{
    private readonly IDbEventObserver _dbEventObserver;
    
    public DbContextLogger(IDbEventObserver dbEventObserver)
    {
        _dbEventObserver = dbEventObserver;
    }

    public void Log<TState>(
        LogLevel logLevel, 
        EventId eventId, 
        TState state, 
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }
        
        if (eventId == RelationalEventId.CommandExecuted.Id || eventId == RelationalEventId.CommandError.Id)
        {
            TimeSpan duration = GetDuration(state);
            string? sql = GetSql(state, exception, formatter);
            if (sql != null)
            {
                _dbEventObserver.HandleSql(logLevel, duration, sql);
            }
        }
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= LogLevel.Information;
    }

    public IDisposable BeginScope<TState>(TState state) where TState : notnull
    {
        return NullScope.Instance;
    }
    
    private class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();
        public void Dispose() { }
    }
    
    private TimeSpan GetDuration<TState>(TState state)
    {
        if (state is IEnumerable<KeyValuePair<string, object>> items)
        {
            foreach (KeyValuePair<string, object> item in items)
            {
                if (item.Key == "elapsed")
                {
                    if (double.TryParse(item.Value.ToString(), out double ms))
                    {
                        return TimeSpan.FromMilliseconds(ms);
                    }
                }
            }
        }

        return TimeSpan.Zero;
    }
    
    private static string? GetSql<TState>(
        TState state,
        Exception? exception,
        Func<TState, Exception, string> formatter)
    {
        string? commandText = string.Empty;
        string? parameters = string.Empty;

        if (state is IEnumerable<KeyValuePair<string, object>> items)
        {
            foreach (KeyValuePair<string, object> item in items)
            {
                switch (item.Key)
                {
                    case "commandText":
                        commandText = item.Value.ToString();
                        break;
                    case "parameters":
                        parameters = item.Value.ToString();
                        break;
                }
            }
        }

        if (string.IsNullOrEmpty(commandText) && exception != null)
        {
            return formatter(state, exception);
        }

        if (string.IsNullOrEmpty(parameters))
        {
            return commandText;
        }

        return commandText + " -- " + parameters;
    }
}