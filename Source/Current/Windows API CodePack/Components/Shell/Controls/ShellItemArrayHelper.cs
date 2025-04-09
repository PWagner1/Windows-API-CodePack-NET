namespace Microsoft.WindowsAPICodePack.Controls.WindowsForms
{
    internal static class ShellItemArrayHelper
    {
        public static IShellItemArray FromKnownFolder(IKnownFolder folder)
        {
            IShellItem? shellItem;
            Guid iidShellItem = new Guid(ShellIIDGuid.IShellItem);

            int hr = ShellNativeMethods.SHCreateItemFromParsingName(folder.Path, IntPtr.Zero, ref iidShellItem, out shellItem);
            if (hr != 0)
                Marshal.ThrowExceptionForHR(hr);

            IShellItemArray array = new ShellItemArrayWrapper(shellItem);
            return array;
        }
    }
}