namespace AnastasiaHueApp.Models;

public class HueLight
{
    public int Id { get; set; } = int.MinValue; // If set to minvalue many functions will throw, meaning we know we forgot to set this.
    public required string ModelId { get; init; }
    public required string Name { get; init; }
    public required string SwVersion { get; init; } // Firmware.
    public required HueLightState State { get; init; }
    public required string Type { get; init; }
    public required PointSymbol PointSymbol { get; init; }
    public required string UniqueId { get; init; }
}