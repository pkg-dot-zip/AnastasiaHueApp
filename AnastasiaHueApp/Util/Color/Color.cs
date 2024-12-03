using AnastasiaHueApp.Util.Extensions;

namespace AnastasiaHueApp.Util.Color;

/// <summary>
/// Simple color class that contains values for hue, saturation and brightness (<a href="https://developers.meethue.com/develop/get-started-2/#so-lets-get-started">hsb</a>).
/// Allows creation from multiple color models, such as <see cref="FromRgb"/>.
/// </summary>
public class Color
{
    /// <summary>
    /// Hue directly given to the Hue Light. <br/>
    /// Ranges from <b>0 to 65535</b>.
    /// </summary>
    public int Hue { get; private set; }

    /// <summary>
    /// Saturation directly given to the Hue Light. <br/>
    /// Ranges from <b>0 to 254</b>.
    /// </summary>
    public int Saturation { get; private set; }

    /// <summary>
    /// Brightness directly given to the Hue Light. <br/>
    /// Ranges from <b>0 to 254</b>.
    /// </summary>
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

    // NOTE: This method is partially written by AI. 🤖
    public Microsoft.Maui.Graphics.Color ToMauiColor()
    {
        // Normalize HSB values.
        float h = Hue / 65535f * 360f;
        float s = Saturation / 254f;
        float b = Brightness / 254f;

        // Convert HSB to RGB.
        float c = b * s; // Chroma.
        float x = c * (1 - Math.Abs((h / 60) % 2 - 1));
        float m = b - c;

        float r = 0, g = 0, bl = 0;

        switch (h)
        {
            case >= 0 and < 60:
                r = c;
                g = x;
                bl = 0;
                break;
            case >= 60 and < 120:
                r = x;
                g = c;
                bl = 0;
                break;
            case >= 120 and < 180:
                r = 0;
                g = c;
                bl = x;
                break;
            case >= 180 and < 240:
                r = 0;
                g = x;
                bl = c;
                break;
            case >= 240 and < 300:
                r = x;
                g = 0;
                bl = c;
                break;
            case >= 300 and <= 360:
                r = c;
                g = 0;
                bl = x;
                break;
        }

        // Adjust RGB values by adding m.
        r += m;
        g += m;
        bl += m;

        // Convert to Maui Color.
        return Microsoft.Maui.Graphics.Color.FromRgb((int)(r * 255), (int)(g * 255), (int)(bl * 255));
    }


    /// <summary>
    /// Allows initialization of <see cref="Color"/> from the <a href="https://developers.meethue.com/develop/get-started-2/#so-lets-get-started">Philips Hue HSB model</a>.
    /// </summary>
    /// <param name="hue">Must be between <b>0-65535</b>.</param>
    /// <param name="saturation">Must be between <b>0-254</b>.</param>
    /// <param name="brightness">Must be between <b>0-254</b>.</param>
    /// <returns>A new instance of <see cref="Color"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Color FromHueHsb(int hue, int saturation, int brightness)
    {
        if (!hue.IsInRange(0, 65535)) throw new ArgumentOutOfRangeException(nameof(hue));
        if (!saturation.IsInRange(0, 254)) throw new ArgumentOutOfRangeException(nameof(saturation));
        if (!brightness.IsInRange(0, 254)) throw new ArgumentOutOfRangeException(nameof(brightness));
        return new Color(hue, saturation, brightness);
    }

    public static Color FromHueHsb(int? hue, int? saturation, int? brightness) =>
        FromHueHsb((int)hue!, (int)saturation!, (int)brightness!);

    // NOTE: This method is partially written by AI. 🤖 RGB ranges from 0-255.
    /// <summary>
    /// Allows initialization of <see cref="Color"/> from the <a href="https://en.wikipedia.org/wiki/RGB_color_model">RGB model</a>.
    /// </summary>
    /// <param name="red">Must be between <b>0-255</b>.</param>
    /// <param name="green">Must be between <b>0-255</b>.</param>
    /// <param name="blue">Must be between <b>0-255</b>.</param>
    /// <returns>A new instance of <see cref="Color"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Color FromRgb(int red, int green, int blue)
    {
        if (!red.IsInRange(0, 255)) throw new ArgumentOutOfRangeException(nameof(red));
        if (!green.IsInRange(0, 255)) throw new ArgumentOutOfRangeException(nameof(green));
        if (!blue.IsInRange(0, 255)) throw new ArgumentOutOfRangeException(nameof(blue));


        const float tolerance = 1.0f;

        // Normalize RGB values to range 0-1.
        double rNorm = red / 255.0;
        double gNorm = green / 255.0;
        double bNorm = blue / 255.0;

        // Find min and max values of R, G, B.
        double max = Math.Max(rNorm, Math.Max(gNorm, bNorm));
        double min = Math.Min(rNorm, Math.Min(gNorm, bNorm));
        double delta = max - min;

        // Calculate Hue.
        double h = 0;
        if (delta > 0)
        {
            if (Math.Abs(max - rNorm) < tolerance)
            {
                h = (gNorm - bNorm) / delta;
            }
            else if (Math.Abs(max - gNorm) < tolerance)
            {
                h = 2 + (bNorm - rNorm) / delta;
            }
            else if (Math.Abs(max - bNorm) < tolerance)
            {
                h = 4 + (rNorm - gNorm) / delta;
            }
        }

        h *= 60;
        if (h < 0) h += 360;

        int hue = (int)(h / 360 * 65535); // Convert Hue to range 0-65535.
        double s = max == 0 ? 0 : delta / max; // Calculate Saturation.
        int saturation = (int)(s * 254); // Convert Saturation to range 0-254.
        double v = max; // Calculate Brightness.
        int brightness = (int)(v * 254); // Convert Brightness to range 0-254.
        return FromHueHsb(hue, saturation, brightness);
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

    public override string ToString() => $"[{Hue}, {Brightness}, {Saturation}]";

    #region Equals
    protected bool Equals(Color other) =>
        Hue == other.Hue && Saturation == other.Saturation && Brightness == other.Brightness;

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Color)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Hue, Saturation, Brightness);

    #endregion
}