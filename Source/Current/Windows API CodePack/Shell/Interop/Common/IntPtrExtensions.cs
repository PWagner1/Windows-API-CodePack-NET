#pragma warning disable CS8600, CS8603
namespace Microsoft.WindowsAPICodePack.Shell
{
    internal static class IntPtrExtensions
    {
        public static T MarshalAs<T>(this IntPtr ptr) => (T)Marshal.PtrToStructure(ptr, typeof(T));
    }
}
