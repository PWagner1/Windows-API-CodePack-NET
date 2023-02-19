namespace Microsoft.WindowsAPICodePack.Shell;

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid(ShellIIDGuid.IEnumUnknown)]
internal interface IEnumUnknown
{
    [PreserveSig]
    HResult Next(UInt32 requestedNumber, ref IntPtr buffer, ref UInt32 fetchedNumber);
    [PreserveSig]
    HResult Skip(UInt32 number);
    [PreserveSig]
    HResult Reset();
    [PreserveSig]
    HResult Clone(out IEnumUnknown result);
}