namespace Platform.Domain.EF.Transactions;

public interface IAsyncLockProvider : IAsyncDisposable
{
    Task<IAsyncDisposable> EnterReadAsync(CancellationToken cancellationToken = default);
    
    Task<IAsyncDisposable> EnterWriteAsync(CancellationToken cancellationToken = default);
}