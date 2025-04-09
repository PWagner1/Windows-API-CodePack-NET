namespace Microsoft.WindowsAPICodePack.Shell
{
    [ComImport,
     Guid(ShellIIDGuid.IRichChunk),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IRichChunk
    {
        // The position *pFirstPos is zero-based.
        // Any one of pFirstPos, pLength, ppsz and pValue may be NULL.
        [PreserveSig]
        HResult GetData(/*[out, annotation("__out_opt")] ULONG* pFirstPos, [out, annotation("__out_opt")] ULONG* pLength, [out, annotation("__deref_opt_out_opt")] LPWSTR* ppsz, [out, annotation("__out_opt")] PROPVARIANT* pValue*/);
    }
}