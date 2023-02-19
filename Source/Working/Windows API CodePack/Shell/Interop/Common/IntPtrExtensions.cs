namespace Microsoft.WindowsAPICodePack.Shell
{
    internal static class IntPtrExtensions
    {
        public static T MarshalAs<T>(this IntPtr ptr)
        {
            return (T)Marshal.PtrToStructure(ptr, typeof(T));
        }
    }
}
