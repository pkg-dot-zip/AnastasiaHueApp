using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using AnastasiaHueApp.Models;
using AnastasiaHueApp.Models.Message;
using AnastasiaHueApp.Util.Extensions;
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

        _logger.LogInformation("Got json: {0}", json);

        if (_parsers.TryGetValue(typeof(T), out var parser))
        {
            try
            {
                return (T)parser(json);
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
            return new UsernameResponse { Username = username! };
        });

        // NOTE: No index is passed here. You'd have to set that later MANUALLY.
        Register<HueLight>(json =>
        {
            var doc = JsonDocument.Parse(json);
            return GetHueLight(doc.RootElement);
        });

        // List<HueLight>. 
        Register<List<HueLight>>(json =>
        {
            var doc = JsonDocument.Parse(json);
            return doc.RootElement.EnumerateObject()
                .Select(prop => GetHueLight(prop.Value, int.Parse(prop.Name))).ToList();
        });
    }

    // Helper method since multiple parsers need to create instances of HueLight.
    // NOTE: This one is partially written by AI 🤖, since I couldn't be bothered writing all the json parsing myself.
    private HueLight GetHueLight(JsonElement element, int? id = null)
    {
        return new HueLight
        {
            Id = id ?? int.MinValue,
            ModelId = element.GetProperty("modelid").GetString()!,
            Name = element.GetProperty("name").GetString()!,
            SwVersion = element.GetProperty("swversion").GetString()!,
            State = new HueLightState
            {
                On = element.GetProperty("state").GetProperty("on").GetBoolean(),
                Reachable = element.GetProperty("state").GetProperty("reachable").GetBoolean(),
                Alert = element.GetProperty("state").GetProperty("alert").GetEnum<HueAlert>(),
                Effect = element.GetProperty("state").GetProperty("effect").GetEnum<HueEffect>(),
                XyPoint = new Point
                {
                    X = element.GetProperty("state").GetProperty("xy")[0].GetDouble(),
                    Y = element.GetProperty("state").GetProperty("xy")[1].GetDouble()
                },
                Hue = element.GetProperty("state").GetProperty("hue").GetInt32(),
                Saturation = element.GetProperty("state").GetProperty("sat").GetInt32(),
                Brightness = element.GetProperty("state").GetProperty("bri").GetInt32(),
                ColorMode = element.GetProperty("state").GetProperty("colormode").GetEnum<HueColorMode>(),
                Ct = element.GetProperty("state").GetProperty("ct").GetInt32()
            },
            Type = element.GetProperty("type").GetString()!,
            PointSymbol = new PointSymbol
            {
                Symbol1 = element.GetProperty("pointsymbol").GetProperty("1").GetString()!,
                Symbol2 = element.GetProperty("pointsymbol").GetProperty("2").GetString()!,
                Symbol3 = element.GetProperty("pointsymbol").GetProperty("3").GetString()!,
                Symbol4 = element.GetProperty("pointsymbol").GetProperty("4").GetString()!,
                Symbol5 = element.GetProperty("pointsymbol").GetProperty("5").GetString()!,
                Symbol6 = element.GetProperty("pointsymbol").GetProperty("6").GetString()!,
                Symbol7 = element.GetProperty("pointsymbol").GetProperty("7").GetString()!,
                Symbol8 = element.GetProperty("pointsymbol").GetProperty("8").GetString()!,
            },
            UniqueId = element.GetProperty("uniqueid").GetString()!
        };
    }
}