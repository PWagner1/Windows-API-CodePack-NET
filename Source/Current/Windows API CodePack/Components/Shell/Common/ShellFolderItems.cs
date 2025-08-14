//Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.WindowsAPICodePack.Shell;

class ShellFolderItems : IEnumerator<ShellObject>
{
    #region Private Fields

    private IEnumIDList? _nativeEnumIdList;
    private ShellObject? _currentItem;
    readonly ShellContainer _nativeShellFolder;

    #endregion

    #region Internal Constructor

    internal ShellFolderItems(ShellContainer nativeShellFolder)
    {
        _nativeShellFolder = nativeShellFolder;

        HResult hr = nativeShellFolder!.NativeShellFolder!.EnumObjects(
            IntPtr.Zero,
            ShellNativeMethods.ShellFolderEnumerationOptions.Folders | ShellNativeMethods.ShellFolderEnumerationOptions.NonFolders,
            out _nativeEnumIdList);


        if (!CoreErrorHelper.Succeeded(hr))
        {
            if (hr == HResult.Canceled)
            {
                throw new FileNotFoundException();
            }
            else
            {
                throw new ShellException(hr);
            }
        }


    }

    #endregion

    #region IEnumerator<ShellObject> Members

#pragma warning disable CS8766
    public ShellObject? Current => _currentItem;
#pragma warning restore CS8766

    #endregion

    #region IDisposable Members

    public void Dispose()
    {
        if (_nativeEnumIdList != null)
        {
            Marshal.ReleaseComObject(_nativeEnumIdList);
            _nativeEnumIdList = null;
        }
    }

    #endregion

    #region IEnumerator Members

    object? IEnumerator.Current => _currentItem;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool MoveNext()
    {
        if (_nativeEnumIdList == null) { return false; }

        IntPtr item;
        uint numItemsReturned;
        uint itemsRequested = 1;
        HResult hr = _nativeEnumIdList.Next(itemsRequested, out item, out numItemsReturned);

        if (numItemsReturned < itemsRequested || hr != HResult.Ok) { return false; }

        _currentItem = ShellObjectFactory.Create(item, _nativeShellFolder);

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Reset()
    {
        if (_nativeEnumIdList != null)
        {
            _nativeEnumIdList.Reset();
        }
    }


    #endregion
}