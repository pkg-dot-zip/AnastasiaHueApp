using AnastasiaHueApp.Util.Json;
    
namespace AnastasiaHueApp.Util.Extensions;

/// <summary>
/// Extension methods for the <see cref="HttpContent"/> class.
/// </summary>
public static class HttpContentExtensions
{
    // Only here for readability reasons for now.
    // TODO: Verify json here. If not of Json format throw exception.
    public static async Task<string> ReadAsJsonString(this HttpContent content) => await content.ReadAsStringAsync();

    /// <summary>
    /// Will parse <paramref name="content"/> into <typeparamref name="T"/>, or will return null.
    /// <br/>
    /// If, instead of one value or <see langword="null"></see>, you'd want two return types, use <see cref="ReadAsEitherAsync{T1,T2}"/>.
    /// </summary>
    /// <typeparam name="T"><see cref="Type"/> to attempt parsing for.</typeparam>
    /// <param name="content">Content to parse. Will have to contain a json response parsable by <paramref name="registry"/>.</param>
    /// <param name="registry"><see cref="IJsonRegistry"/> to use for parsing <typeparamref name="T"/>.</param>
    /// <returns><typeparamref name="T"/> or <see langword="null"/>.</returns>
    public static async Task<T?> ReadAsOrNullAsync<T>(this HttpContent content, IJsonRegistry registry)
    {
        var json = await content.ReadAsJsonString();
        return registry.Parse<T>(json);
    }

    /// <summary>
    /// Will parse <paramref name="content"/> into <typeparamref name="T1"/> or <typeparamref name="T2"/>, wrapped in an <see cref="Either{T1,T2}"/> object.
    /// Note that success of this operation is dependent on your <paramref name="registry"/> implementation.
    /// <br/>
    /// If you only need one value or <see langword="null"></see> use <see cref="ReadAsOrNullAsync{T}"/>.
    /// </summary>
    /// <typeparam name="T1"><see cref="Type"/> to attempt parsing for.</typeparam>
    /// <typeparam name="T2"><see cref="Type"/> to attempt parsing for.</typeparam>
    /// <param name="content">Content to parse. Will have to contain a json response parsable by <paramref name="registry"/>.</param>
    /// <param name="registry"><see cref="IJsonRegistry"/> to use for parsing <typeparamref name="T1"/> & <typeparamref name="T2"/>.</param>
    /// <returns>An instance of <see cref="Either{T1,T2}"/> holding <typeparamref name="T1"/> or <typeparamref name="T2"/>.</returns>
    public static async Task<Either<T1, T2>> ReadAsEitherAsync<T1, T2>(this HttpContent content, IJsonRegistry registry)
    {
        var json = await content.ReadAsJsonString();
        var value1 = registry.Parse<T1>(json);
        var value2 = registry.Parse<T2>(json);
        return new Either<T1, T2>(value1, value2);
    }
}