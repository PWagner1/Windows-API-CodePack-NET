namespace Microsoft.WindowsAPICodePack.Controls;

/// <summary>
/// Indicates the visibility state of an ExplorerBrowser pane
/// </summary>
public enum PaneVisibilityState
{
    /// <summary>
    /// Allow the explorer browser to determine if this pane is displayed.
    /// </summary>
    DoNotCare,
    /// <summary>
    /// Hide the pane
    /// </summary>
    Hide,
    /// <summary>
    /// Show the pane
    /// </summary>
    Show
}