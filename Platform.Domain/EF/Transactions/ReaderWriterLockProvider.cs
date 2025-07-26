using Nito.AsyncEx;

namespace Platform.Domain.EF.Transactions;

/// <summary>
/// Реализует асинхронный провайдер блокировок с использованием ReaderWriterLockSlim. (Мульти чтение, одиночная запись)
/// </summary>
public class ReaderWriterLockProvider : IAsyncLockProvider
{
    private readonly AsyncReaderWriterLock _readerWriterLock = new();

    public async Task<IAsyncDisposable> EnterReadAsync(CancellationToken cancellationToken = default)
    {
        IDisposable lockHandle = await _readerWriterLock.ReaderLockAsync(cancellationToken);
        
        return new AsyncDisposableAdapter(lockHandle);
    }

    public async Task<IAsyncDisposable> EnterWriteAsync(CancellationToken cancellationToken = default)
    {
        IDisposable lockHandle = await _readerWriterLock.WriterLockAsync(cancellationToken);
        
        return new AsyncDisposableAdapter(lockHandle);
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }

    private class AsyncDisposableAdapter : IAsyncDisposable
    {
        private readonly IDisposable _disposable;

        public AsyncDisposableAdapter(IDisposable disposable)
        {
            _disposable = disposable;
        }

        public ValueTask DisposeAsync()
        {
            _disposable.Dispose();
            return ValueTask.CompletedTask;
        }
    }
}