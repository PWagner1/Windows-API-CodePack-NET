//Copyright (c) Microsoft Corporation.  All rights reserved.

// ReSharper disable ConditionIsAlwaysTrueOrFalse
#pragma warning disable CS8602
namespace Microsoft.WindowsAPICodePack.Dialogs
{
    /// <summary>
    /// Creates a Vista or Windows 7 Common File Dialog, allowing the user to select the filename and location for a saved file.
    /// </summary>
    /// <permission cref="System.Security.Permissions.FileDialogPermission">
    /// to save a file. Associated enumeration: <see cref="System.Security.Permissions.SecurityAction.LinkDemand"/>.
    /// </permission>
    public sealed class CommonSaveFileDialog : CommonFileDialog
    {
        private NativeFileSaveDialog? _saveDialogCoClass;

        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        public CommonSaveFileDialog() { }
        /// <summary>
        /// Creates a new instance of this class with the specified name.
        /// </summary>
        /// <param name="name">The name of this dialog.</param>
        public CommonSaveFileDialog(string? name) : base(name) { }

        #region Public API specific to Save

        private bool _overwritePrompt = true;
        /// <summary>
        /// Gets or sets a value that controls whether to prompt before 
        /// overwriting an existing file of the same name. Default value is true.
        /// </summary>
        /// <permission cref="System.InvalidOperationException">
        /// This property cannot be changed when the dialog is showing.
        /// </permission>
        public bool OverwritePrompt
        {
            get => _overwritePrompt;
            set
            {
                ThrowIfDialogShowing(LocalizedMessages.OverwritePromptCannotBeChanged);
                _overwritePrompt = value;
            }
        }

        private bool _createPrompt;
        /// <summary>
        /// Gets or sets a value that controls whether to prompt for creation if the item returned in the save dialog does not exist. 
        /// </summary>
        /// <remarks>Note that this does not actually create the item.</remarks>
        /// <permission cref="System.InvalidOperationException">
        /// This property cannot be changed when the dialog is showing.
        /// </permission>
        public bool CreatePrompt
        {
            get => _createPrompt;
            set
            {
                ThrowIfDialogShowing(LocalizedMessages.CreatePromptCannotBeChanged);
                _createPrompt = value;
            }
        }

        private bool _isExpandedMode;
        /// <summary>
        /// Gets or sets a value that controls whether to the save dialog 
        /// displays in expanded mode. 
        /// </summary>
        /// <remarks>Expanded mode controls whether the dialog
        /// shows folders for browsing or hides them.</remarks>
        /// <permission cref="System.InvalidOperationException">
        /// This property cannot be changed when the dialog is showing.
        /// </permission>
        public bool IsExpandedMode
        {
            get => _isExpandedMode;
            set
            {
                ThrowIfDialogShowing(LocalizedMessages.IsExpandedModeCannotBeChanged);
                _isExpandedMode = value;
            }
        }

        private bool _alwaysAppendDefaultExtension;
        /// <summary>
        /// Gets or sets a value that controls whether the 
        /// returned file name has a file extension that matches the 
        /// currently selected file type.  If necessary, the dialog appends the correct 
        /// file extension.
        /// </summary>
        /// <permission cref="System.InvalidOperationException">
        /// This property cannot be changed when the dialog is showing.
        /// </permission>
        public bool AlwaysAppendDefaultExtension
        {
            get => _alwaysAppendDefaultExtension;
            set
            {
                ThrowIfDialogShowing(LocalizedMessages.AlwaysAppendDefaultExtensionCannotBeChanged);
                _alwaysAppendDefaultExtension = value;
            }
        }

        /// <summary>
        /// Sets an item to appear as the initial entry in a <b>Save As</b> dialog.
        /// </summary>
        /// <param name="item">The initial entry to be set in the dialog.</param>
        /// <remarks>The name of the item is displayed in the file name edit box, 
        /// and the containing folder is opened in the view. This would generally be 
        /// used when the application is saving an item that already exists.</remarks>
        public void SetSaveAsItem(ShellObject item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            InitializeNativeFileDialog();
            IFileSaveDialog? nativeDialog = GetNativeFileDialog() as IFileSaveDialog;

            // Get the native IShellItem from ShellObject
            if (nativeDialog != null)
            {
                nativeDialog.SetSaveAsItem(item.NativeShellItem);
            }
        }

