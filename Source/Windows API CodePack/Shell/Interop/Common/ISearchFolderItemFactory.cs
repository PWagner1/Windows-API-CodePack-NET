namespace Microsoft.WindowsAPICodePack.Shell;

[ComImport,
 Guid(ShellIIDGuid.ISearchFolderItemFactory),
 InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface ISearchFolderItemFactory
{
    [PreserveSig]
    HResult SetDisplayName([In, MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName);

    [PreserveSig]
    HResult SetFolderTypeID([In] Guid ftid);

    [PreserveSig]
    HResult SetFolderLogicalViewMode([In] FolderLogicalViewMode flvm);

    [PreserveSig]
    HResult SetIconSize([In] int iIconSize);

    [PreserveSig]
    HResult SetVisibleColumns([In] uint cVisibleColumns, [In, MarshalAs(UnmanagedType.LPArray)] PropertyKey[] rgKey);

    [PreserveSig]
    HResult SetSortColumns([In] uint cSortColumns, [In, MarshalAs(UnmanagedType.LPArray)] SortColumn[] rgSortColumns);

    [PreserveSig]
    HResult SetGroupColumn([In] ref PropertyKey keyGroup);

    [PreserveSig]
    HResult SetStacks([In] uint cStackKeys, [In, MarshalAs(UnmanagedType.LPArray)] PropertyKey[] rgStackKeys);

    [PreserveSig]
    HResult SetScope([In, MarshalAs(UnmanagedType.Interface)] IShellItemArray ppv);

    [PreserveSig]
    HResult SetCondition([In] ICondition pCondition);

    [PreserveSig]
    int GetShellItem(ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out IShellItem? ppv);

    [PreserveSig]
    HResult GetIDList([Out] IntPtr ppidl);
};