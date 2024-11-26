namespace AnastasiaHueApp.Models.Message;

/// <summary>
/// This response can occur upon the following actions (<i>note that some occurrences might be missing from this list</i>): <br/>
/// - Attempting to link to the Hue lights without pressing the physical linking button. <br/>
/// </summary>
public class ErrorResponse
{
    public required string Address { get; init; }
    public required string Description { get; init; }
    public required string Type { get; init; }
}