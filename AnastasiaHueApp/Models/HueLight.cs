using Color = AnastasiaHueApp.Util.Color.Color;

namespace AnastasiaHueApp.Models;

public class HueLight
{
    public int Id { get; set; } =
        int.MinValue; // If set to minvalue many functions will throw, meaning we know we forgot to set this.

    /// <summary>
    /// The hardware model of the light.
    /// </summary>
    public required string ModelId { get; init; }

    /// <summary>
    /// A unique, editable name given to the light.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// An identifier for the software version running on the light.
    /// </summary>
    public required string SwVersion { get; init; }

    /// <summary>
    /// Details the state of the light.
    /// </summary>
    public required HueLightState State { get; init; }

    /// <summary>
    /// A fixed name describing the type of light e.g. <i>“Extended color light”</i>.
    /// </summary>
    public required string Type { get; init; }

    /// <summary>
    /// <b>Deprecated</b> by Philips in favour of scenes and animations. <b>Ignore</b>.
    /// </summary>
    public required PointSymbol PointSymbol { get; init; }

    /// <summary>
    /// Unique id of the device. The MAC address of the device with a unique endpoint id in the form: <b>AA:BB:CC:DD:EE:FF:00:11-XX</b>.
    /// </summary>
    public required string UniqueId { get; init; }

    #region Equals

    protected bool Equals(HueLight other)
    {
        return Id == other.Id && ModelId == other.ModelId && Name == other.Name && SwVersion == other.SwVersion &&
               State.Equals(other.State) && Type == other.Type && PointSymbol.Equals(other.PointSymbol) &&
               UniqueId == other.UniqueId;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((HueLight)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, ModelId, Name, SwVersion, State, Type, PointSymbol, UniqueId);
    }

    #endregion
}