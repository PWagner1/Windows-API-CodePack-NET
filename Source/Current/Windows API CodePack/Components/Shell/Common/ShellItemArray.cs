//Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.WindowsAPICodePack.Shell;

internal class ShellItemArray : IShellItemArray
{
    readonly List<IShellItem?> _shellItemsList = new();

    internal ShellItemArray(IShellItem?[] shellItems)
    {
        _shellItemsList.AddRange(shellItems);
    }

    #region IShellItemArray Members

    public HResult BindToHandler(IntPtr pbc, ref Guid rbhid, ref Guid riid, out IntPtr ppvOut)
    {
        throw new NotSupportedException();
    }

    public HResult GetPropertyStore(int flags, ref Guid riid, out IntPtr ppv)
    {
        throw new NotSupportedException();
    }

    public HResult GetPropertyDescriptionList(ref PropertyKey keyType, ref Guid riid, out IntPtr ppv)
    {
        throw new NotSupportedException();
    }

    public HResult GetAttributes(ShellNativeMethods.ShellItemAttributeOptions dwAttribFlags, ShellNativeMethods.ShellFileGetAttributesOptions sfgaoMask, out ShellNativeMethods.ShellFileGetAttributesOptions psfgaoAttribs)
    {
        throw new NotSupportedException();
    }

    public HResult GetCount(out uint pdwNumItems)
    {
        pdwNumItems = (uint)_shellItemsList.Count;
        return HResult.Ok;
    }

    public HResult GetItemAt(uint dwIndex, out IShellItem? ppsi)
    {
        int index = (int)dwIndex;

        if (index < _shellItemsList.Count)
        {
            ppsi = _shellItemsList[index];
            return HResult.Ok;
        }
        else
        {
            ppsi = null;
            return HResult.Fail;
        }
    }

    public HResult EnumItems(out IntPtr ppenumShellItems)
    {
        throw new NotSupportedException();
    }

    #endregion
}