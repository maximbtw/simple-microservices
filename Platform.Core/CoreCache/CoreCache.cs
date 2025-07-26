namespace Platform.Core.CoreCache;

public class CoreCache<TStorage, TStorageUpdateSource>(
    Func<TStorage, ICoreCacheIndexesUpdater<TStorageUpdateSource>> updaterFactory,
    Func<TStorage, ICoreCacheIndexesReader> readerFactory)
    : ICoreCacheReader, ICoreCacheUpdater<TStorageUpdateSource>
    where TStorage : new()
{
    private TStorage _storage = new();
    private readonly ReaderWriterLockSlim _lock = new();
    private readonly ManualResetEventSlim _firstUpdateEvent = new(initialState: false);

    public void Update(TStorageUpdateSource updateSource, bool fullUpdate)
    {
        try
        {
            _lock.EnterWriteLock();

            if (fullUpdate)
            {
                _storage = new TStorage();
            }

            updaterFactory(_storage).Update(updateSource, fullUpdate);

            _firstUpdateEvent.Set();
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public CoreCacheReaderContext GetReaderContext()
    {
        _firstUpdateEvent.Wait();
        
        return new CoreCacheReaderContext(readerFactory(_storage), _lock);
    }
}