﻿namespace AnastasiaHueApp.Models;


// See https://developers.meethue.com/develop/hue-api/lights-api/
public class HueLightState
{
    public bool On { get; set; } // Whether the light should be on.
    public bool Reachable { get; set; } // 	Indicates if a light can be reached by the bridge. TODO: Figure out when this happens and don't let user interact if unreachable.
    public required string Alert { get; init; } // select flashes light once, lselect flashes repeatedly for 15 seconds. TODO: MAKE ENUM.
    public required string Effect { get; init; } // either 'none' or colorloop. TODO: MAKE ENUM.

    // Color.

    public Point XyPoint { get; set; } // Color as XY array.
    public int Hue { get; set; } // In range 0 - 65535. Confusing name. :(
    public int Saturation { get; set; } // In range 0 - 254.
    public int Brightness { get; set; } // In range 0 - 254. 0 is NOT off.
    public required string ColorMode { get; init; } // hs, xy or cy. We will never ever change this.
    public int Ct { get; set; } // White color temperature, 153 (cold) - 500 (warm).
}