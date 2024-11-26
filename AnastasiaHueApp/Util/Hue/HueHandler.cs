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
            return await response.Content.ReadAsEitherAsync<HueLight, ErrorResponse>(registry);
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e, "Code {0}. Returning ErrorResponse.", e.StatusCode);
            return new Either<HueLight, ErrorResponse>(new ErrorResponse(e));
        }
    }

    public async Task<ErrorResponse?> LightSwitch(int index, bool on)
    {
        try
        {
            if (index <= 0) throw new ArgumentOutOfRangeException(nameof(index));
            var response = await HttpClient.PutAsJsonAsync($"{_username}/lights/{index}/state", new
            {
                on,
            });
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsOrNullAsync<ErrorResponse>(registry);
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e, "Code {0}. Returning ErrorResponse.", e.StatusCode);
            return new ErrorResponse(e);
        }
    }

    public async Task<ErrorResponse?> SetColorTo(int index, Color.Color color)
    {
        try
        {
            var response = await HttpClient.PutAsJsonAsync($"{_username}/lights/{index}/state", new
            {
                on = true, // Light is advised to be set on: https://developers.meethue.com/develop/get-started-2/#so-lets-get-started
                sat = color.Saturation,
                bri = color.Brightness,
                hue = color.Hue,
            });
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