        /// <summary>
        /// Specifies which properties will be collected in the save dialog.
        /// </summary>
        /// <param name="appendDefault">True to show default properties for the currently selected 
        /// filetype in addition to the properties specified by propertyList. False to show only properties 
        /// specified by pList.
        /// <param name="propertyList">List of properties to collect. This parameter can be null.</param>
        /// </param>
        /// <param name="propertyList"></param>
        /// <remarks>
        /// SetCollectedPropertyKeys can be called at any time before the dialog is displayed or while it 
        /// is visible. If different properties are to be collected depending on the chosen filetype, 
        /// then SetCollectedProperties can be called in response to CommonFileDialog::FileTypeChanged event.
        /// Note: By default, no properties are collected in the save dialog.
        /// </remarks>
        public void SetCollectedPropertyKeys(bool appendDefault, params PropertyKey[] propertyList)
        {
            // Loop through all our property keys and create a semicolon-delimited property list string.
            // The string we pass to PSGetPropertyDescriptionListFromString must
            // start with "prop:", followed a list of canonical names for each 
            // property that is to collected.
            if (propertyList != null && propertyList.Length > 0 && propertyList[0] != null)
            {
                StringBuilder sb = new StringBuilder("prop:");
                foreach (PropertyKey key in propertyList)
                {
                    if (ShellPropertyDescriptionsCache.Cache != null)
                    {
                        string canonicalName = ShellPropertyDescriptionsCache.Cache.GetPropertyDescription(key).CanonicalName;
                        if (!string.IsNullOrEmpty(canonicalName)) { sb.AppendFormat("{0};", canonicalName); }
                    }
                }

                Guid guid = new Guid(ShellIIDGuid.IPropertyDescriptionList);
                IPropertyDescriptionList propertyDescriptionList = null;

                try
                {
                    int hr = PropertySystemNativeMethods.PSGetPropertyDescriptionListFromString(
                        sb.ToString(),
                        ref guid,
                        out propertyDescriptionList);

                    // If we get a IPropertyDescriptionList, setit on the native dialog.
                    if (CoreErrorHelper.Succeeded(hr))
                    {
                        InitializeNativeFileDialog();
                        IFileSaveDialog nativeDialog = GetNativeFileDialog() as IFileSaveDialog;

                        if (nativeDialog != null)
                        {
                            hr = nativeDialog.SetCollectedProperties(propertyDescriptionList, appendDefault);

                            if (!CoreErrorHelper.Succeeded(hr))
                            {
                                throw new ShellException(hr);
                            }
                        }
                    }
                }
                finally
                {
                    if (propertyDescriptionList != null)
                    {
                        Marshal.ReleaseComObject(propertyDescriptionList);
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves the set of property values for a saved item or an item in the process of being saved.
        /// </summary>
        /// <returns>Collection of property values collected from the save dialog</returns>
        /// <remarks>This property can be called while the dialog is showing to retrieve the current 
        /// set of values in the metadata collection pane. It can also be called after the dialog 
        /// has closed, to retrieve the final set of values. The call to this method will fail 
        /// unless property collection has been turned on with a call to SetCollectedPropertyKeys method.
        /// </remarks>
        public ShellPropertyCollection CollectedProperties
        {
            get
            {
                InitializeNativeFileDialog();
                IFileSaveDialog nativeDialog = GetNativeFileDialog() as IFileSaveDialog;

                if (nativeDialog != null)
                {
                    IPropertyStore? propertyStore;
                    HResult hr = nativeDialog.GetProperties(out propertyStore);

                    if (propertyStore != null && CoreErrorHelper.Succeeded(hr))
                    {
                        return new ShellPropertyCollection(propertyStore);
                    }
                }

                return null;
            }
        }

        #endregion

        internal override void InitializeNativeFileDialog()
        {
            if (_saveDialogCoClass == null)
            {
                _saveDialogCoClass = new NativeFileSaveDialog();
            }
        }

        internal override IFileDialog? GetNativeFileDialog()
        {
            Debug.Assert(_saveDialogCoClass != null, "Must call Initialize() before fetching dialog interface");
            return (IFileDialog)_saveDialogCoClass;
        }

        internal override void PopulateWithFileNames(
            Collection<string> names)
        {
            IShellItem? item;
            _saveDialogCoClass.GetResult(out item);

            if (item == null)
            {
                throw new InvalidOperationException(LocalizedMessages.SaveFileNullItem);
            }
            names.Clear();
            names.Add(GetFileNameFromShellItem(item));
        }

        internal override void PopulateWithIShellItems(Collection<IShellItem?> items)
        {
            IShellItem? item;
            _saveDialogCoClass.GetResult(out item);

            if (item == null)
            {
                throw new InvalidOperationException(LocalizedMessages.SaveFileNullItem);
            }
            items.Clear();
            items.Add(item);
        }

        internal override void CleanUpNativeFileDialog()
        {
            if (_saveDialogCoClass != null)
            {
                Marshal.ReleaseComObject(_saveDialogCoClass);
            }
        }

        internal override ShellNativeMethods.FileOpenOptions GetDerivedOptionFlags(ShellNativeMethods.FileOpenOptions flags)
        {
            if (_overwritePrompt)
            {
                flags |= ShellNativeMethods.FileOpenOptions.OverwritePrompt;
            }
            if (_createPrompt)
            {
                flags |= ShellNativeMethods.FileOpenOptions.CreatePrompt;
            }
            if (!_isExpandedMode)
            {
                flags |= ShellNativeMethods.FileOpenOptions.DefaultNoMiniMode;
            }
            if (_alwaysAppendDefaultExtension)
            {
                flags |= ShellNativeMethods.FileOpenOptions.StrictFileTypes;
            }
            return flags;
        }
    }
}
