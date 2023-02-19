namespace Microsoft.WindowsAPICodePack.ShellExtensions.Interop;

/// <summary>
/// Provides means by which to initialize with a file.
/// </summary>
[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("b7d14566-0509-4cce-a71f-0a554233bd9b")]
interface IInitializeWithFile
{
    /// <summary>
    /// Initializes with a file.
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="fileMode"></param>
    void Initialize([MarshalAs(UnmanagedType.LPWStr)] string filePath, AccessModes fileMode);
}