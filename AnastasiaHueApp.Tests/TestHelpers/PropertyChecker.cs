using System.Reflection;

namespace AnastasiaHueApp.Tests.TestHelpers;

/// <summary>
/// Helper util classes to avoid duplicate code in our unit tests.
/// </summary>
public static class PropertyChecker
{
    public static bool CheckAllPropertiesAreNotNull<T>(
        T obj,
        out Dictionary<string, bool> propertyStatuses,
        params string[] propertiesToIgnore)
    {
        propertyStatuses = new Dictionary<string, bool>();

        if (obj is null) throw new ArgumentNullException(nameof(obj));

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