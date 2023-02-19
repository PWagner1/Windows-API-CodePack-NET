namespace Microsoft.WindowsAPICodePack.ShellExtensions.Interop;

/// <summary>
/// Provides means by which to initialize with a ShellObject
/// </summary>
[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("7f73be3f-fb79-493c-a6c7-7ee14e245841")]
interface IInitializeWithItem
{
    /// <summary>
    /// Initializes with ShellItem
    /// </summary>
    /// <param name="shellItem"></param>
    /// <param name="accessMode"></param>
    void Initialize(IShellItem? shellItem, AccessModes accessMode);
}