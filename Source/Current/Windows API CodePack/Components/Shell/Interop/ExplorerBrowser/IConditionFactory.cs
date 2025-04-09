namespace Microsoft.WindowsAPICodePack.Shell.Interop
{
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("A5EFE073-B16F-474f-9F3E-9F8B497A3E08")]
    internal interface IConditionFactory
    {
        void MakeLeaf(
            [MarshalAs(UnmanagedType.LPWStr)] string propertyName,
            CONDITION_OPERATION operation,
            PropVariant value,
            [MarshalAs(UnmanagedType.LPWStr)] string propertyNameTerm,
            [MarshalAs(UnmanagedType.Interface)] object propertyTerm,
            out ICondition condition);
    }
}
