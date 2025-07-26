using ProtoBuf;

namespace Utilities;

public partial class SerializationHelper
{
    public static byte[] SerializeAsProto<T>(T obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        using var ms = new MemoryStream();
        Serializer.Serialize(ms, obj);
        return ms.ToArray();
    }
    
    public static T DeserializeFromProto<T>(byte[] bytes)
    {
        using var ms = new MemoryStream(bytes);
        return Serializer.Deserialize<T>(ms);
    }
    
    public static T DeserializeFromProto<T>(Stream stream)
    {
        return Serializer.Deserialize<T>(stream);
    }
}