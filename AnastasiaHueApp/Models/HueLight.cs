namespace AnastasiaHueApp.Models;

public class HueLight
{
    public required string ModelId { get; init; }
    public required string Name { get; init; }
    public required string SwVersion { get; init; } // Firmware.
    public required HueLightState State { get; init; }
    public required string Type { get; init; }
    public required PointSymbol PointSymbol { get; init; }
    public required string UniqueId { get; init; }
}