using Microsoft.Extensions.Logging;

namespace Platform.Domain.EF;

internal class DbContextLoggerProvider : ILoggerProvider
{
    private readonly IDbEventObserver _dbEventObserver;
    
    public DbContextLoggerProvider(IDbEventObserver dbEventObserver)
    {
        _dbEventObserver = dbEventObserver;
    }
    
    public static LoggerFactory GetFactory(IDbEventObserver observer) => new([new DbContextLoggerProvider(observer)]);

    /// <inheritdoc />
    public void Dispose()
    {
    }

    /// <inheritdoc />
    public ILogger CreateLogger(string categoryName) => new DbContextLogger(_dbEventObserver);
}