namespace Microsoft.WindowsAPICodePack.Shell;

/// <summary>
/// Managed definition of the native <c>IShellItem</c> COM interface, which represents an item in the Shell namespace.
/// </summary>
[ComImport,
 Guid(ShellIIDGuid.IShellItem),
 InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IShellItem
{
    // Not supported: IBindCtx.
    /// <summary>
    /// Binds to a handler for the specified item.
    /// </summary>
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    HResult BindToHandler(
        [In] IntPtr pbc,
        [In] ref Guid bhid,
        [In] ref Guid riid,
        [Out, MarshalAs(UnmanagedType.Interface)] out IShellFolder? ppv);

    /// <summary>
    /// Gets the parent of an item in the Shell namespace.
    /// </summary>
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetParent([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

    /// <summary>
    /// Gets the display name of the item for the requested format.
    /// </summary>
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    HResult GetDisplayName(
        [In] ShellNativeMethods.ShellItemDesignNameOptions sigdnName,
        out IntPtr ppszName);

    /// <summary>
    /// Gets the requested attributes of the item.
    /// </summary>
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetAttributes([In] ShellNativeMethods.ShellFileGetAttributesOptions sfgaoMask, out ShellNativeMethods.ShellFileGetAttributesOptions psfgaoAttribs);

    /// <summary>
    /// Compares two Shell items to determine their relative sorting order.
    /// </summary>
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    HResult Compare(
        [In, MarshalAs(UnmanagedType.Interface)] IShellItem? psi,
        [In] SICHINTF hint,
        out int piOrder);
}