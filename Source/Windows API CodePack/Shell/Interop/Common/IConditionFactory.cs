namespace Microsoft.WindowsAPICodePack.Shell;

[ComImport,
 Guid(ShellIIDGuid.IConditionFactory),
 InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IConditionFactory
{
    [PreserveSig]
    HResult MakeNot([In] ICondition pcSub, [In] bool fSimplify, [Out] out ICondition ppcResult);

    [PreserveSig]
    HResult MakeAndOr([In] SearchConditionType ct, [In] IEnumUnknown peuSubs, [In] bool fSimplify, [Out] out ICondition ppcResult);

    [PreserveSig]
    HResult MakeLeaf(
        [In, MarshalAs(UnmanagedType.LPWStr)] string pszPropertyName,
        [In] SearchConditionOperation cop,
        [In, MarshalAs(UnmanagedType.LPWStr)] string pszValueType,
        [In] PropVariant ppropvar,
        IRichChunk richChunk1,
        IRichChunk richChunk2,
        IRichChunk richChunk3,
        [In] bool fExpand,
        [Out] out ICondition ppcResult);

    [PreserveSig]
    HResult Resolve(/*[In] ICondition pc, [In] STRUCTURED_QUERY_RESOLVE_OPTION sqro, [In] ref SYSTEMTIME pstReferenceTime, [Out] out ICondition ppcResolved*/);

};