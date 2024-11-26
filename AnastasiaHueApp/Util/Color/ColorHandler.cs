namespace AnastasiaHueApp.Util.Color;

public class ColorHandler
{
    private const float Tolerance = 1.0f;

    // NOTE: This method is partially written by AI. 🤖 RGB ranges from 0-255.
    public static Color RGBToHueHSB(int r, int g, int b)
    {
        // Normalize RGB values to range 0-1.
        double rNorm = r / 255.0;
        double gNorm = g / 255.0;
        double bNorm = b / 255.0;

        // Find min and max values of R, G, B.
        double max = Math.Max(rNorm, Math.Max(gNorm, bNorm));
        double min = Math.Min(rNorm, Math.Min(gNorm, bNorm));
        double delta = max - min;

        // Calculate Hue.
        double h = 0;
        if (delta > 0)
        {
            if (Math.Abs(max - rNorm) < Tolerance)
            {
                h = (gNorm - bNorm) / delta;
            }
            else if (Math.Abs(max - gNorm) < Tolerance)
            {
                h = 2 + (bNorm - rNorm) / delta;
            }
            else if (Math.Abs(max - bNorm) < Tolerance)
            {
                h = 4 + (rNorm - gNorm) / delta;
            }
        }

        h *= 60;
        if (h < 0) h += 360;

        // Convert Hue to range 0-65535.
        int hue = (int)(h / 360 * 65535);
            
        // Calculate Saturation.
        double s = max == 0 ? 0 : delta / max;

        // Convert Saturation to range 0-254.
        int saturation = (int)(s * 254);

        // Calculate Brightness.
        double v = max;

        // Convert Brightness to range 0-254.
        int brightness = (int)(v * 254);

        return Color.FromHueHsb(hue, saturation, brightness);
    }
}