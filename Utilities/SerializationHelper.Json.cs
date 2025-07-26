using System.Collections;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Utilities;

public partial class SerializationHelper
{
    public static string SerializeAsJsonString<T>(T obj, bool ignoreNullPropertiesAndEmptyCollections = false)
    {
        if (obj == null)
        {
            return string.Empty;
        }

        if (ignoreNullPropertiesAndEmptyCollections)
        {
            JToken jToken = JToken.FromObject(obj, IgnoreDefaultsAndEmptyCollectionsJsonSerializer);

            return RemoveJsonEmptyChildren(jToken).ToString(Formatting.None);
        }

        return JsonConvert.SerializeObject(obj);
    }
        
    public static T DeserializeFromJsonString<T>(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return default!;
        }

        return JsonConvert.DeserializeObject<T>(json)!;
    }

    private static JToken RemoveJsonEmptyChildren(JToken token)
    {
        if (token.Type == JTokenType.Object)
        {
            var copy = new JObject();
            foreach (JProperty prop in token.Children<JProperty>())
            {
                JToken child = prop.Value;
                if (child.HasValues)
                {
                    child = RemoveJsonEmptyChildren(child);
                }

                if (!child.IsEmptyOrDefaultJToken())
                {
                    copy.Add(prop.Name, child);
                }
            }

            return copy;
        }

        if (token.Type == JTokenType.Array)
        {
            var copy = new JArray();
            foreach (JToken item in token.Children())
            {
                JToken child = item;
                if (child.HasValues)
                {
                    child = RemoveJsonEmptyChildren(child);
                }

                if (!child.IsEmptyOrDefaultJToken())
                {
                    copy.Add(child);
                }
            }

            return copy;
        }

        return token;
    }

    private static bool IsEmptyOrDefaultJToken(this JToken token)
    {
        return
            token.Type == JTokenType.Array && !token.HasValues ||
            token.Type == JTokenType.Object && !token.HasValues ||
            token.Type == JTokenType.String && token.ToString() == string.Empty ||
            token.Type == JTokenType.Boolean && token.Value<bool>() == false ||
            token.Type == JTokenType.Integer && token.Value<long>() == 0 ||
            token.Type == JTokenType.Float && token.Value<double>() == 0.0 ||
            token.Type == JTokenType.Null;
    }
        
    private class IgnoreEmptyCollectionsJsonContractResolver : DefaultContractResolver
    {
        /// <inheritdoc />
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty jsonProperty = base.CreateProperty(member, memberSerialization);

            if (member is PropertyInfo propertyInfo)
            {
                if (!propertyInfo.CanWrite)
                {
                    jsonProperty.ShouldSerialize = _ => false;

                }
                else if (typeof(IList).IsAssignableFrom(propertyInfo.PropertyType))
                {
                    jsonProperty.ShouldSerialize = obj =>
                        obj != null && (propertyInfo.GetValue(obj, index: null) as IList)?.Count > 0;
                }
            }

            return jsonProperty;
        }
    }

    private static readonly JsonSerializerSettings IgnoreDefaultsAndEmptyCollectionsJsonSerializerSettings =
        new JsonSerializerSettings()
        {
            Formatting = Formatting.None,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            ContractResolver = new IgnoreEmptyCollectionsJsonContractResolver(),
            Converters = new List<JsonConverter>
            {
                new StringEnumConverter()
            }
        };

    private static readonly JsonSerializer IgnoreDefaultsAndEmptyCollectionsJsonSerializer =
        JsonSerializer.Create(IgnoreDefaultsAndEmptyCollectionsJsonSerializerSettings);
}