using System.Net.Http.Json;
using AnastasiaHueApp.Models;
using AnastasiaHueApp.Models.Message;
using AnastasiaHueApp.Util.Extensions;
using AnastasiaHueApp.Util.Json;

namespace AnastasiaHueApp.Util.Hue;

public class HueHandler(IJsonRegistry registry)
{
    private static readonly HttpClient HttpClient = new()
    {
        BaseAddress = new Uri("http://localhost/api/"), // NOTE: If using port 80 no port needs to be specified.
    };

    // TODO: Store username here so it's not needed in params.
    public async Task<Either<UsernameResponse, ErrorResponse>> AttemptLinkAsync()
    {
        var response = await HttpClient.PostAsJsonAsync("", new
        {
            devicetype = "my_hue_app#iphone peter", // From: https://developers.meethue.com/develop/get-started-2/
        });
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsEitherAsync<UsernameResponse, ErrorResponse>(registry);
    }

    public async Task<Either<List<HueLight>, ErrorResponse>> GetLights(string username)
    {
        var response = await HttpClient.GetAsync($"{username}/lights");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsEitherAsync<List<HueLight>, ErrorResponse>(registry);
    }

    public async Task<Either<HueLight, ErrorResponse>> GetLight(string username, int index)
    {
        if (index <= 0) throw new ArgumentOutOfRangeException(nameof(index));
        var response = await HttpClient.GetAsync($"{username}/lights/{index}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsEitherAsync<HueLight, ErrorResponse>(registry);
    }
}