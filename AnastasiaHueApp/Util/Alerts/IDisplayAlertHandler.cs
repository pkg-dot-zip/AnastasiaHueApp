using AnastasiaHueApp.Models.Message;

namespace AnastasiaHueApp.Util.Alerts;

public interface IDisplayAlertHandler
{
    /// <summary>
    /// Displays an alert dialog to the application user with a single cancel button.
    /// </summary>
    /// <param name="title">The title of the alert dialog. Can be <see langword="null"/> to hide the title.</param>
    /// <param name="message">The body text of the alert dialog.</param>
    /// <param name="cancel">Text to be displayed on the 'Cancel' button.</param>
    /// <returns>A <see cref="Task"/> that contains the user's choice as a <see cref="bool"/> value. <see langword="false"/> indicates that the user cancelled the alert. This will <b>always</b> happen!</returns>
    public Task<bool> DisplayAlert(string? title, string message, string cancel = "OK");

    /// <summary>
    /// Displays an alert dialog to the application user with a single cancel button.
    /// </summary>
    /// <param name="title">The title of the alert dialog. Can be <see langword="null"/> to hide the title.</param>
    /// <param name="message">The body text of the alert dialog.</param>
    /// <param name="accept">Text to be displayed on the 'Accept' button. Can be <see langword="null"/> to hide this button.</param>
    /// <param name="cancel">Text to be displayed on the 'Cancel' button.</param>
    /// <param name="flowDirection">The flow direction to be used by the alert.</param>
    /// <returns>A <see cref="Task"/> that contains the user's choice as a <see cref="bool"/> value. <see langword="true"/> indicates that the user accepted the alert. <see langword="false"/> indicates that the user cancelled the alert.</returns>
    public Task<bool> DisplayAlert(string? title, string message, string? accept, string cancel, FlowDirection flowDirection);

    /// <summary>
    /// Helper method that calls <see cref="DisplayAlert(string?,string,string)"/> with params based on <paramref name="error"/>.
    /// </summary>
    /// <param name="error">Error to display in the alert.</param>
    /// <returns>Always <see langword="false"/>, since there is only a cancel button.</returns>
    public Task<bool> DisplayAlert(ErrorResponse error);
}