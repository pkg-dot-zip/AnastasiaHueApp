namespace AnastasiaHueApp.Models;

/// <summary>
/// The alert effect, which is a temporary change to the bulb’s state.
/// </summary>
public enum HueAlert
{
    /// <summary>
    /// The light is not performing an alert effect.
    /// </summary>
    None,

    /// <summary>
    /// The light is performing one breathe cycle.
    /// </summary>
    Select,

    /// <summary>
    /// The light is performing breathe cycles for 15 seconds or until an <see cref="None"/> command is received. <br/>
    /// Note that this contains the last alert sent to the light and not its current state.
    /// i.e. After the breathe cycle has finished the bridge does not reset the alert to <see cref="None"/>.
    /// </summary>
    LSelect,
}