using Microsoft.Extensions.DependencyInjection;

namespace Platform.Core.CoreCache;

public static class DependencyInjectionExtensions
{
    public static void AddCoreCache<TStorage, TStorageUpdateSource>(this IServiceCollection services,
        Func<TStorage, ICoreCacheIndexesUpdater<TStorageUpdateSource>> updaterFactory,
        Func<TStorage, ICoreCacheIndexesReader> readerFactory) where TStorage : new()
    {
        services.AddSingleton<CoreCache<TStorage, TStorageUpdateSource>>();
        services.AddSingleton<ICoreCacheReader>(x => x.GetService<CoreCache<TStorage, TStorageUpdateSource>>()!);
        services.AddSingleton<ICoreCacheUpdater<TStorageUpdateSource>>(x =>
            x.GetService<CoreCache<TStorage, TStorageUpdateSource>>()!);

        services.AddSingleton(updaterFactory);
        services.AddSingleton(readerFactory);
    }
}