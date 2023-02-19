namespace Microsoft.WindowsAPICodePack.Controls;

/// <summary>
/// Event argument for The NavigationComplete event
/// </summary>
public class NavigationCompleteEventArgs : EventArgs
{
    /// <summary>
    /// The new location of the explorer browser
    /// </summary>
    public ShellObject? NewLocation { get; set; }
}