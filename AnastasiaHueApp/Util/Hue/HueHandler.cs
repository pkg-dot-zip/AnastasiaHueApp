using System.Dynamic;
using System.Net.Http.Json;
using AnastasiaHueApp.Models;
using AnastasiaHueApp.Models.Message;
using AnastasiaHueApp.Util.Extensions;
using AnastasiaHueApp.Util.Json;
using AnastasiaHueApp.Util.Preferences;
using Microsoft.Extensions.Logging;

namespace AnastasiaHueApp.Util.Hue;

public class HueHandler(ILogger<HueHandler> logger, IJsonRegistry registry, IPreferencesHandler preferencesHandler)
    : IHueHandler
{
    private static readonly HttpClient HttpClient = new()
    {
#if ANDROID
        BaseAddress =
 new Uri("http://10.0.2.2/api/"), // NOTE: Emulator. If using port 80 no port needs to be specified.
        //BaseAddress = new Uri("http://192.168.1.179/api/"), // NOTE: Hardware. If using port 80 no port needs to be specified
#elif WINDOWS
        BaseAddress =
 new Uri("http://localhost/api/"), // NOTE: Emulator. If using port 80 no port needs to be specified.
        //BaseAddress = new Uri("http://192.168.1.179/api/"), // NOTE: Hardware. If using port 80 no port needs to be specified.
#else

#endif
    };

    /// <inheritdoc />
    public async Task<bool> IsOldConnectionValid()
    {
        // TODO: What happens if no username was set before this?!

        try
        {
            // Make a lightsCall with stored username.
            var username = preferencesHandler.RetrieveUsername();
            var response = await HttpClient.GetAsync($"{username}/lights");
            var either = await response.Content.ReadAsEitherAsync<List<HueLight>, ErrorResponse>(registry);

            if (either.IsType<ErrorResponse>(out var error))
            {
                if (error!.Type != "1") throw new InvalidOperationException(); // If not unauthorized user then something really went wrong. :(
                return false;
            }

            if (either.IsType<List<HueLight>>(out _)) return true;
        }
        catch (HttpRequestException e)
        {
            return false;
        }

        return false;
    }

    /// <inheritdoc />
    public async Task<Either<UsernameResponse, ErrorResponse>> AttemptLinkAsync()
    {
        try
        {
            var response = await HttpClient.PostAsJsonAsync("", new
            {
                devicetype = "my_hue_app#iphone peter", // From: https://developers.meethue.com/develop/get-started-2/
            });
            response.EnsureSuccessStatusCode();
            var either = await response.Content.ReadAsEitherAsync<UsernameResponse, ErrorResponse>(registry);

            if (either.IsType<UsernameResponse>(out var username)) preferencesHandler.SetUsername(username!.Username);
            return either;
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e, "Code {0}. Returning ErrorResponse.", e.StatusCode);
            return new Either<UsernameResponse, ErrorResponse>(new ErrorResponse(e));
        }
    }

    /// <inheritdoc />
    public async Task<Either<List<HueLight>, ErrorResponse>> GetLights()
    {
        if (!IsAllowedToMakeCall(out var error)) return new Either<List<HueLight>, ErrorResponse>(error!);

        try
        {
            var response = await HttpClient.GetAsync($"{preferencesHandler.RetrieveUsername()}/lights");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsEitherAsync<List<HueLight>, ErrorResponse>(registry);
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e, "Code {0}. Returning ErrorResponse.", e.StatusCode);
            return new Either<List<HueLight>, ErrorResponse>(new ErrorResponse(e));
        }
    }

    /// <inheritdoc />
    public async Task<Either<HueLight, ErrorResponse>> GetLight(int index)
    {
        if (!IsAllowedToMakeCall(out var error)) return new Either<HueLight, ErrorResponse>(error!);

        try
        {
            if (index <= 0) throw new ArgumentOutOfRangeException(nameof(index));
            var response = await HttpClient.GetAsync($"{preferencesHandler.RetrieveUsername()}/lights/{index}");
            response.EnsureSuccessStatusCode();

            // Here we set the light id / index, since that is not returned in the json. :(
            var either = await response.Content.ReadAsEitherAsync<HueLight, ErrorResponse>(registry);

            if (either.IsType<HueLight>(out var light))
            {
                light!.Id = index;
                return new Either<HueLight, ErrorResponse>(light);
            }

            return either;
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e, "Code {0}. Returning ErrorResponse.", e.StatusCode);
            return new Either<HueLight, ErrorResponse>(new ErrorResponse(e));
        }
    }

    /// <inheritdoc />
    public async Task<ErrorResponse?> LightSwitch(int index, bool on)
    {
        if (index <= 0) throw new ArgumentOutOfRangeException(nameof(index));
        return await SetLightState(index, new HueLightState() { On = on });
    }

    /// <inheritdoc />
    public async Task<ErrorResponse?> SetColorTo(int index, Color.Color color)
    {
        return await SetLightState(index, new HueLightState()
        {
            On = true, // Light is advised to be set on: https://developers.meethue.com/develop/get-started-2/#so-lets-get-started
            Saturation = color.Saturation,
            Brightness = color.Brightness,
            Hue = color.Hue,
        });
    }

    /// <inheritdoc />
    public async Task<ErrorResponse?> MakeLightBlink(int index)
    {
        return await SetLightState(index, new HueLightState { Alert = HueAlert.LSelect });
    }

    /// <inheritdoc />
    public async Task<ErrorResponse?> MakeLightColorLoop(int index)
    {
        return await SetLightState(index, new HueLightState { Effect = HueEffect.ColorLoop });
    }

    /// <inheritdoc />
    public async Task<ErrorResponse?> SetLightState(int index, HueLightState state)
    {
        if (!IsAllowedToMakeCall(out var error)) return error;

        try
        {
            // Crazy hack to allow us to check whether a property of state is null before putting it into a dynamic object.
            dynamic payload = new ExpandoObject();
            var payloadDict = (IDictionary<string, object>)payload;

            if (state.Alert is not null) payloadDict["alert"] = state.Alert.GetName();
            if (state.Brightness is not null) payloadDict["bri"] = state.Brightness;
            // if (state.ColorMode is not null) payloadDict["colormode"] = state.ColorMode.GetName(); // NOTE: We will never change the colorMode, hence we don't put it in the payload. 
            // if (state.Ct is not null) payloadDict["ct"] = state.Ct;  // NOTE: We will never change the color temperature, hence we don't put it in the payload. 
            if (state.Effect is not null) payloadDict["effect"] = state.Effect.GetName();
            if (state.Hue is not null) payloadDict["hue"] = state.Hue;
            if (state.On is not null) payloadDict["on"] = state.On;
            if (state.Reachable is not null) payloadDict["reachable"] = state.Reachable;
            if (state.Saturation is not null) payloadDict["sat"] = state.Saturation;
            // if (state.XyPoint is not null) payloadDict["xy"] = state.XyPoint; // NOTE: We don't set XyPoint since we will never use it.

            var response =
                await HttpClient.PutAsJsonAsync($"{preferencesHandler.RetrieveUsername()}/lights/{index}/state",
                    payloadDict);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsOrNullAsync<ErrorResponse>(registry);
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e, "Code {0}. Returning ErrorResponse.", e.StatusCode);
            return new ErrorResponse(e);
        }
    }

    private bool IsAllowedToMakeCall(out ErrorResponse? error)
    {
        if (preferencesHandler.RetrieveUsername() is null or "")
        {
            error = new ErrorResponse
            {
                Description = "Hue Lights are not linked. Please link before making calls.",
                Address = string.Empty,
                Type = "Fabricated"
            };
            return false;
        }

        error = null;
        return true;
    }
}