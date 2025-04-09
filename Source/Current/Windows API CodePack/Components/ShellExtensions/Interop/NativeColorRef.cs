namespace Microsoft.WindowsAPICodePack.ShellExtensions.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct NativeColorRef
    {
        public uint Dword { get; set; }
    }
}