//Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.WindowsAPICodePack.Shell
{
    class ShellFolderItems : IEnumerator<ShellObject>
    {
        #region Private Fields

        private IEnumIDList nativeEnumIdList;
        private ShellObject? currentItem;
        readonly ShellContainer nativeShellFolder;

        #endregion

        #region Internal Constructor

        internal ShellFolderItems(ShellContainer nativeShellFolder)
        {
            this.nativeShellFolder = nativeShellFolder;

            HResult hr = nativeShellFolder.NativeShellFolder.EnumObjects(
                IntPtr.Zero,
                ShellNativeMethods.ShellFolderEnumerationOptions.Folders | ShellNativeMethods.ShellFolderEnumerationOptions.NonFolders,
                out nativeEnumIdList);


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
        public ShellObject? Current => currentItem;
#pragma warning restore CS8766

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (nativeEnumIdList != null)
            {
                Marshal.ReleaseComObject(nativeEnumIdList);
                nativeEnumIdList = null;                
            }            
        }

        #endregion

        #region IEnumerator Members

        object? IEnumerator.Current => currentItem;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            if (nativeEnumIdList == null) { return false; }

            IntPtr item;
            uint numItemsReturned;
            uint itemsRequested = 1;
            HResult hr = nativeEnumIdList.Next(itemsRequested, out item, out numItemsReturned);

            if (numItemsReturned < itemsRequested || hr != HResult.Ok) { return false; }

            currentItem = ShellObjectFactory.Create(item, nativeShellFolder);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            if (nativeEnumIdList != null)
            {
                nativeEnumIdList.Reset();
            }
        }


        #endregion
    }
}
