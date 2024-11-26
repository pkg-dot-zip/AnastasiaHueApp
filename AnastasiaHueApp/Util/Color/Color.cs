using AnastasiaHueApp.Util.Extensions;

namespace AnastasiaHueApp.Util.Color;

/// <summary>
/// Simple color class that contains values for hue, saturation and brightness (<a href="https://developers.meethue.com/develop/get-started-2/#so-lets-get-started">hsb</a>).
/// Allows creation from multiple color models, such as <see cref="FromRgb"/>.
/// </summary>
public class Color
{
    public int Hue { get; private set; }
    public int Saturation { get; private set; }
    public int Brightness { get; private set; }

    private Color(int hue, int saturation, int brightness)
    {
        if (!hue.IsInRange(0, 65535)) throw new ArgumentOutOfRangeException(nameof(hue));
        if (!saturation.IsInRange(0, 254)) throw new ArgumentOutOfRangeException(nameof(saturation));
        if (!brightness.IsInRange(0, 254)) throw new ArgumentOutOfRangeException(nameof(brightness));

        Hue = hue;
        Saturation = saturation;
        Brightness = brightness;
    }

    /// <summary>
    /// Allows initialization of <see cref="Color"/> from the <a href="https://developers.meethue.com/develop/get-started-2/#so-lets-get-started">Philips Hue HSB model</a>.
    /// </summary>
    /// <param name="hue">Must be between 0-65535.</param>
    /// <param name="saturation">Must be between 0-254.</param>
    /// <param name="brightness">Must be between 0-254.</param>
    /// <returns>A new instance of <see cref="Color"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Color FromHueHsb(int hue, int saturation, int brightness)
    {
        if (!hue.IsInRange(0, 65535)) throw new ArgumentOutOfRangeException(nameof(hue));
        if (!saturation.IsInRange(0, 254)) throw new ArgumentOutOfRangeException(nameof(saturation));
        if (!brightness.IsInRange(0, 254)) throw new ArgumentOutOfRangeException(nameof(brightness));
        return new Color(hue, saturation, brightness);
    }

    /// <summary>
    /// Allows initialization of <see cref="Color"/> from the <a href="https://en.wikipedia.org/wiki/RGB_color_model">RGB model</a>.
    /// </summary>
    /// <param name="red">Must be between 0-255.</param>
    /// <param name="green">Must be between 0-255.</param>
    /// <param name="blue">Must be between 0-255.</param>
    /// <returns>A new instance of <see cref="Color"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Color FromRgb(int red, int green, int blue)
    {
        if (!red.IsInRange(0, 255)) throw new ArgumentOutOfRangeException(nameof(red));
        if (!green.IsInRange(0, 255)) throw new ArgumentOutOfRangeException(nameof(green));
        if (!blue.IsInRange(0, 255)) throw new ArgumentOutOfRangeException(nameof(blue));
        return ColorHandler.RGBToHueHSB(red, green, blue);
    }
}