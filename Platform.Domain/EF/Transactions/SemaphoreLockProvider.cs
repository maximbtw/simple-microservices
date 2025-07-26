namespace Platform.Domain.EF.Transactions;

/// <summary>
/// Реализует асинхронный провайдер блокировок с использованием SemaphoreSlim. (Одна запись, одно чтение)
/// </summary>
public class SemaphoreLockProvider : IAsyncLockProvider
{
    private readonly SemaphoreSlim _semaphore = new(initialCount: 1, maxCount: 1);

    public async Task<IAsyncDisposable> EnterReadAsync(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        return new Releaser(_semaphore);
    }

    public async Task<IAsyncDisposable> EnterWriteAsync(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        return new Releaser(_semaphore);
    }

    public ValueTask DisposeAsync()
    {
        _semaphore.Dispose();
        return ValueTask.CompletedTask;
    }

    private sealed class Releaser : IAsyncDisposable
    {
        private readonly SemaphoreSlim _semaphore;
        private bool _disposed;

        public Releaser(SemaphoreSlim semaphore)
        {
            _semaphore = semaphore;
        }

        public ValueTask DisposeAsync()
        {
            if (!_disposed)
            {
                _disposed = true;
                _semaphore.Release();
            }

            return ValueTask.CompletedTask;
        }
    }
}