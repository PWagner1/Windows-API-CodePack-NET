//Copyright (c) Microsoft Corporation.  All rights reserved.

// ReSharper disable IdentifierTypo
// ReSharper disable UseNameofExpression
// ReSharper disable InconsistentNaming
// ReSharper disable SuspiciousTypeConversion.Global

namespace Microsoft.WindowsAPICodePack.Taskbar
{
    /// <summary>
    /// Represents a jump list link object.
    /// </summary>
    public class JumpListLink : JumpListTask, IJumpListItem, IDisposable
    {
        internal static PropertyKey _pKeyTitle = SystemProperties.System.Title;

        /// <summary>
        /// Initializes a new instance of a JumpListLink with the specified path.
        /// </summary>
        /// <param name="pathValue">The path to the item. The path is required for the JumpList Link</param>
        /// <param name="titleValue">The title for the JumpListLink item. The title is required for the JumpList link.</param>
        public JumpListLink(string? pathValue, string? titleValue)
        {
            if (string.IsNullOrEmpty(pathValue))
            {
                throw new ArgumentNullException("pathValue", LocalizedMessages.JumpListLinkPathRequired);
            }

            if (string.IsNullOrEmpty(titleValue))
            {
                throw new ArgumentNullException("titleValue", LocalizedMessages.JumpListLinkTitleRequired);
            }

            Path = pathValue;
            Title = titleValue;
        }

        private string? _title;
        /// <summary>
        /// Gets or sets the link's title
        /// </summary>
        public string? Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value", LocalizedMessages.JumpListLinkTitleRequired);
                }

                _title = value;
            }
        }

        private string? _path;
        /// <summary>
        /// Gets or sets the link's path
        /// </summary>
        public string? Path
        {
            get => _path;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value", LocalizedMessages.JumpListLinkTitleRequired);
                }

                _path = value;
            }
        }

        /// <summary>
        /// Gets or sets the icon reference (location and index) of the link's icon.
        /// </summary>
        public IconReference IconReference { get; set; }

        /// <summary>
        /// Gets or sets the object's arguments (passed to the command line).
        /// </summary>
        public string? Arguments { get; set; }

        /// <summary>
        /// Gets or sets the object's working directory.
        /// </summary>
        public string? WorkingDirectory { get; set; }

        /// <summary>
        /// Gets or sets the show command of the lauched application.
        /// </summary>
        public WindowShowCommand ShowCommand { get; set; }

        private IPropertyStore? _nativePropertyStore;
        private IShellLinkW? _nativeShellLink;
        /// <summary>
        /// Gets an IShellLinkW representation of this object
        /// </summary>
        internal override IShellLinkW? NativeShellLink
        {
            get
            {
                if (_nativeShellLink != null)
                {
                    Marshal.ReleaseComObject(_nativeShellLink);
                    _nativeShellLink = null;
                }

                _nativeShellLink = (IShellLinkW)new CShellLink();

                if (_nativePropertyStore != null)
                {
                    Marshal.ReleaseComObject(_nativePropertyStore);
                    _nativePropertyStore = null;
                }

                _nativePropertyStore = (IPropertyStore)_nativeShellLink;

                _nativeShellLink.SetPath(Path);

                if (!string.IsNullOrEmpty(IconReference.ModuleName))
                {
                    _nativeShellLink.SetIconLocation(IconReference.ModuleName, IconReference.ResourceId);
                }

                if (!string.IsNullOrEmpty(Arguments))
                {
                    _nativeShellLink.SetArguments(Arguments);
                }

                if (!string.IsNullOrEmpty(WorkingDirectory))
                {
                    _nativeShellLink.SetWorkingDirectory(WorkingDirectory);
                }

                _nativeShellLink.SetShowCmd((uint)ShowCommand);

                using (PropVariant propVariant = new(Title))
                {
                    HResult result = _nativePropertyStore.SetValue(ref _pKeyTitle, propVariant);
                    if (!CoreErrorHelper.Succeeded(result))
                    {
                        throw new ShellException(result);
                    }

                    _nativePropertyStore.Commit();
                }

                return _nativeShellLink;
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Release the native and managed objects
        /// </summary>
        /// <param name="disposing">Indicates that this is being called from Dispose(), rather than the finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _title = null;
            }

            if (_nativePropertyStore != null)
            {
                Marshal.ReleaseComObject(_nativePropertyStore);
                _nativePropertyStore = null;
            }

            if (_nativeShellLink != null)
            {
                Marshal.ReleaseComObject(_nativeShellLink);
                _nativeShellLink = null;
            }
        }

        /// <summary>
        /// Release the native objects.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Implement the finalizer.
        /// </summary>
        ~JumpListLink()
        {
            Dispose(false);
        }

        #endregion

    }
}
