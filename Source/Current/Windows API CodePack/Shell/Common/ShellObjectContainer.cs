//Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.WindowsAPICodePack.Shell
{
    /// <summary>
    /// Represents the base class for all types of Shell "containers". Any class deriving from this class
    /// can contain other ShellObjects (e.g. ShellFolder, FileSystemKnownFolder, ShellLibrary, etc)
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "This will complicate the class hierarchy and naming convention used in the Shell area")]
    public abstract class ShellContainer : ShellObject, IEnumerable<ShellObject>, IDisposable
    {

        #region Private Fields

        private IShellFolder? _desktopFolderEnumeration;
        private IShellFolder? _nativeShellFolder;

        #endregion

        #region Internal Properties

        internal IShellFolder? NativeShellFolder
        {
            get
            {
                if (_nativeShellFolder == null)
                {
                    Guid guid = new(ShellIIDGuid.IShellFolder);
                    Guid handler = new(ShellBHIDGuid.ShellFolderObject);

                    if (NativeShellItem != null)
                    {
                        HResult hr = NativeShellItem.BindToHandler(
                            IntPtr.Zero, ref handler, ref guid, out _nativeShellFolder);

                        if (CoreErrorHelper.Failed(hr))
                        {
                            string? str = ShellHelper.GetParsingName(NativeShellItem);
                            if (str != null && str != Environment.GetFolderPath(Environment.SpecialFolder.Desktop))
                            {
                                throw new ShellException(hr);
                            }
                        }
                    }
                }

                return _nativeShellFolder;
            }
        }

        #endregion

        #region Internal Constructor

        internal ShellContainer() { }

        internal ShellContainer(IShellItem2? shellItem) : base(shellItem) { }

        #endregion

        #region Disposable Pattern

        /// <summary>
        /// Release resources
        /// </summary>
        /// <param name="disposing"><B>True</B> indicates that this is being called from Dispose(), rather than the finalizer.</param>
        protected override void Dispose(bool disposing)
        {
            if (_nativeShellFolder != null)
            {
                Marshal.ReleaseComObject(_nativeShellFolder);
                _nativeShellFolder = null;
            }

            if (_desktopFolderEnumeration != null)
            {
                Marshal.ReleaseComObject(_desktopFolderEnumeration);
                _desktopFolderEnumeration = null;
            }

            base.Dispose(disposing);
        }

        #endregion

        #region IEnumerable<ShellObject> Members

        /// <summary>
        /// Enumerates through contents of the ShellObjectContainer
        /// </summary>
        /// <returns>Enumerated contents</returns>
        public IEnumerator<ShellObject> GetEnumerator()
        {
            if (NativeShellFolder == null)
            {
                if (_desktopFolderEnumeration == null)
                {
                    ShellNativeMethods.SHGetDesktopFolder(out _desktopFolderEnumeration);
                }

                _nativeShellFolder = _desktopFolderEnumeration;
            }

            return new ShellFolderItems(this);
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ShellFolderItems(this);
        }

        #endregion
    }
}
