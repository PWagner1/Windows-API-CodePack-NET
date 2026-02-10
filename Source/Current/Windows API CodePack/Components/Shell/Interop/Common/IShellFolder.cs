namespace Microsoft.WindowsAPICodePack.Shell;

/// <summary>
/// Managed definition of the native <c>IShellFolder</c> COM interface, which represents a Shell namespace folder.
/// </summary>
[ComImport,
 Guid(ShellIIDGuid.IShellFolder),
 InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
 ComConversionLoss]
public interface IShellFolder
{
    /// <summary>
    /// Translates a display name into a pointer to an item identifier list (PIDL).
    /// </summary>
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ParseDisplayName(IntPtr hwnd, [In, MarshalAs(UnmanagedType.Interface)] IBindCtx pbc, [In, MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, [In, Out] ref uint pchEaten, [Out] IntPtr ppidl, [In, Out] ref uint pdwAttributes);

    /// <summary>
    /// Creates an item identifier enumeration object for the contents of the folder.
    /// </summary>
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    HResult EnumObjects([In] IntPtr hwnd, [In] ShellNativeMethods.ShellFolderEnumerationOptions grfFlags, [MarshalAs(UnmanagedType.Interface)] out IEnumIDList? ppenumIDList);

    /// <summary>
    /// Binds to a subfolder, returning its <see cref="IShellFolder"/> interface.
    /// </summary>
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    HResult BindToObject([In] IntPtr pidl, /*[In, MarshalAs(UnmanagedType.Interface)] IBindCtx*/ IntPtr pbc, [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out IShellFolder ppv);

    /// <summary>
    /// Binds to a storage object that is represented by a Shell item.
    /// </summary>
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void BindToStorage([In] ref IntPtr pidl, [In, MarshalAs(UnmanagedType.Interface)] IBindCtx pbc, [In] ref Guid riid, out IntPtr ppv);

    /// <summary>
    /// Compares two item identifier lists.
    /// </summary>
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void CompareIDs([In] IntPtr lParam, [In] ref IntPtr pidl1, [In] ref IntPtr pidl2);

    /// <summary>
    /// Retrieves an object that can be used to obtain information from or interact with the folder.
    /// </summary>
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void CreateViewObject([In] IntPtr hwndOwner, [In] ref Guid riid, out IntPtr ppv);

    /// <summary>
    /// Retrieves the attributes of one or more items in a folder.
    /// </summary>
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetAttributesOf([In] uint cidl, [In] IntPtr apidl, [In, Out] ref uint rgfInOut);

    /// <summary>
    /// Retrieves interfaces that handle operations specific to particular data objects or items in a Shell folder.
    /// </summary>
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetUIObjectOf([In] IntPtr hwndOwner, [In] uint cidl, [In] IntPtr apidl, [In] ref Guid riid, [In, Out] ref uint rgfReserved, out IntPtr ppv);

    /// <summary>
    /// Gets the display name for the specified item.
    /// </summary>
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetDisplayNameOf([In] ref IntPtr pidl, [In] uint uFlags, out IntPtr pName);

    /// <summary>
    /// Sets the display name of a file object or subfolder, changing the item identifier in the process.
    /// </summary>
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetNameOf([In] IntPtr hwnd, [In] ref IntPtr pidl, [In, MarshalAs(UnmanagedType.LPWStr)] string pszName, [In] uint uFlags, [Out] IntPtr ppidlOut);
}