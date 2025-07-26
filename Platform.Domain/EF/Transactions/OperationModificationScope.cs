using System.Data;

namespace Platform.Domain.EF.Transactions;

public class OperationModificationScope : OperationScopeBase
{
    private readonly IDbEventObserver _observer;
    
    private IAsyncDisposable? _writeLock;
    private DateTime _scopeStart;
    private bool _hasErrors;
    private bool _committed;

    public OperationModificationScope(
        DbContextBase dbContext,
        IDbEventObserver observer,
        IAsyncLockProvider lockProvider,
        bool enableGlobalLock = true,
        IsolationLevel isolationLevel = IsolationLevel.Snapshot,
        CancellationToken cancellationToken = default)
        : base(dbContext, observer, lockProvider, enableGlobalLock, isolationLevel, cancellationToken)
    {
        _observer = observer;
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _hasErrors = true;

            _observer.HandleUnexpectedException(ex);
        }
    }

    public async Task CommitChangesIfSucceededAsync(bool success, CancellationToken cancellationToken = default)
    {
        if (success && !_hasErrors)
        {
            await this.DbContextTransaction.CommitAsync(cancellationToken);
            _committed = true;
        }
        else
        {
            await this.DbContextTransaction.RollbackAsync(cancellationToken);
        }
    }

    protected override async Task EnterLockCore(CancellationToken cancellationToken = default)
    {
        _writeLock = await this.LockProvider.EnterWriteAsync(cancellationToken);
        _scopeStart = DateTime.UtcNow;
    }

    protected override async ValueTask ExitLockCore()
    {
        if (_writeLock != null)
        {
            await _writeLock.DisposeAsync();
            _writeLock = null;
        }

        _observer.HandleEndOperationModificationScope(_scopeStart);
    }

    protected override void DisposeCore()
    {
        if (!_committed)
        {
            this.DbContextTransaction.Rollback();
        }
    }
}
