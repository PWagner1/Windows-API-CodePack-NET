//Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.WindowsAPICodePack.Controls;

/// <summary>
/// Event argument for The NavigationPending event
/// </summary>
public class NavigationPendingEventArgs : EventArgs
{
    /// <summary>
    /// The location being navigated to
    /// </summary>
    public ShellObject? PendingLocation { get; set; }

    /// <summary>
    /// Set to 'True' to cancel the navigation.
    /// </summary>
    public bool Cancel { get; set; }

}