namespace AnastasiaHueApp.Util.Shell;

internal class ShellContainer : IShellContainer
{
    /// <inheritdoc />
    public Microsoft.Maui.Controls.Shell CurrentShell => Microsoft.Maui.Controls.Shell.Current;
}