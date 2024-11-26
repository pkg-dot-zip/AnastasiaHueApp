using System;
using System.Net.Http.Json;
using AnastasiaHueApp.Models;
using AnastasiaHueApp.Models.Message;
using AnastasiaHueApp.Util.Extensions;
using AnastasiaHueApp.Util.Json;
using Microsoft.Extensions.Logging;

namespace AnastasiaHueApp.Util.Hue;

public class HueHandler(ILogger<HueHandler> logger, IJsonRegistry registry)
{
    private static readonly HttpClient HttpClient = new()
    {
        BaseAddress = new Uri("http://localhost/api/"), // NOTE: If using port 80 no port needs to be specified.
    };

    // TODO: Store username here so it's not needed in params.
    public async Task<Either<UsernameResponse, ErrorResponse>> AttemptLinkAsync()
    {
        try
        {
            var response = await HttpClient.PostAsJsonAsync("", new
            {
                devicetype = "my_hue_app#iphone peter", // From: https://developers.meethue.com/develop/get-started-2/
            });
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsEitherAsync<UsernameResponse, ErrorResponse>(registry);
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e, "Code {0}. Returning ErrorResponse.", e.StatusCode);
            return new Either<UsernameResponse, ErrorResponse>(new ErrorResponse(e));
        }
    }

    public async Task<Either<List<HueLight>, ErrorResponse>> GetLights(string username)
    {
        try
        {
            var response = await HttpClient.GetAsync($"{username}/lights");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsEitherAsync<List<HueLight>, ErrorResponse>(registry);
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e, "Code {0}. Returning ErrorResponse.", e.StatusCode);
            return new Either<List<HueLight>, ErrorResponse>(new ErrorResponse(e));
        }
    }

    public async Task<Either<HueLight, ErrorResponse>> GetLight(string username, int index)
    {
        try
        {
            if (index <= 0) throw new ArgumentOutOfRangeException(nameof(index));
            var response = await HttpClient.GetAsync($"{username}/lights/{index}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsEitherAsync<HueLight, ErrorResponse>(registry);
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e, "Code {0}. Returning ErrorResponse.", e.StatusCode);
            return new Either<HueLight, ErrorResponse>(new ErrorResponse(e));
        }
    }

    public async Task<ErrorResponse?> LightSwitch(string username, int index, bool turnOn)
    {
        try
        {
            if (index <= 0) throw new ArgumentOutOfRangeException(nameof(index));
            var response = await HttpClient.PutAsJsonAsync($"{username}/lights/{index}/state", new
            {
                On = turnOn,
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