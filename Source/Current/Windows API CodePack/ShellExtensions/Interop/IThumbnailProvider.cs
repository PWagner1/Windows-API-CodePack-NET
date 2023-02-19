namespace Microsoft.WindowsAPICodePack.ShellExtensions.Interop;

/// <summary>
/// ComVisible interface for native IThumbnailProvider
/// </summary>
[ComImport]
[Guid("e357fccd-a995-4576-b01f-234630154e96")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
interface IThumbnailProvider
{
    /// <summary>
    /// Gets a pointer to a bitmap to display as a thumbnail
    /// </summary>
    /// <param name="squareLength"></param>
    /// <param name="bitmapHandle"></param>
    /// <param name="bitmapType"></param>
    [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#"), SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#")]
    void GetThumbnail(uint squareLength, [Out] out IntPtr bitmapHandle, [Out] out UInt32 bitmapType);        
}