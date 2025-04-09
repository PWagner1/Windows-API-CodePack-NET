namespace Microsoft.WindowsAPICodePack.Controls.WindowsForms
{
    internal class ShellItemArrayWrapper : IShellItemArray
    {
        private readonly IShellItem? _item;

        public ShellItemArrayWrapper(IShellItem? item)
        {
            _item = item;
        }

        public void BindToHandler(IntPtr pbc, ref Guid bhid, ref Guid riid, out IntPtr ppvOut) => throw new NotImplementedException();
        HResult IShellItemArray.GetPropertyStore(int Flags, ref Guid riid, out IntPtr ppv)
        {
            throw new NotImplementedException();
        }

        HResult IShellItemArray.GetPropertyDescriptionList(ref PropertyKey keyType, ref Guid riid, out IntPtr ppv)
        {
            throw new NotImplementedException();
        }

        public HResult GetAttributes(ShellNativeMethods.ShellItemAttributeOptions dwAttribFlags, ShellNativeMethods.ShellFileGetAttributesOptions sfgaoMask,
            out ShellNativeMethods.ShellFileGetAttributesOptions psfgaoAttribs)
        {
            throw new NotImplementedException();
        }

        HResult IShellItemArray.GetCount(out uint pdwNumItems)
        {
            throw new NotImplementedException();
        }

        HResult IShellItemArray.GetItemAt(uint dwIndex, out IShellItem? ppsi)
        {
            throw new NotImplementedException();
        }

        HResult IShellItemArray.EnumItems(out IntPtr ppenumShellItems)
        {
            throw new NotImplementedException();
        }

        HResult IShellItemArray.BindToHandler(IntPtr pbc, ref Guid rbhid, ref Guid riid, out IntPtr ppvOut)
        {
            throw new NotImplementedException();
        }

        public void GetPropertyStore(int flags, ref Guid riid, out IntPtr ppv) => throw new NotImplementedException();
        public void GetPropertyDescriptionList(ref PropertyKey keyType, ref Guid riid, out IntPtr ppv) => throw new NotImplementedException();
        public void GetAttributes(SIATTRIBFLAGS dwAttribFlags, uint sfgaoMask, out uint psfgaoAttribs) => throw new NotImplementedException();
        public void GetCount(out uint pdwNumItems) => pdwNumItems = 1;
        public void GetItemAt(uint dwIndex, out IShellItem? ppsi) => ppsi = _item;
        public void EnumItems(out IntPtr ppenumShellItems) => throw new NotImplementedException();
    }
}