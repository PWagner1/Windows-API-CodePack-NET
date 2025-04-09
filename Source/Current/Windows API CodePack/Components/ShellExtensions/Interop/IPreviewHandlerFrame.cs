using Message = Microsoft.WindowsAPICodePack.Shell.Interop.Message;

namespace Microsoft.WindowsAPICodePack.ShellExtensions.Interop
{
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("fec87aaf-35f9-447a-adb7-20234491401a")]
    interface IPreviewHandlerFrame
    {
        void GetWindowContext(IntPtr pinfo);
        [PreserveSig]
        HResult TranslateAccelerator(ref Message pmsg);
    };
}