namespace AnastasiaHueApp.Util.Preferences;

public interface IStorageHandler
{
    /// <summary>
    /// Retrieves the username for the Hue Bridge from the preferences if one was stored.
    /// </summary>
    /// <returns>The username. If not found, it returns <see langword="null"/>.</returns>
    public string? RetrieveUsername();

    /// <summary>
    /// Sets the username used for the Hue Bridge.
    /// </summary>
    /// <param name="username">Username to store.</param>
    public void SetUsername(string username);
}