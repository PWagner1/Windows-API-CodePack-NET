namespace Microsoft.WindowsAPICodePack.Shell;

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("00000109-0000-0000-C000-000000000046")]
internal interface IPersistStream
{
    // Summary:
    //     Retrieves the class identifier (CLSID) of an object.
    //
    // Parameters:
    //   pClassID:
    //     When this method returns, contains a reference to the CLSID. This parameter
    //     is passed uninitialized.
    [PreserveSig]
    void GetClassID(out Guid pClassID);
    //
    // Summary:
    //     Checks an object for changes since it was last saved to its current file.
    //
    // Returns:
    //     S_OK if the file has changed since it was last saved; S_FALSE if the file
    //     has not changed since it was last saved.
    [PreserveSig]
    HResult IsDirty();

    [PreserveSig]
    HResult Load([In, MarshalAs(UnmanagedType.Interface)] IStream stm);

    [PreserveSig]
    HResult Save([In, MarshalAs(UnmanagedType.Interface)] IStream stm, bool fRemember);

    [PreserveSig]
    HResult GetSizeMax(out ulong cbSize);
}