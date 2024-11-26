namespace AnastasiaHueApp.Util.Extensions;

/// <summary>
/// Extension methods for the <see cref="int"/> primitive.
/// </summary>
public static class IntExtensions
{
    /// <summary>
    /// Checks whether a number <paramref name="n"/> is in the range of <paramref name="a"/>-<paramref name="b"/>. This also returns true if <paramref name="n"/> equals <paramref name="a"/> or <paramref name="b"/>.
    /// </summary>
    /// <param name="n">Number to check for.</param>
    /// <param name="a">Lower-bounds.</param>
    /// <param name="b">Upper-bounds.</param>
    /// <returns><see langword="true"/> if <paramref name="n"/> is in the range of <paramref name="a"/>-<paramref name="b"/>. This also returns true if <paramref name="n"/> equals <paramref name="a"/> or <paramref name="b"/>. In all other cases it returns <see langword="false"/>.</returns>
    public static bool IsInRange(this int n, int a, int b) => n >= a && n <= b;
}