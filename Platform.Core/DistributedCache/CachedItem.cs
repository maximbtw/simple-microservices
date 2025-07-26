using ProtoBuf;

namespace Platform.Core.DistributedCache;

[ProtoContract]
internal class CachedItem<T>
{
    public CachedItem(T value, DateTime timestamp)
    {
        this.Value = value;
        this.Timestamp = timestamp;
    }

    [ProtoMember(1)]
    public T Value { get; set; }

    [ProtoMember(2)]
    public DateTime Timestamp { get; set; }
}
