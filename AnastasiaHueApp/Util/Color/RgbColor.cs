namespace AnastasiaHueApp.Util.Color;

// NOTE: Should only be used by UI (including command parameters). Use the Color class in all other cases.
public class RgbColor
{
    // NOTE: needed for command params in xaml files. DO NOT REMOVE!
    public RgbColor()
    {
    }

    public RgbColor(int r, int g, int b)
    {
        R = r;
        G = g;
        B = b;
    }

    public int R { get; set; }
    public int G { get; set; }
    public int B { get; set; }

    public Color ToColor() => Color.FromRgb(R, G, B);
}
