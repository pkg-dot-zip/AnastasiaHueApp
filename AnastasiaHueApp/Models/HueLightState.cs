namespace AnastasiaHueApp.Models;

// See https://developers.meethue.com/develop/hue-api/lights-api/
public class HueLightState
{
    /// <summary>
    /// <b>On/Off state</b> of the light. <br/>
    /// <b>On</b>=<see langword="true"/>. <br/>
    /// <b>Off</b>=<see langword="false"/>. <br/>
    /// </summary>
    public bool? On { get; set; } = null;

    /// <summary>
    /// Indicates if a light can be reached by the bridge.
    /// </summary>
    public bool? Reachable { get; set; } = null; // TODO: Figure out when this happens and don't let user interact if unreachable.

    /// <inheritdoc cref="HueAlert"/>
    public HueAlert? Alert { get; set; } = null;

    /// <inheritdoc cref="HueEffect"/>
    public HueEffect? Effect { get; set; } = null;

    /// <summary>
    /// The x and y coordinates of a color in CIE color space.
    /// </summary>
    public Point? XyPoint { get; set; } = null;

    /// <summary>
    /// Hue of the light. This is a wrapping <b>value between 0 and 65535</b>. <br/>
    /// Note, that <see cref="Hue"/>/<see cref="Saturation"/> values are hardware dependent which means that programming two devices with the same value does <b>not</b> guarantee that they will be the same color.
    /// </summary>
    public int? Hue { get; set; } = null;

    /// <summary>
    /// Saturation of the light. 254 is the most saturated (<i>colored</i>) and 0 is the least saturated (<i>white</i>). <br/>
    /// Ranges from <b>0 to 254</b>.
    /// </summary>
    public int? Saturation { get; set; } = null;

    /// <summary>
    /// Brightness of the light. This is a scale from the minimum brightness the light is capable of, 1, to the maximum capable brightness, 254.
    /// Ranges from <b>1 to 254</b>.
    /// </summary>
    public int? Brightness { get; set; } = null; // TODO: Check if 0 is an allowed value, since in all our checks we check from 0 to 254 instead of 1 to 254.

    /// <inheritdoc cref="HueColorMode"/>
    public HueColorMode? ColorMode { get; set; } = null;

    /// <summary>
    /// The Mired Color temperature of the light (warm white color). 2012 connected lights are capable of 153 (<i>6500K</i>) to 500 (<i>2000K</i>). <br/>
    /// Ranges from <b>153 to 500</b>.
    /// </summary>
    public int? Ct { get; set; } = null;
}