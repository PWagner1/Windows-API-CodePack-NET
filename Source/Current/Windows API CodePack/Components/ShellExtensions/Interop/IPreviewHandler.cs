using Message = Microsoft.WindowsAPICodePack.Shell.Interop.Message;

namespace Microsoft.WindowsAPICodePack.ShellExtensions.Interop
{
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("8895b1c6-b41f-4c1c-a562-0d564250836f")]
    interface IPreviewHandler
    {
        void SetWindow(IntPtr hwnd, ref NativeRect rect);
        void SetRect(ref NativeRect rect);
        void DoPreview();
        void Unload();
        void SetFocus();
        void QueryFocus(out IntPtr phwnd);
        [PreserveSig]
        HResult TranslateAccelerator(ref Message pmsg);
    }
}