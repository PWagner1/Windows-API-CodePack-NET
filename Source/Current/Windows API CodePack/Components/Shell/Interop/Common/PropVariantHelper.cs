namespace Microsoft.WindowsAPICodePack.Shell.Interop;

internal static class PropVariantHelper
{
    [DllImport("propsys.dll", CharSet = CharSet.Unicode)]
    public static extern int InitPropVariantFromString(
        [MarshalAs(UnmanagedType.LPWStr)] string psz,
        out PROPVARIANT ppropvar);
}