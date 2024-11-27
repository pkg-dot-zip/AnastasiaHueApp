using System.Dynamic;
using System.Net.Http.Json;
using AnastasiaHueApp.Models;
using AnastasiaHueApp.Models.Message;
using AnastasiaHueApp.Util.Extensions;
using AnastasiaHueApp.Util.Json;
using Microsoft.Extensions.Logging;

namespace AnastasiaHueApp.Util.Hue;

public class HueHandler(ILogger<HueHandler> logger, IJsonRegistry registry) : IHueHandler
{
    private static readonly HttpClient HttpClient = new()
    {
        BaseAddress = new Uri("http://localhost/api/"), // NOTE: If using port 80 no port needs to be specified.
        // BaseAddress = new Uri("http://192.168.1.179/api/"), // NOTE: If using port 80 no port needs to be specified.
    };

    private static string? _username = null;

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

            if (either.IsType<UsernameResponse>(out var username)) _username = username!.Username;
            return either;
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e, "Code {0}. Returning ErrorResponse.", e.StatusCode);
            return new Either<UsernameResponse, ErrorResponse>(new ErrorResponse(e));
        }
    }

    public async Task<Either<List<HueLight>, ErrorResponse>> GetLights()
    {
        try
        {
            var response = await HttpClient.GetAsync($"{_username}/lights");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsEitherAsync<List<HueLight>, ErrorResponse>(registry);
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e, "Code {0}. Returning ErrorResponse.", e.StatusCode);
            return new Either<List<HueLight>, ErrorResponse>(new ErrorResponse(e));
        }
    }

    public async Task<Either<HueLight, ErrorResponse>> GetLight(int index)
    {
        try
        {
            if (index <= 0) throw new ArgumentOutOfRangeException(nameof(index));
            var response = await HttpClient.GetAsync($"{_username}/lights/{index}");
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

    public async Task<ErrorResponse?> LightSwitch(int index, bool on)
    {
        if (index <= 0) throw new ArgumentOutOfRangeException(nameof(index));
        return await SetLightState(index, new HueLightState() {On = on});
    }

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

    public async Task<ErrorResponse?> MakeLightBlink(int index)
    {
        return await SetLightState(index, new HueLightState {Alert = HueAlert.LSelect});
    }

    public async Task<ErrorResponse?> MakeLightColorLoop(int index)
    {
        return await SetLightState(index, new HueLightState {Effect = HueEffect.ColorLoop});
    }

    public async Task<ErrorResponse?> SetLightState(int index, HueLightState state)
    {
        try
        {
            // Crazy hack to allow us to check whether a property of state is null before putting it into a dynamic object.
            dynamic payload = new ExpandoObject();
            var payloadDict = (IDictionary<string, object>) payload;

            if (state.Alert is not null) payloadDict["alert"] = state.Alert;
            if (state.Brightness is not null) payloadDict["bri"] = state.Brightness;
            if (state.ColorMode is not null) payloadDict["colormode"] = state.ColorMode;
            if (state.Ct is not null) payloadDict["ct"] = state.Ct;
            if (state.Effect is not null) payloadDict["effect"] = state.Effect;
            if (state.Hue is not null) payloadDict["hue"] = state.Hue;
            if (state.On is not null) payloadDict["on"] = state.On;
            if (state.Reachable is not null) payloadDict["reachable"] = state.Reachable;
            if (state.Saturation is not null) payloadDict["sat"] = state.Saturation;
            if (state.XyPoint is not null) payloadDict["xy"] = state.XyPoint;

            var response = await HttpClient.PutAsJsonAsync($"{_username}/lights/{index}/state", payloadDict);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsOrNullAsync<ErrorResponse>(registry);
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e, "Code {0}. Returning ErrorResponse.", e.StatusCode);
            return new ErrorResponse(e);
        }
    }
}