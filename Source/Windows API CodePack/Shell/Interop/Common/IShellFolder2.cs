namespace Microsoft.WindowsAPICodePack.Shell;

[ComImport,
 Guid(ShellIIDGuid.IShellFolder2),
 InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
 ComConversionLoss]
internal interface IShellFolder2 : IShellFolder
{
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ParseDisplayName([In] IntPtr hwnd, [In, MarshalAs(UnmanagedType.Interface)] IBindCtx pbc, [In, MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, [In, Out] ref uint pchEaten, [Out] IntPtr ppidl, [In, Out] ref uint pdwAttributes);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EnumObjects([In] IntPtr hwnd, [In] ShellNativeMethods.ShellFolderEnumerationOptions grfFlags, [MarshalAs(UnmanagedType.Interface)] out IEnumIDList ppenumIDList);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void BindToObject([In] IntPtr pidl, /*[In, MarshalAs(UnmanagedType.Interface)] IBindCtx*/ IntPtr pbc, [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out IShellFolder ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void BindToStorage([In] ref IntPtr pidl, [In, MarshalAs(UnmanagedType.Interface)] IBindCtx pbc, [In] ref Guid riid, out IntPtr ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void CompareIDs([In] IntPtr lParam, [In] ref IntPtr pidl1, [In] ref IntPtr pidl2);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void CreateViewObject([In] IntPtr hwndOwner, [In] ref Guid riid, out IntPtr ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetAttributesOf([In] uint cidl, [In] IntPtr apidl, [In, Out] ref uint rgfInOut);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetUIObjectOf([In] IntPtr hwndOwner, [In] uint cidl, [In] IntPtr apidl, [In] ref Guid riid, [In, Out] ref uint rgfReserved, out IntPtr ppv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetDisplayNameOf([In] ref IntPtr pidl, [In] uint uFlags, out IntPtr pName);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetNameOf([In] IntPtr hwnd, [In] ref IntPtr pidl, [In, MarshalAs(UnmanagedType.LPWStr)] string pszName, [In] uint uFlags, [Out] IntPtr ppidlOut);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetDefaultSearchGUID(out Guid pguid);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EnumSearches([Out] out IntPtr ppenum);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetDefaultColumn([In] uint dwRes, out uint pSort, out uint pDisplay);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetDefaultColumnState([In] uint iColumn, out uint pcsFlags);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetDetailsEx([In] ref IntPtr pidl, [In] ref PropertyKey pscid, [MarshalAs(UnmanagedType.Struct)] out object pv);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetDetailsOf([In] ref IntPtr pidl, [In] uint iColumn, out IntPtr psd);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void MapColumnToSCID([In] uint iColumn, out PropertyKey pscid);
}