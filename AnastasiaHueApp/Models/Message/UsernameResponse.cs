namespace AnastasiaHueApp.Models.Message;

/// <summary>
/// This response can occur upon the following actions (<i>note that some occurrences might be missing from this list</i>): <br/>
/// - Attempting to link to the Hue lights AFTER pressing the physical linking button. <br/>
/// </summary>
public class UsernameResponse
{
    public required string Username { get; init; }
}