namespace Microsoft.WindowsAPICodePack.Shell
{
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid(ShellIIDGuid.IEnumUnknown)]
    internal interface IEnumUnknown
    {
        [PreserveSig]
        HResult Next(uint requestedNumber, ref IntPtr buffer, ref uint fetchedNumber);
        [PreserveSig]
        HResult Skip(uint number);
        [PreserveSig]
        HResult Reset();
        [PreserveSig]
        HResult Clone(out IEnumUnknown result);
    }
}