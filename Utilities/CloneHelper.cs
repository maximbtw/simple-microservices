using System.Collections;
using System.Reflection;

namespace Utilities;

public static class CloneHelper
{
    public static T CloneDeep<T>(T source, bool copyIds = true) where T : new()
    {
        if(source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }
        
        return (T)CloneDeepCore(source, copyIds);
    }

    private static object CloneDeepCore(object source, bool copyIds = true)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        Type sourceType = source.GetType();

        if (IsSimpleType(sourceType))
        {
            return source;
        }

        if (IsCollection(sourceType))
        {
            Type elementType = sourceType.GetGenericArguments()[0];

            var clonedCollection = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType))!;
            foreach (object item in (IEnumerable)source)
            {
                clonedCollection.Add(CloneDeepCore(item, copyIds));
            }

            return clonedCollection;
        }

        object clone = Activator.CreateInstance(sourceType);

        foreach (PropertyInfo property in sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!property.CanWrite)
            {
                continue;
            }

            object value = property.GetValue(source);

            if (value == null)
            {
                property.SetValue(clone, null);

                continue;
            }

            Type type = property.PropertyType;

            if (!copyIds && property.Name.EndsWith("Id", StringComparison.OrdinalIgnoreCase) && IsSimpleType(type))
            {
                object? defaultValue = type.IsValueType ? Activator.CreateInstance(type) : null;

                property.SetValue(clone, defaultValue);
            }
            else
            {
                var clonedObject = CloneDeepCore(value, copyIds);

                property.SetValue(clone, clonedObject);
            }
        }

        return clone;
    }

    private static bool IsCollection(Type type)
    {
        return typeof(IEnumerable).IsAssignableFrom(type) && type.IsGenericType;
    }

    private static bool IsSimpleType(Type type)
    {
        return type.IsPrimitive || type.IsEnum || type == typeof(string) || type == typeof(decimal);
    }
}