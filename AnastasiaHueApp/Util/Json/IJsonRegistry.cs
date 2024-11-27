using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AnastasiaHueApp.Util.Json;

/// <summary>
/// Custom registry container for json parsing.
/// This was needed because auto-parsing wasn't always that efficient code-wise due to elements being rooted or nested in a way we did not like.
/// </summary>
public interface IJsonRegistry
{
    /// <summary>
    /// Registers the code to parse <typeparamref name="T"/> from a <see cref="string"/> into a private <see cref="Dictionary{Type, Func}"/>.
    /// After registering a <see cref="Type"/> here you can retrieve it by calling <see cref="Parse{T}"/>.
    /// </summary>
    /// <typeparam name="T"><see cref="Type"/> to register <paramref name="parser"/> for.</typeparam>
    /// <param name="parser">Method to parse <typeparamref name="T"/> from a <see cref="string"/>.</param>
    public void Register<T>(Func<string, T> parser);

    /// <summary>
    /// Parses <paramref name="json"/> into an instance of <typeparam name="T"/> with all properties set.
    /// If parsing fails, usually meaning that the <paramref name="json"/> was not meant for type <typeparamref name="T"/>, <see langword="null"/> is returned.
    /// </summary>
    /// <typeparam name="T">Nullable type to return.</typeparam>
    /// <param name="json"><see cref="string"/> in <c>.Json</c> format.</param>
    /// <returns><typeparamref name="T"/> with all properties set like in the <paramref name="json"/>. If the <paramref name="json"/> is not meant for <typeparamref name="T"/> we return <see langword="null"/>.</returns>
    /// <exception cref="ArgumentException">If <paramref name="json"/> was empty.</exception>
    /// <exception cref="InvalidOperationException">If no parser for <typeparamref name="T"/> was registered.</exception>
    public T? Parse<T>([StringSyntax(StringSyntaxAttribute.Json)] string json);
}