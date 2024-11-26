namespace AnastasiaHueApp.Util.Extensions;

/// <summary>
/// Extension methods for the <see cref="int"/> primitive.
/// </summary>
public static class IntExtensions
{
    public static bool IsInRange(this int it, int a, int b) => it >= a && it <= b;
}