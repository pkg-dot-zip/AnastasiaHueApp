namespace AnastasiaHueApp.Models.Message;

public class LightsResponse
{
    public List<HueLight> Lights { get; set; } = new();
}