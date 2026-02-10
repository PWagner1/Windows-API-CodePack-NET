namespace Microsoft.WindowsAPICodePack.Shell;

/// <summary>
/// Flags that specify how Shell items are compared when calling <c>IShellItem.Compare</c>.
/// </summary>
public enum SICHINTF
{
    /// <summary>
    /// Compare based on the display name.
    /// </summary>
    SICHINT_DISPLAY = 0x00000000,

    /// <summary>
    /// Compare a canonical name that is suitable for programmatic comparison.
    /// </summary>
    SICHINT_CANONICAL = 0x10000000,

    /// <summary>
    /// If canonical comparison fails, compare by file system path.
    /// </summary>
    SICHINT_TEST_FILESYSPATH_IF_NOT_EQUAL = 0x20000000,

    /// <summary>
    /// Compare all available fields.
    /// </summary>
    SICHINT_ALLFIELDS = unchecked((int)0x80000000)
}