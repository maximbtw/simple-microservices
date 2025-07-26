namespace Platform.Core.DistributedCache;

public interface IDistributedCacheObserver
{
    void OnSet(DateTime startDate, DateTime endDate, string key, int size, Exception? exception);
    
    void OnGet(DateTime startDate, DateTime endDate, string key, int size, Exception? exception);
}