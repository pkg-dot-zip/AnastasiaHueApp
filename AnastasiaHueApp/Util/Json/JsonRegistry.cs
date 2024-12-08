using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using AnastasiaHueApp.Models;
using AnastasiaHueApp.Models.Message;
using AnastasiaHueApp.Util.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

        _logger.LogInformation("Got json: {0}", json);

        if (_parsers.TryGetValue(typeof(T), out var parser))
        {
            try
            {
                return (T) parser(json);
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogInformation("Is NOT of type {0}", typeof(T));
                return default; // This occurs when this is not of this type of object, since a definition is missing!
            }
            catch (InvalidOperationException e)
            {
                _logger.LogInformation("Is NOT of type {0}", typeof(T));
                return default; // Can happen when having an array / object mismatch.
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

            // Find the first array element with an error-response.
            JsonElement? firstError = null;

            foreach (var element in doc.RootElement.EnumerateArray())
            {
                if (element.TryGetProperty("error", out var errorElement))
                {
                    firstError = errorElement;
                    break; // Stop after finding the first error. This means other errors will be discarded!
                }
            }

            // NOTE: VERY IMPORTANT! If nothing found we need to throw. Check Parse implementation.
            if (firstError is null) throw new InvalidOperationException("No error response found in the JSON array.");

            return new ErrorResponse
            {
                Address = firstError.Value.GetProperty("address").GetString()!,
                Description = firstError.Value.GetProperty("description").GetString()!,
                Type = firstError.Value.GetProperty("type").ToString()!,
            };
        });

        // UsernameResponse.
        Register<UsernameResponse>(json =>
        {
            var doc = JsonDocument.Parse(json);
            var username = doc.RootElement[0].GetProperty("success").GetProperty("username").GetString();
            return new UsernameResponse {Username = username!};
        });

        // Single HueLight. NOTE: No index is set here. You need to set that later MANUALLY.
        Register<HueLight>(json => JsonConvert.DeserializeObject<HueLight>(json)
                                   ?? throw new InvalidOperationException("Failed to deserialize HueLight."));

        // List<HueLight>.
        Register<List<HueLight>>(json =>
        {
            var lights = JsonConvert.DeserializeObject<Dictionary<string, HueLight>>(json);
            if (lights is null)
                throw new InvalidOperationException("Failed to deserialize list of HueLights.");

            return lights.Select(kvp =>
            {
                var light = kvp.Value;
                light.Id = int.Parse(kvp.Key);
                return light;
            }).ToList();
        });
    }
}