namespace Microsoft.WindowsAPICodePack.ShellExtensions;

/// <summary>
/// Adornment applied to thumbnails.
/// </summary>
public enum ThumbnailAdornment
{
    /// <summary>
    /// This will use the associated application's default icon as the adornment.
    /// </summary>
    Default = -1, // Default behaviour for no value added in registry

    /// <summary>
    /// No adornment
    /// </summary>
    None = 0,

    /// <summary>
    /// Drop shadow adornment
    /// </summary>
    DropShadow = 1,

    /// <summary>
    /// Photo border adornment
    /// </summary>
    PhotoBorder = 2,

    /// <summary>
    /// Video sprocket adornment
    /// </summary>
    VideoSprockets = 3
}