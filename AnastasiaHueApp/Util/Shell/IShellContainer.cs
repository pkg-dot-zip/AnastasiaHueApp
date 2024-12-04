namespace AnastasiaHueApp.Util.Shell;

public interface IShellContainer
{
    /// <summary>
    /// The current <see cref="Microsoft.Maui.Controls.Shell"/> used for navigation.
    /// </summary>
    public Microsoft.Maui.Controls.Shell CurrentShell { get; }
}