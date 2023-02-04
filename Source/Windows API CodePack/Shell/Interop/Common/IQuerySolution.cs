namespace Microsoft.WindowsAPICodePack.Shell;

[ComImport,
 Guid(ShellIIDGuid.IQuerySolution),
 InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
interface IQuerySolution : IConditionFactory
{
    [PreserveSig]
    new HResult MakeNot([In] ICondition pcSub, [In] bool fSimplify, [Out] out ICondition ppcResult);

    [PreserveSig]
    new HResult MakeAndOr([In] SearchConditionType ct, [In] IEnumUnknown peuSubs, [In] bool fSimplify, [Out] out ICondition ppcResult);

    [PreserveSig]
    new HResult MakeLeaf(
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
    new HResult Resolve(/*[In] ICondition pc, [In] int sqro, [In] ref SYSTEMTIME pstReferenceTime, [Out] out ICondition ppcResolved*/);

    // Retrieve the condition tree and the "main type" of the solution.
    // ppQueryNode and ppMainType may be NULL.
    [PreserveSig]
    HResult GetQuery([Out, MarshalAs(UnmanagedType.Interface)] out ICondition ppQueryNode, [Out, MarshalAs(UnmanagedType.Interface)] out IEntity ppMainType);

    // Identify parts of the input string not accounted for.
    // Each parse error is represented by an IRichChunk where the position information
    // reflect token counts, the string is NULL and the value is a VT_I4
    // where lVal is from the ParseErrorType enumeration. The valid
    // values for riid are IID_IEnumUnknown and IID_IEnumVARIANT.
    [PreserveSig]
    HResult GetErrors([In] ref Guid riid, [Out] out /* void** */ IntPtr ppParseErrors);

    // Report the query string, how it was tokenized and what LCID and word breaker were used (for recognizing keywords).
    // ppszInputString, ppTokens, pLocale and ppWordBreaker may be NULL.
    [PreserveSig]
    HResult GetLexicalData([MarshalAs(UnmanagedType.LPWStr)] out string ppszInputString, [Out] /* ITokenCollection** */ out IntPtr ppTokens, [Out] out uint plcid, [Out] /* IUnknown** */ out IntPtr ppWordBreaker);
}