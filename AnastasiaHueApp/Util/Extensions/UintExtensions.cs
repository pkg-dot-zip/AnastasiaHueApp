namespace AnastasiaHueApp.Util.Extensions;

using Color = Color.Color;

/// <summary>
/// Extension methods for the <see cref="uint"/> primitive.
/// </summary>
public static class UintExtensions
{
    /// <summary>
    /// Helper extension method that converts this <see cref="uint"/> <paramref name="n"/> to an instance of <see cref="Color"/>.
    /// </summary>
    /// <param name="n">Hexadecimal number to convert.</param>
    /// <returns>Hex formatted <paramref name="n"/> as <see cref="Color"/>.</returns>
    public static Color ToColor(this uint n) => Color.FromHex(n);
}