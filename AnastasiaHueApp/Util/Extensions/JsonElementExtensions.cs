using System.Text.Json;

namespace AnastasiaHueApp.Util.Extensions;

/// <summary>
/// Extension methods for the <see cref="JsonElement"/> class.
/// </summary>
public static class JsonElementExtensions
{

    // TODO: What to do if there are two enums with the same characters but different casing?! Define behaviour in docs and unit tests!

    /// <summary>
    /// Retrieves an <see langword="enum"/> value of type <typeparamref name="T"/> directly from a <see cref="JsonElement"/>.
    /// </summary>
    /// <typeparam name="T">The <see langword="enum"/> <see cref="Type"/> to convert to.</typeparam>
    /// <param name="element">The <see cref="JsonElement"/> containing the string representation of the <see langword="enum"/>.</param>
    /// <returns>The corresponding <see langword="enum"/> value of type <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentException">Thrown if <typeparamref name="T"/> is not an <see langword="enum"/>.</exception>
    /// <exception cref="JsonException">Thrown if the value cannot be converted to the <see langword="enum"/>.</exception>
    public static T GetEnum<T>(this JsonElement element) where T : struct, Enum
    {
        var stringValue = element.GetString();
        if (stringValue is null) throw new JsonException("The JSON element is null (or perhaps not a string).");
        if (Enum.TryParse<T>(stringValue, true, out var enumValue)) return enumValue;
        throw new JsonException($"'{stringValue}' could not be converted to enum of type {typeof(T).Name}.");
    }
}