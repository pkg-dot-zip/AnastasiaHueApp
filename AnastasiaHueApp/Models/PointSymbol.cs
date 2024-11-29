namespace AnastasiaHueApp.Models;

/// <summary>
/// NOTE: This feature is deprecated. Scenes and animations were introduced for this. Do not use.
/// </summary>
public class PointSymbol
{
    public required string Symbol1 { get; init; }
    public required string Symbol2 { get; init; }
    public required string Symbol3 { get; init; }
    public required string Symbol4 { get; init; }
    public required string Symbol5 { get; init; }
    public required string Symbol6 { get; init; }
    public required string Symbol7 { get; init; }
    public required string Symbol8 { get; init; }

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