using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using AnastasiaHueApp.Models.Message;
using Microsoft.Extensions.Logging;

namespace AnastasiaHueApp.Util.Json;

/// <inheritdoc />
public class JsonRegistry : IJsonRegistry
{
    private readonly Dictionary<Type, Func<string, object>> _parsers = new();
    private readonly ILogger<JsonRegistry> _logger;

    public JsonRegistry(ILogger<JsonRegistry> logger)
    {
        _logger = logger;
        RegisterAll();
    }

    /// <inheritdoc />
    public void Register<T>(Func<string, T> parser)
    {
        _parsers[typeof(T)] = json => parser(json);
        _logger.LogInformation("Registered parser for {0}.", typeof(T).Name);
    }

    /// <inheritdoc />
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

    // Contains all parsers we use in the default application.
    // Note that this registered here in the implementation itself, meaning that a mock of IJsonRegistry will not contain said definitions.
    private void RegisterAll()
    {
        // ErrorResponse.
        Register<ErrorResponse>(json =>
        {
            var doc = JsonDocument.Parse(json);
            var error = doc.RootElement[0].GetProperty("error");
            return new ErrorResponse
            {
                Address = error.GetProperty("address").GetString()!,
                Description = error.GetProperty("description").GetString()!,
                Type = error.GetProperty("type").GetString()!,
            };
        });

        // UsernameResponse.
        Register<UsernameResponse>(json =>
        {
            var doc = JsonDocument.Parse(json);
            var username = doc.RootElement[0].GetProperty("success").GetProperty("username").GetString();
            return new UsernameResponse { Username = username! };
        });
    }
}