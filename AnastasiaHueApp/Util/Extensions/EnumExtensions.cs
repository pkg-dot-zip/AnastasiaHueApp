namespace AnastasiaHueApp.Util.Extensions;

/// <summary>
/// Extension methods for the <see cref="Enum"/> class.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Returns the lowercase name of the value.
    /// </summary>
    /// <param name="enumValue">Enum to retrieve name for.</param>
    /// <returns>Lowercase variant of name.</returns>
    public static string GetName(this Enum enumValue) => enumValue.ToString().ToLower();
}