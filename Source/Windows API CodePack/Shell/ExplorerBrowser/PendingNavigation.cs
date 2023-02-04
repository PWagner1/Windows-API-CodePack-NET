namespace Microsoft.WindowsAPICodePack.Controls;

/// <summary>
/// A navigation traversal request
/// </summary>
internal class PendingNavigation
{
    internal PendingNavigation(ShellObject? location, int index)
    {
        Location = location;
        Index = index;
    }

    internal ShellObject? Location { get; set; }
    internal int Index { get; set; }
}