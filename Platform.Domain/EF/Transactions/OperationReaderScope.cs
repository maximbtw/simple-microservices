using System.Data;

namespace Platform.Domain.EF.Transactions;

public class OperationReaderScope : OperationScopeBase
{
    private readonly IDbEventObserver _observer;
    
    private IAsyncDisposable? _readLock;
    private DateTime _scopeStart;

    public OperationReaderScope(
        DbContextBase dbContext,
        IDbEventObserver observer,
        IAsyncLockProvider lockProvider,
        bool enableGlobalLock = true,
        IsolationLevel isolationLevel = IsolationLevel.ReadUncommitted,
        CancellationToken cancellationToken = default)
        : base(dbContext, observer, lockProvider, enableGlobalLock, isolationLevel, cancellationToken)
    {
        _observer = observer;
    }

    protected override async Task EnterLockCore(CancellationToken cancellationToken = default)
    {
        _readLock = await this.LockProvider.EnterReadAsync(cancellationToken);
        _scopeStart = DateTime.UtcNow;
    }

    protected override async ValueTask ExitLockCore()
    {
        if (_readLock != null)
        {
            await _readLock.DisposeAsync();
            _readLock = null;
        }

        _observer.HandleEndOperationReaderScope(_scopeStart);
    }
}
