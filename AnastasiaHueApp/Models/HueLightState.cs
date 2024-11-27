namespace AnastasiaHueApp.Models;


// See https://developers.meethue.com/develop/hue-api/lights-api/
public class HueLightState
{
    public bool? On { get; set; } = null; // Whether the light should be on.
    public bool? Reachable { get; set; } = null; // 	Indicates if a light can be reached by the bridge. TODO: Figure out when this happens and don't let user interact if unreachable.
    public string? Alert { get; set; } = null; // select flashes light once, lselect flashes repeatedly for 15 seconds. TODO: MAKE ENUM.
    public string? Effect { get; set; } = null; // either 'none' or colorloop. TODO: MAKE ENUM.

    // Color.
    public Point? XyPoint { get; set; } = null; // Color as XY array.
    public int? Hue { get; set; } = null; // In range 0 - 65535. Confusing name. :(
    public int? Saturation { get; set; } = null; // In range 0 - 254.
    public int? Brightness { get; set; } = null; // In range 0 - 254. 0 is NOT off.
    public string? ColorMode { get; set; } = null; // hs, xy or cy. We will never ever change this.
    public int? Ct { get; set; } = null; // White color temperature, 153 (cold) - 500 (warm).
}