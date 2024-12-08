namespace AnastasiaHueApp.Models;

/// <summary>
/// Indicates the color mode in which the light is working, this is the last command type it received.
/// This parameter is only present when the light supports at least one of the values.
/// </summary>
public enum HueColorMode
{
    /// <summary>
    /// Hue and Saturation <b>(Default)</b>.
    /// </summary>
    Hs,

    /// <summary>
    /// XY.
    /// </summary>
    Xy,

    /// <summary>
    /// Color Temperature.
    /// </summary>
    Ct,
}