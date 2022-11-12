namespace Microsoft.WindowsAPICodePack.Shell;

[ComImport(),
 Guid(ShellIIDGuid.IModalWindow),
 InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IModalWindow
{
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
     PreserveSig]
    int Show([In] IntPtr parent);
}