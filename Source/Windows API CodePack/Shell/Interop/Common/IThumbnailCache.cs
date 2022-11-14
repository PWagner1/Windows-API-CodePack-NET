namespace Microsoft.WindowsAPICodePack.Shell;

[ComImport,
 Guid(ShellIIDGuid.IThumbnailCache),
 InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
interface IThumbnailCache
{
    void GetThumbnail([In] IShellItem pShellItem,
        [In] uint cxyRequestedThumbSize,
        [In] ShellNativeMethods.ThumbnailOptions flags,
        [Out] out ISharedBitmap ppvThumb,
        [Out] out ShellNativeMethods.ThumbnailCacheOptions pOutFlags,
        [Out] ShellNativeMethods.ThumbnailId pThumbnailID);

    void GetThumbnailByID([In] ShellNativeMethods.ThumbnailId thumbnailID,
        [In] uint cxyRequestedThumbSize,
        [Out] out ISharedBitmap ppvThumb,
        [Out] out ShellNativeMethods.ThumbnailCacheOptions pOutFlags);
}