using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;

namespace Platform.Test;

/// <summary>
/// Иммитирует сессию приложения
/// </summary>
public class TestDbExecutorProvider : IAsyncDisposable
{
    //private readonly string _dbPath;
    private readonly string _connectionString;
    private readonly IAsyncLockProvider _lockProvider;
    
    public TestDbExecutorProvider(IAsyncLockProvider lockProvider)
    {
        _lockProvider = lockProvider;
       // _dbPath = Path.Combine(Directory.GetCurrentDirectory(), "db-test.sqlite");
        _connectionString = "Server=localhost;Port=5432;Database=Test;Username=postgres;Password=123123;";

        DeleteDbIfExists();
        CreateEmptyDatabaseAsync().GetAwaiter().GetResult();
    }
    
    /// <summary>
    /// Иммитирует Scoped объект, который работает в рамках одного http запроса
    /// </summary>
    public IDbScopeProvider GetDbScopeProviderAsync()
    {
        return new TestDbScopeProvider(_lockProvider, _connectionString);
    }

    private async Task CreateEmptyDatabaseAsync()
    {
        await using IDbScopeProvider scopeProvider = GetDbScopeProviderAsync();
        await using OperationModificationScope scope = scopeProvider.GetModificationScope();

        await scope.GetDbContext().Database.EnsureCreatedAsync();
    }
    
    public async ValueTask DisposeAsync()
    {
        await Task.Delay(500);
        GC.Collect();
        GC.WaitForPendingFinalizers();
        
        DeleteDbIfExists();
    }

    private void DeleteDbIfExists()
    {
        // if (File.Exists(_dbPath))
        // {
        //     File.Delete(_dbPath);
        // }
    }
}

public class TestDbScopeProvider : IDbScopeProvider
{
    private readonly TestDbContext _dbContext;
    private readonly IDbEventObserver _dbEventObserver;
    private readonly IAsyncLockProvider _lockProvider;
    
    public TestDbScopeProvider(IAsyncLockProvider lockProvider, string connectionString)
    {
        _lockProvider = lockProvider;
        _dbEventObserver = new Mock<IDbEventObserver>().Object;
        _dbContext = CreateDbContext( connectionString);
    }
    
    private TestDbContext CreateDbContext(string connectionString)
    {
        DbContextOptions<TestDbContext> options = new DbContextOptionsBuilder<TestDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        var dbContext = new TestDbContext(options, _dbEventObserver);
        return dbContext;
    }
    
    public OperationReaderScope GetReaderScope(
        bool enableGlobalLock = false,
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, 
        CancellationToken cancellationToken = default)
    {
        return new OperationReaderScope(
            _dbContext,
            _dbEventObserver, 
            _lockProvider, 
            enableGlobalLock, 
            isolationLevel, 
            cancellationToken);
    }

    public OperationModificationScope GetModificationScope(
        bool enableGlobalLock = true,
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, 
        CancellationToken cancellationToken = default)
    {
        return new OperationModificationScope(
            _dbContext, 
            _dbEventObserver, 
            _lockProvider, 
            enableGlobalLock, 
            isolationLevel, 
            cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
    }
}

public class TestDbContext : DbContextBase
{
    public TestDbContext(DbContextOptions<TestDbContext> options, IDbEventObserver observer) : base(options, observer)
    {
    }
}
