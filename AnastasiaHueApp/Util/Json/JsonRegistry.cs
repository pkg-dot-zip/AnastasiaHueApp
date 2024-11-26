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
            // TODO: If unsuccessful, return null.

            return (T)parser(json);
        }

        throw new InvalidOperationException($"No parser registered for type {nameof(T)} ({typeof(T)})");
    }
}