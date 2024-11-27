using System.Text.Json;

namespace AnastasiaHueApp.Util.Extensions;

public static class JsonElementExtensions
{
    /// <summary>
    /// Retrieves an enum value of type T directly from a JSON element.
    /// </summary>
    /// <typeparam name="T">The enum type to convert to.</typeparam>
    /// <param name="element">The JSON element containing the string representation of the enum.</param>
    /// <returns>The corresponding enum value of type T.</returns>
    /// <exception cref="ArgumentException">Thrown if T is not an enum.</exception>
    /// <exception cref="JsonException">Thrown if the value cannot be converted to the enum.</exception>
    public static T GetEnum<T>(this JsonElement element) where T : struct, Enum
    {
        var stringValue = element.GetString();
        if (stringValue is null) throw new JsonException("The JSON element is null (or perhaps not a string).");
        if (Enum.TryParse<T>(stringValue, true, out var enumValue)) return enumValue;
        throw new JsonException($"'{stringValue}' could not be converted to enum of type {typeof(T).Name}.");
    }
}