using System.Data;
using Media.Domain;
using Platform.Domain.EF;
using Platform.Domain.EF.Transactions;

namespace Media.WebApi.Providers;

internal class DbScopeProvider : IDbScopeProvider
{
    private readonly MediaDbContext _dbContext;
    private readonly IDbEventObserver _dbEventObserver;
    private readonly IAsyncLockProvider _lockProvider;

    public DbScopeProvider(MediaDbContext dbContext, IDbEventObserver dbEventObserver, IAsyncLockProvider lockProvider)
    {
        _dbContext = dbContext;
        _dbEventObserver = dbEventObserver;
        _lockProvider = lockProvider;
    }

    public OperationReaderScope GetReaderScope(
        bool enableGlobalLock = false,
        IsolationLevel isolationLevel = IsolationLevel.Snapshot, 
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
        bool enableGlobalLock = false,
        IsolationLevel isolationLevel = IsolationLevel.Snapshot, 
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