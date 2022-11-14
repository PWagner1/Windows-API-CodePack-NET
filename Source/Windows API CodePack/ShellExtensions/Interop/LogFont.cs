namespace Microsoft.WindowsAPICodePack.ShellExtensions.Interop;

/// <summary>
/// Class for marshaling to native LogFont struct
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class LogFont
{
    /// <summary>
    /// Font height
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Font width
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Font escapement
    /// </summary>
    public int Escapement { get; set; }

    /// <summary>
    /// Font orientation
    /// </summary>
    public int Orientation { get; set; }

    /// <summary>
    /// Font weight
    /// </summary>
    public int Weight { get; set; }

    /// <summary>
    /// Font italic
    /// </summary>
    public byte Italic { get; set; }

    /// <summary>
    /// Font underline
    /// </summary>
    public byte Underline { get; set; }

    /// <summary>
    /// Font strikeout
    /// </summary>
    public byte Strikeout { get; set; }

    /// <summary>
    /// Font character set
    /// </summary>
    public byte CharacterSet { get; set; }

    /// <summary>
    /// Font out precision
    /// </summary>
    public byte OutPrecision { get; set; }

    /// <summary>
    /// Font clip precision
    /// </summary>
    public byte ClipPrecision { get; set; }

    /// <summary>
    /// Font quality
    /// </summary>
    public byte Quality { get; set; }

    /// <summary>
    /// Font pitch and family
    /// </summary>
    public byte PitchAndFamily { get; set; }

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    private string faceName = string.Empty;

    /// <summary>
    /// Font face name
    /// </summary>
    public string FaceName { get { return faceName; } set { faceName = value; } }
}