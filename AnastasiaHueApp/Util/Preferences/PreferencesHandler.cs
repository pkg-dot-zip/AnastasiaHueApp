using Microsoft.Extensions.Logging;

namespace AnastasiaHueApp.Util.Preferences;

public class PreferencesHandler(ILogger<PreferencesHandler> logger, IPreferences preferences) : IPreferencesHandler
{
    private const string UsernameKey = "bridge_username";

    /// <inheritdoc />
    public string? RetrieveUsername() => RetrievePreference(UsernameKey);

    /// <inheritdoc />
    public void SetUsername(string username) => SetPreference(UsernameKey, username);

    private string? RetrievePreference(string key)
    {
        var str = preferences.Get(key, string.Empty);
        return str is null or "" ? null : str;
    }

    private void SetPreference(string key, string value)
    {
        logger.LogInformation("Set preference {0} to {1}", key, value);
        preferences.Set(key, value);
    }
}