namespace AnastasiaHueApp.Models;

/// <summary>
/// The dynamic effect of the light. <br/>
/// If set to <see cref="ColorLoop"/>, the light will cycle through all hues using the current brightness and saturation settings.
/// </summary>
public enum HueEffect
{
    None,

    /// <summary>
    /// If set to <see cref="ColorLoop"/>, the light will cycle through all hues using the current brightness and saturation settings.
    /// </summary>
    ColorLoop,
}