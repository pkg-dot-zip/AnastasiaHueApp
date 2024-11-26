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

    // NOTE: This method is partially written by AI. 🤖
    /// <summary>
    /// Allows initialization of <see cref="Color"/> from the <a href="https://en.wikipedia.org/wiki/Hexadecimal">HEX model</a>.
    /// Please note that although <i>alpha</i> is calculated, it will not be passed on to the new instance of <see cref="Color"/>.
    /// </summary>
    /// <param name="hex">Hexadecimal number to convert into a <see cref="Color"/>.</param>
    /// <returns>A new instance of <see cref="Color"/>.</returns>
    public static Color FromHex(uint hex)
    {
        int r, g, b, a;

        if ((hex & 0xFF000000) != 0) // Check if the hex includes alpha (e.g., 0xRRGGBBAA).
        {
            a = (int)((hex >> 24) & 0xFF);
            r = (int)((hex >> 16) & 0xFF);
            g = (int)((hex >> 8) & 0xFF);
            b = (int)(hex & 0xFF);
        }
        else // If no alpha is provided, assume 255 (fully opaque).
        {
            a = 255;
            r = (int)((hex >> 16) & 0xFF);
            g = (int)((hex >> 8) & 0xFF);
            b = (int)(hex & 0xFF);
        }

        return FromRgb(r, g, b);
    }
}