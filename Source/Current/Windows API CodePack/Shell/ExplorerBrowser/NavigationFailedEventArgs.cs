namespace Microsoft.WindowsAPICodePack.Controls;

/// <summary>
/// Event argument for the NavigatinoFailed event
/// </summary>
public class NavigationFailedEventArgs : EventArgs
{
    /// <summary>
    /// The location the the browser would have navigated to.
    /// </summary>
    public ShellObject? FailedLocation { get; set; }
}