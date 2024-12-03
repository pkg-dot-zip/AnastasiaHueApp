namespace AnastasiaHueApp.Util.Extensions;

/// <summary>
/// Contains extension methods for <seealso cref="IList{T}"/>.
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IListExtensions
{
    /// <summary>
    /// Adds all of the <paramref name="elements"/> to the end of the <paramref name="collection"/>.
    /// Useful since <seealso cref="List{T}.AddRange"/> is only available in the implementation <seealso cref="List{T}"/>.
    /// </summary>
    /// <typeparam name="T"><see langword="type"/> of <seealso cref="elements"/></typeparam>
    /// <param name="collection">Collection to add the <paramref name="elements"/> to.</param>
    /// <param name="elements">Elements to add to <paramref name="collection"/>.</param>
    public static void AddAll<T>(this IList<T> collection, params T[] elements) => collection.AddAll(elements as IEnumerable<T>);

    /// <summary>
    /// Adds all of the <paramref name="elements"/> to the end of the <paramref name="collection"/>.
    /// Useful since <seealso cref="List{T}.AddRange"/> is only available in the implementation <seealso cref="List{T}"/>.
    /// </summary>
    /// <typeparam name="T"><see langword="type"/> of <seealso cref="elements"/></typeparam>
    /// <param name="collection">Collection to add the <paramref name="elements"/> to.</param>
    /// <param name="elements">Elements to add to <paramref name="collection"/>.</param>
    public static void AddAll<T>(this IList<T> collection, IEnumerable<T> elements)
    {
        foreach (var element in elements) collection.Add(element);
    }
}