namespace Microsoft.WindowsAPICodePack.ShellExtensions.Interop
{
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("8327b13c-b63f-4b24-9b8a-d010dcc3f599")]
    interface IPreviewHandlerVisuals
    {
        void SetBackgroundColor(NativeColorRef color);
        void SetFont(ref LogFont plf);
        void SetTextColor(NativeColorRef color);
    }
}