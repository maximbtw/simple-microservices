namespace Platform.Core.CoreCache;

public interface ICoreCacheUpdater<in TUpdateSource>
{
    void Update(TUpdateSource updateSource, bool fullUpdate);
}