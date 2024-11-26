using AnastasiaHueApp.Util.Extensions;

namespace AnastasiaHueApp.Util.Color;

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

    public static Color FromHueHsb(int hue, int saturation, int brightness) => new(hue, saturation, brightness);
    public static Color FromRgb(int red, int green, int blue) => ColorHandler.RGBToHueHSB(red, green, blue);
}