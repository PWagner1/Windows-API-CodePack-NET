﻿//Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.WindowsAPICodePack.Shell
{
    /// <summary>
    /// Represents a standard system icon.
    /// </summary>
    public class StockIcon : IDisposable
    {
        #region Private Members

        private StockIconIdentifier _identifier;
        private StockIconSize _currentSize = StockIconSize.Large;
        private bool _linkOverlay;
        private bool _selected;
        private bool _invalidateIcon;
        private IntPtr _hIcon = IntPtr.Zero;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Creates a new StockIcon instance with the specified identifer, default size 
        /// and no link overlay or selected states.
        /// </summary>
        /// <param name="id">A value that identifies the icon represented by this instance.</param>
        public StockIcon(StockIconIdentifier id)
        {
            _identifier = id;
            _invalidateIcon = true;
        }

        /// <summary>
        /// Creates a new StockIcon instance with the specified identifer and options.
        /// </summary>
        /// <param name="id">A value that identifies the icon represented by this instance.</param>
        /// <param name="size">A value that indicates the size of the stock icon.</param>
        /// <param name="isLinkOverlay">A bool value that indicates whether the icon has a link overlay.</param>
        /// <param name="isSelected">A bool value that indicates whether the icon is in a selected state.</param>
        public StockIcon(StockIconIdentifier id, StockIconSize size, bool isLinkOverlay, bool isSelected)
        {
            _identifier = id;
            _linkOverlay = isLinkOverlay;
            _selected = isSelected;
            _currentSize = size;
            _invalidateIcon = true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether the icon appears selected.
        /// </summary>
        /// <value>A <see cref="System.Boolean"/> value.</value>
        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                _invalidateIcon = true;
            }
        }

        /// <summary>
        /// Gets or sets a value that cotrols whether to put a link overlay on the icon.
        /// </summary>
        /// <value>A <see cref="System.Boolean"/> value.</value>
        public bool LinkOverlay
        {
            get => _linkOverlay;
            set
            {
                _linkOverlay = value;
                _invalidateIcon = true;
            }
        }

        /// <summary>
        /// Gets or sets a value that controls the size of the Stock Icon.
        /// </summary>
        /// <value>A <see cref="Microsoft.WindowsAPICodePack.Shell.StockIconSize"/> value.</value>
        public StockIconSize CurrentSize
        {
            get => _currentSize;
            set
            {
                _currentSize = value;
                _invalidateIcon = true;
            }
        }

        /// <summary>
        /// Gets or sets the Stock Icon identifier associated with this icon.
        /// </summary>
        public StockIconIdentifier Identifier
        {
            get => _identifier;
            set
            {
                _identifier = value;
                _invalidateIcon = true;
            }
        }

        /// <summary>
        /// Gets the icon image in <see cref="System.Drawing.Bitmap"/> format. 
        /// </summary>
        public Bitmap? Bitmap
        {
            get
            {
                UpdateHIcon();

                return _hIcon != IntPtr.Zero ? Bitmap.FromHicon(_hIcon) : null;
            }
        }

        /// <summary>
        /// Gets the icon image in <see cref="System.Windows.Media.Imaging.BitmapSource"/> format. 
        /// </summary>
        public BitmapSource? BitmapSource
        {
            get
            {
                UpdateHIcon();

                return (_hIcon != IntPtr.Zero) ?
                    Imaging.CreateBitmapSourceFromHIcon(_hIcon, Int32Rect.Empty, null) : null;
            }
        }

        /// <summary>
        /// Gets the icon image in <see cref="System.Drawing.Icon"/> format.
        /// </summary>
        public Icon? Icon
        {
            get
            {
                UpdateHIcon();

                return _hIcon != IntPtr.Zero ? Icon.FromHandle(_hIcon) : null;
            }
        }

        #endregion

        #region Private Methods

        private void UpdateHIcon()
        {
            if (_invalidateIcon)
            {
                if (_hIcon != IntPtr.Zero)
                    CoreNativeMethods.DestroyIcon(_hIcon);

                _hIcon = GetHIcon();

                _invalidateIcon = false;
            }
        }

        private IntPtr GetHIcon()
        {
            // Create our internal flag to pass to the native method
            StockIconsNativeMethods.StockIconOptions flags = StockIconsNativeMethods.StockIconOptions.Handle;

            // Based on the current settings, update the flags
            if (CurrentSize == StockIconSize.Small)
            {
                flags |= StockIconsNativeMethods.StockIconOptions.Small;
            }
            else if (CurrentSize == StockIconSize.ShellSize)
            {
                flags |= StockIconsNativeMethods.StockIconOptions.ShellSize;
            }
            else
            {
                flags |= StockIconsNativeMethods.StockIconOptions.Large;  // default
            }

            if (Selected)
            {
                flags |= StockIconsNativeMethods.StockIconOptions.Selected;
            }

            if (LinkOverlay)
            {
                flags |= StockIconsNativeMethods.StockIconOptions.LinkOverlay;
            }

            // Create a StockIconInfo structure to pass to the native method.
            StockIconsNativeMethods.StockIconInfo info = new();
            info.StuctureSize = (uint)Marshal.SizeOf(typeof(StockIconsNativeMethods.StockIconInfo));

            // Pass the struct to the native method
            HResult hr = StockIconsNativeMethods.SHGetStockIconInfo(_identifier, flags, ref info);

            // If we get an error, return null as the icon requested might not be supported
            // on the current system
            if (hr != HResult.Ok)
            {
                if (hr == HResult.InvalidArguments)
                {
                    throw new InvalidOperationException(
                        string.Format(CultureInfo.InvariantCulture,
                        LocalizedMessages.StockIconInvalidGuid,
                        _identifier));
                }

                return IntPtr.Zero;
            }

            // If we succeed, return the HIcon
            return info.Handle;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Release the native and managed objects
        /// </summary>
        /// <param name="disposing">Indicates that this is being called from Dispose(), rather than the finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources here
            }

            // Unmanaged resources
            if (_hIcon != IntPtr.Zero)
                CoreNativeMethods.DestroyIcon(_hIcon);
        }

        /// <summary>
        /// Release the native objects
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        ~StockIcon()
        {
            Dispose(false);
        }

        #endregion
    }
}

