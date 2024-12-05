using Newtonsoft.Json;

namespace AnastasiaHueApp.Models;

/// <summary>
/// NOTE: This feature is <b>deprecated</b>. Scenes and animations were introduced for this. Do not use.
/// </summary>
public class PointSymbol
{
    [JsonProperty("1")]
    public string Symbol1 { get; set; }
    [JsonProperty("2")]
    public string Symbol2 { get; set; }
    [JsonProperty("3")]
    public string Symbol3 { get; set; }
    [JsonProperty("4")]
    public string Symbol4 { get; set; }
    [JsonProperty("5")]
    public string Symbol5 { get; set; }
    [JsonProperty("6")]
    public string Symbol6 { get; set; }
    [JsonProperty("7")]
    public string Symbol7 { get; set; }
    [JsonProperty("8")]
    public string Symbol8 { get; set; }

    #region Equals

    protected bool Equals(PointSymbol other)
    {
        return Symbol1 == other.Symbol1 && Symbol2 == other.Symbol2 && Symbol3 == other.Symbol3 &&
               Symbol4 == other.Symbol4 && Symbol5 == other.Symbol5 && Symbol6 == other.Symbol6 &&
               Symbol7 == other.Symbol7 && Symbol8 == other.Symbol8;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((PointSymbol)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Symbol1, Symbol2, Symbol3, Symbol4, Symbol5, Symbol6, Symbol7, Symbol8);
    }

    #endregion
}