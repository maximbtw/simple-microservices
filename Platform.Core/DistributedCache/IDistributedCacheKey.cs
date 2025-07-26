namespace Platform.Core.DistributedCache;

public interface IDistributedCacheKey
{
    /// <summary>
    /// Актуальность данных в кэше.
    /// </summary>
    TimeSpan CachedValueRelevanceInterval { get; set; }
}