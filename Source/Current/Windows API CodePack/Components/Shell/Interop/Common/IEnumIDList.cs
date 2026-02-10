namespace Microsoft.WindowsAPICodePack.Shell;

/// <summary>
/// Managed definition of the native <c>IEnumIDList</c> COM interface used to enumerate item ID lists (PIDLs).
/// </summary>
[ComImport,
 Guid(ShellIIDGuid.IEnumIDList),
 InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IEnumIDList
{
    /// <summary>
    /// Retrieves a specified number of item identifiers in the enumeration sequence.
    /// </summary>
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    HResult Next(uint celt, out IntPtr rgelt, out uint pceltFetched);

    /// <summary>
    /// Skips the specified number of elements in the enumeration sequence.
    /// </summary>
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    HResult Skip([In] uint celt);

    /// <summary>
    /// Resets the enumeration sequence to the beginning.
    /// </summary>
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    HResult Reset();

    /// <summary>
    /// Creates a new enumerator that contains the same enumeration state as the current one.
    /// </summary>
    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    HResult Clone([MarshalAs(UnmanagedType.Interface)] out IEnumIDList ppenum);
}