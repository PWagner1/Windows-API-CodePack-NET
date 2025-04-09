using ThumbnailAlphaType = Microsoft.WindowsAPICodePack.Taskbar.ThumbnailAlphaType;

namespace Microsoft.WindowsAPICodePack.Shell
{
    [ComImport,
     Guid(ShellIIDGuid.ISharedBitmap),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface ISharedBitmap
    {
        void GetSharedBitmap([Out] out IntPtr phbm);
        void GetSize([Out] out CoreNativeMethods.Size pSize);
        void GetFormat([Out] out ThumbnailAlphaType pat);
        void InitializeBitmap([In] IntPtr hbm, [In] ThumbnailAlphaType wtsAT);
        void Detach([Out] out IntPtr phbm);
    }
}