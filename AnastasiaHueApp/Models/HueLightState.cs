namespace AnastasiaHueApp.Models;

// See https://developers.meethue.com/develop/hue-api/lights-api/
public class HueLightState
{
    public bool? On { get; set; } = null; // Whether the light should be on.
    public bool? Reachable { get; set; } = null; // Indicates if a light can be reached by the bridge. TODO: Figure out when this happens and don't let user interact if unreachable.
    public HueAlert? Alert { get; set; } = null;
    public HueEffect? Effect { get; set; } = null;

    // Color.
    public Point? XyPoint { get; set; } = null; // Color as XY array.
    public int? Hue { get; set; } = null; // In range 0 - 65535. Confusing name. :(
    public int? Saturation { get; set; } = null; // In range 0 - 254.
    public int? Brightness { get; set; } = null; // In range 0 - 254. 0 is NOT off.
    public HueColorMode? ColorMode { get; set; } = null;
    public int? Ct { get; set; } = null; // White color temperature, 153 (cold) - 500 (warm).
}