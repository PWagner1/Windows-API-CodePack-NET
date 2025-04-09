namespace Microsoft.WindowsAPICodePack.Shell
{
    [ComImport()]
    [Guid("bcc18b79-ba16-442f-80c4-8a59c30c463b")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IShellItemImageFactory
    {
        [PreserveSig]
        HResult GetImage(
            [In, MarshalAs(UnmanagedType.Struct)] CoreNativeMethods.Size size,
            [In] ShellNativeMethods.SIIGBF flags,
            [Out] out IntPtr phbm);
    }
}