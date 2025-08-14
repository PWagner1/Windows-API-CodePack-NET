//Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.WindowsAPICodePack.Controls;

/// <summary>
/// Indicates the viewing mode of the explorer browser
/// </summary>
public enum ExplorerBrowserViewMode
{
    /// <summary>
    /// Choose the best view mode for the folder
    /// </summary>
    Auto = -1,

    /// <summary>
    /// (New for Windows7)
    /// </summary>
    Content = 8,

    /// <summary>
    /// Object names and other selected information, such as the size or date last updated, are shown.
    /// </summary>
    Details = 4,

    /// <summary>
    /// The view should display medium-size icons.
    /// </summary>
    Icon = 1,

    /// <summary>
    /// Object names are displayed in a list view.
    /// </summary>
    List = 3,

    /// <summary>
    /// The view should display small icons. 
    /// </summary>
    SmallIcon = 2,

    /// <summary>
    /// The view should display thumbnail icons. 
    /// </summary>
    Thumbnail = 5,

    /// <summary>
    /// The view should display icons in a filmstrip format.
    /// </summary>
    ThumbStrip = 7,

    /// <summary>
    /// The view should display large icons. 
    /// </summary>
    Tile = 6
}