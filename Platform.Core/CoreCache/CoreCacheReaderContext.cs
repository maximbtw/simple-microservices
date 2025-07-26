namespace Platform.Core.CoreCache;

public class CoreCacheReaderContext : IDisposable
{
    private readonly ReaderWriterLockSlim _locker;
    private readonly ICoreCacheIndexesReader _indexesReader;

    internal CoreCacheReaderContext(ICoreCacheIndexesReader indexesReader, ReaderWriterLockSlim locker)
    {
        _indexesReader = indexesReader;
        _locker = locker;

        _locker.EnterReadLock();
    }

    public TIndexesReader GetReader<TIndexesReader>()
        where TIndexesReader : ICoreCacheIndexesReader
        => (TIndexesReader)_indexesReader;

    public void Dispose()
    {
        _locker.ExitReadLock();
    }
}