using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using ProtoBuf;
using Utilities;

namespace Platform.Core.DistributedCache;

public static class DistributedCacheHelper
{
    private static readonly JsonSerializerOptions CacheSerializerOptions = new()
    {
        // Результаты кэширования могут быть достаточно большими, поэтому резервируем 1Mb при сериализации ответа.
        DefaultBufferSize = 1024 * 1024
    };

    public static async Task<TValue> GetAsync<TKey, TValue>(
        IDistributedCache cache,
        TKey key,
        IDistributedCacheObserver? observer = null)
        where TKey : IDistributedCacheKey, new()
    {
        if (key.CachedValueRelevanceInterval == default)
        {
            return default!;
        }

        var stringKey = GetCacheKey(key);

        DateTime utcNow = DateTime.UtcNow;

        return await GetAsync<TValue>(cache, stringKey, key.CachedValueRelevanceInterval, utcNow, observer);
    }

    public static async Task SetAsync<TKey, TValue>(
        IDistributedCache cache,
        TKey key,
        TValue value,
        IDistributedCacheObserver? observer = null)
        where TKey : IDistributedCacheKey, new()
    {
        var stringKey = GetCacheKey(key);

        DateTime utcNow = DateTime.UtcNow;
        DateTime absoluteExpiration = utcNow.Add(key.CachedValueRelevanceInterval);

        await SetAsync(cache, stringKey, value, absoluteExpiration, utcNow, observer);
    }

    private static async Task<TValue> GetAsync<TValue>(
        IDistributedCache cache,
        string key,
        TimeSpan cachedResultRelevanceInterval,
        DateTime beginDateTime,
        IDistributedCacheObserver? observer = null)
    {
        int size = 0;
        Exception? exception = null;

        try
        {
            // Вычисляем крайнюю дату валидности данных кэша.
            DateTime cachedResultRelevanceTimestamp = cachedResultRelevanceInterval != TimeSpan.MaxValue
                ? beginDateTime.Add(-cachedResultRelevanceInterval)
                : DateTime.MinValue;

            byte[]? data = await cache.GetAsync(key);
            if (data != null)
            {
                size = data.Length;

                CachedItem<TValue> cachedItem;
                if (typeof(TValue).IsDefined(typeof(ProtoContractAttribute), inherit: true))
                {
                    data = CompressionHelper.Decompress(data);
                    cachedItem = Serializer.Deserialize<CachedItem<TValue>>(new ReadOnlySpan<byte>(data));
                }
                else
                {
                    cachedItem = JsonSerializer.Deserialize<CachedItem<TValue>>(new ReadOnlySpan<byte>(data))!;
                }

                if (cachedItem != null && cachedResultRelevanceTimestamp < cachedItem.Timestamp)
                {
                    return cachedItem.Value;
                }
            }

            return default!;
        }
        catch (Exception e)
        {
            exception = e;

            throw;
        }
        finally
        {
            observer?.OnGet(beginDateTime, DateTime.UtcNow, key, size, exception);
        }
    }

    private static async Task SetAsync<TValue>(
        IDistributedCache cache,
        string key,
        TValue value,
        DateTime absoluteExpiration,
        DateTime beginDateTime,
        IDistributedCacheObserver? observer = null)
    {
        int size = 0;
        Exception? exception = null;
        try
        {
            byte[] data = GetCacheData(value, beginDateTime);

            size = data.Length;

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = absoluteExpiration,
            };

            await cache.SetAsync(key, data, cacheOptions);
        }
        catch (Exception e)
        {
            exception = e;

            throw;
        }
        finally
        {
            observer?.OnSet(beginDateTime, DateTime.UtcNow, key, size, exception);
        }
    }

    private static byte[] GetCacheData<T>(T value, DateTime timestamp)
    {
        var cachedItem = new CachedItem<T>(value, timestamp);

        byte[] data;

        if (typeof(T).IsDefined(typeof(ProtoContractAttribute), true))
        {
            using var memoryStream = new MemoryStream();

            Serializer.Serialize(memoryStream, cachedItem);
            data = CompressionHelper.Compress(memoryStream.ToArray());
        }
        else
        {
            data = JsonSerializer.SerializeToUtf8Bytes(cachedItem, CacheSerializerOptions);
        }

        return data;
    }

    private static string GetCacheKey<TKey>(TKey parameters) where TKey : IDistributedCacheKey, new()
    {
        TKey? keyForCache = CloneHelper.CloneDeep(parameters);

        keyForCache.CachedValueRelevanceInterval = TimeSpan.Zero;

        return GetCacheKeyCore(keyForCache);
    }

    private static string GetCacheKeyCore<TParameters>(TParameters parameters)
    {
        return Convert.ToHexString(SHA256.HashData(JsonSerializer.SerializeToUtf8Bytes(parameters)));
    }
}