namespace Microsoft.WindowsAPICodePack.Shell.Interop
{
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("A0FFBC28-5482-4366-BE27-3E81E78E06C2")]
    internal interface ISearchFolderItemFactory
    {
        void SetDisplayName([MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName);
        void SetFolderTypeID(ref Guid ftid);
        void SetFolderLogicalViewMode(uint flvm);
        void SetIconSize(int iIconSize);
        void SetVisibleColumns(uint cVisibleColumns, IntPtr rgKey);
        void SetSortColumns(uint cSortColumns, IntPtr rgSortColumns);
        void SetGroupColumn(ref PropertyKey keyGroup);
        void SetStacks(uint cStackKeys, IntPtr rgStackKeys);
        void SetScope([MarshalAs(UnmanagedType.Interface)] IShellItemArray pScope);
        void SetCondition([MarshalAs(UnmanagedType.Interface)] ICondition pCondition);
        void GetShellItem(uint flags, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IShellItem shellItem);
    }
}
