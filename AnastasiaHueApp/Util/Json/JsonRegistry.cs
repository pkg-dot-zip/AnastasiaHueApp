using System.Diagnostics.CodeAnalysis;

namespace AnastasiaHueApp.Util.Json;

public class JsonRegistry
{
    private readonly Dictionary<Type, Func<string, object>> _parsers = new();

    public void Register<T>(Func<string, T> parser)
    {
        _parsers[typeof(T)] = json => parser(json);
    }

    public T? Parse<T>([StringSyntax(StringSyntaxAttribute.Json)] string json)
    {
        if (json == string.Empty) throw new ArgumentException(nameof(json));
        if (_parsers.TryGetValue(typeof(T), out var parser))
        {
            try
            {
                return (T)parser(json);
            }
            catch (KeyNotFoundException e)
            {
                return default; // This occurs when this is not of this type of object!
            }
        }

        throw new InvalidOperationException($"No parser registered for type {nameof(T)} ({typeof(T)})");
    }
}