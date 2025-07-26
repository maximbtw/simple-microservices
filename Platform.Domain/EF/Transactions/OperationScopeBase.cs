using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Platform.Domain.EF.Transactions;

public abstract class OperationScopeBase : IAsyncDisposable
{
    protected readonly DbContextBase DbContext;
    protected readonly IDbContextTransaction DbContextTransaction;
    protected readonly IAsyncLockProvider LockProvider;
    private readonly IDbEventObserver _dbEventObserver;
    private readonly bool _enableGlobalLock;
    private bool _disposed;

    protected OperationScopeBase(
        DbContextBase dbContext,
        IDbEventObserver dbEventObserver,
        IAsyncLockProvider lockProvider,
        bool enableGlobalLock,
        IsolationLevel isolationLevel,
        CancellationToken cancellationToken = default)
    {
        _dbEventObserver = dbEventObserver;
        _enableGlobalLock = enableGlobalLock;

        this.LockProvider = lockProvider;
        this.DbContext = dbContext;
        this.DbContextTransaction = dbContext.Database.BeginTransaction(isolationLevel);

        if (enableGlobalLock)
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            EnterLockCore(cancellationToken).GetAwaiter().GetResult();   
        }
    }

    public DbContextBase GetDbContext() => DbContext;

    protected virtual Task EnterLockCore(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    protected virtual ValueTask ExitLockCore()
    {
        return ValueTask.CompletedTask;
    }
    
    protected virtual void DisposeCore() { }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;

        try
        {
            DisposeCore();

            await this.DbContextTransaction.DisposeAsync();
        }
        catch (Exception ex)
        {
            _dbEventObserver.HandleUnexpectedException(ex);
            
            throw;
        }
        finally
        {
            if (_enableGlobalLock)
            {
                await ExitLockCore();
                await this.LockProvider.DisposeAsync();
            }
            
            _disposed = true;
        }
    }
}
