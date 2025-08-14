namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem;

/// <summary>
/// Specifies the display types for a property.
/// </summary>
public enum PropertyDisplayType
{
    /// <summary>
    /// The String Display. This is the default if the property doesn't specify a display type.
    /// </summary>
    String = 0,

    /// <summary>
    /// The Number Display.
    /// </summary>
    Number = 1,

    /// <summary>
    /// The Boolean Display.
    /// </summary>
    Boolean = 2,

    /// <summary>
    /// The DateTime Display.
    /// </summary>
    DateTime = 3,

    /// <summary>
    /// The Enumerated Display.
    /// </summary>
    Enumerated = 4
}