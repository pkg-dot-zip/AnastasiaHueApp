using System.Net.Http.Json;
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

    public async Task<Either<UsernameResponse, ErrorResponse>> AttemptLinkAsync()
    {
        var response = await HttpClient.PostAsJsonAsync("", new
        {
            devicetype = "my_hue_app#iphone peter", // From: https://developers.meethue.com/develop/get-started-2/
        });
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsEitherAsync<UsernameResponse, ErrorResponse>(registry);
    }
}