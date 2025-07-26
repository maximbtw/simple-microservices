using System.Text;
using Newtonsoft.Json;

namespace Utilities;

public static partial class SerializationHelper
{
    public static string SerializeToBase64<T>(T obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        byte[] bytes = SerializeToBytes(obj);
        
        return Convert.ToBase64String(bytes);
    }

    public static T DeserializeFromBase64<T>(string base64)
    {
        if (string.IsNullOrWhiteSpace(base64))
        {
            throw new ArgumentNullException(nameof(base64));
        }

        byte[] bytes = Convert.FromBase64String(base64);
        string json = Encoding.UTF8.GetString(bytes);
        return JsonConvert.DeserializeObject<T>(json)!;
    }
    
    public static byte[] SerializeToBytes<T>(T obj)
    {
        if (obj == null)
        {
            return [];   
        }

        string json = SerializeAsJsonString(obj);

        return Encoding.UTF8.GetBytes(json);
    }
}