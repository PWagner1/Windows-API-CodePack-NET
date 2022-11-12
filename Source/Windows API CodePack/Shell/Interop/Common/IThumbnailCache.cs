namespace Microsoft.WindowsAPICodePack.Shell;

[ComImport,
 Guid(ShellIIDGuid.IThumbnailCache),
 InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
interface IThumbnailCache
{
    void GetThumbnail([In] IShellItem pShellItem,
        [In] uint cxyRequestedThumbSize,
        [In] Microsoft.WindowsAPICodePack.Shell.ShellNativeMethods.ThumbnailOptions flags,
        [Out] out ISharedBitmap ppvThumb,
        [Out] out Microsoft.WindowsAPICodePack.Shell.ShellNativeMethods.ThumbnailCacheOptions pOutFlags,
        [Out] Microsoft.WindowsAPICodePack.Shell.ShellNativeMethods.ThumbnailId pThumbnailID);

    void GetThumbnailByID([In] Microsoft.WindowsAPICodePack.Shell.ShellNativeMethods.ThumbnailId thumbnailID,
        [In] uint cxyRequestedThumbSize,
        [Out] out ISharedBitmap ppvThumb,
        [Out] out Microsoft.WindowsAPICodePack.Shell.ShellNativeMethods.ThumbnailCacheOptions pOutFlags);
}