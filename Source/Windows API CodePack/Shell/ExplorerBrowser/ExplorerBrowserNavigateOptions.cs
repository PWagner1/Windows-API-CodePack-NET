namespace Microsoft.WindowsAPICodePack.Controls;

/// <summary>
/// Specifies the options that control subsequent navigation.
/// Typically use one, or a bitwise combination of these
/// flags to specify how the explorer browser navigates.
/// </summary>
[Flags]
public enum ExplorerBrowserNavigateOptions
{
    /// <summary>
    /// Always navigate, even if you are attempting to navigate to the current folder.
    /// </summary>
    AlwaysNavigate = 0x00000004,

    /// <summary>
    /// Do not navigate further than the initial navigation.
    /// </summary>
    NavigateOnce = 0x00000001,
}