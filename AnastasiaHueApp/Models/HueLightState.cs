namespace AnastasiaHueApp.Models;

public class HueLightState
{
    public bool On { get; set; } // Whether the light should be on.
    public bool Reachable { get; set; } // TODO: Figure out what this is.
    public required string Alert { get; init; } // select flashes light once, lselect flashes repeatedly for 10 seconds. TODO: MAKE ENUM.
    public required string Effect { get; init; } // TODO: Figure out what this is.

    // Color.

    public Point XyPoint { get; set; } // Color as XY array.
    public int Hue { get; set; } // In range 0 - 65535. Confusing name. :(
    public int Saturation { get; set; } // In range 0 - 254.
    public int Brightness { get; set; } // In range 0 - 254. 0 is NOT off.
    public required string ColorMode { get; init; }
    public int Ct { get; set; } // White color temperature, 154 (cold) - 500 (warm).
}