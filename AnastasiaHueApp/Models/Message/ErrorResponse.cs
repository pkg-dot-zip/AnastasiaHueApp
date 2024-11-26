namespace AnastasiaHueApp.Models.Message;

/// <summary>
/// This response can occur upon the following actions (<i>note that some occurrences might be missing from this list</i>): <br/>
/// - Attempting to link to the Hue lights without pressing the physical linking button. <br/>
/// - Attempting to retrieve information about a light that is not there (api/lights/4 when there are only 3 lights). <br/>
/// - Upon receiving a network error (those are fabricated by us and not returned by the api). <br/>
/// </summary>
public class ErrorResponse
{
    public string Address { get; init; }
    public string Description { get; init; }
    public string Type { get; init; }

    public ErrorResponse()
    {
    }

    // Used to easily create popups upon network exceptions. These are fabricated by us and not retrieved from json.
    public ErrorResponse(HttpRequestException e)
    {
        Address = string.Empty;
        Description = $"NETWORK ERROR ({e.StatusCode})";
        Type = e.StatusCode.ToString()!;
    }
}