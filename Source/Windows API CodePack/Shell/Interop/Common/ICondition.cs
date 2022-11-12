namespace Microsoft.WindowsAPICodePack.Shell;

[ComImport(),
 Guid(ShellIIDGuid.ICondition),
 InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface ICondition : IPersistStream
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

    // For any node, return what kind of node it is.
    [PreserveSig]
    HResult GetConditionType([Out()] out SearchConditionType pNodeType);

    // riid must be IID_IEnumUnknown, IID_IEnumVARIANT or IID_IObjectArray, or in the case of a negation node IID_ICondition.
    // If this is a leaf node, E_FAIL will be returned.
    // If this is a negation node, then if riid is IID_ICondition, *ppv will be set to a single ICondition, otherwise an enumeration of one.
    // If this is a conjunction or a disjunction, *ppv will be set to an enumeration of the subconditions.
    [PreserveSig]
    HResult GetSubConditions([In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out object ppv);

    // If this is not a leaf node, E_FAIL will be returned.
    // Retrieve the property name, operation and value from the leaf node.
    // Any one of ppszPropertyName, pcop and ppropvar may be NULL.
    [PreserveSig]
    HResult GetComparisonInfo(
        [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszPropertyName,
        [Out] out SearchConditionOperation pcop,
        [Out] PropVariant ppropvar);

    // If this is not a leaf node, E_FAIL will be returned.
    // *ppszValueTypeName will be set to the semantic type of the value, or to NULL if this is not meaningful.
    [PreserveSig]
    HResult GetValueType([Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszValueTypeName);

    // If this is not a leaf node, E_FAIL will be returned.
    // If the value of the leaf node is VT_EMPTY, *ppszNormalization will be set to an empty string.
    // If the value is a string (VT_LPWSTR, VT_BSTR or VT_LPSTR), then *ppszNormalization will be set to a
    // character-normalized form of the value.
    // Otherwise, *ppszNormalization will be set to some (character-normalized) string representation of the value.
    [PreserveSig]
    HResult GetValueNormalization([Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszNormalization);

    // Return information about what parts of the input produced the property, the operation and the value.
    // Any one of ppPropertyTerm, ppOperationTerm and ppValueTerm may be NULL.
    // For a leaf node returned by the parser, the position information of each IRichChunk identifies the tokens that
    // contributed the property/operation/value, the string value is the corresponding part of the input string, and
    // the PROPVARIANT is VT_EMPTY.
    [PreserveSig]
    HResult GetInputTerms([Out] out IRichChunk ppPropertyTerm, [Out] out IRichChunk ppOperationTerm, [Out] out IRichChunk ppValueTerm);

    // Make a deep copy of this ICondition.
    [PreserveSig]
    HResult Clone([Out()] out ICondition ppc);
};