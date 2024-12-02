namespace AnastasiaHueApp.Util.Preferences;

public interface IPreferencesHandler
{
    public string? RetrieveUsername();
    public void SetUsername(string username);
}