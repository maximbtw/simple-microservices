namespace Platform.Core.CoreCache;

public interface ICoreCacheIndexesUpdater<in TUpdateSource>
{
    void Update(TUpdateSource updateSource, bool fullUpdate);
}