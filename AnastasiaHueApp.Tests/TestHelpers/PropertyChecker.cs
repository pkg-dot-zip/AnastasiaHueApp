using System.Reflection;

namespace AnastasiaHueApp.Tests.TestHelpers;

public static class PropertyChecker
{
    public static bool CheckAllPropertiesAreNotNull<T>(
        T obj,
        out Dictionary<string, bool> propertyStatuses,
        params string[] propertiesToIgnore)
    {
        propertyStatuses = new Dictionary<string, bool>();

        if (obj == null) throw new ArgumentNullException(nameof(obj));

        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => !propertiesToIgnore.Contains(p.Name));

        bool allPropertiesNotNull = true;

        foreach (var property in properties)
        {
            var value = property.GetValue(obj);
            bool isNotNull = value != null;
            propertyStatuses[property.Name] = isNotNull;

            if (!isNotNull) allPropertiesNotNull = false;
        }

        return allPropertiesNotNull;
    }
}