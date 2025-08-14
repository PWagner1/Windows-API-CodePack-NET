//Copyright (c) Microsoft Corporation.  All rights reserved.

// ReSharper disable InlineOutVariableDeclaration
#pragma warning disable CS8600

namespace Microsoft.WindowsAPICodePack.Dialogs;

/// <summary>
/// Creates a Vista or Windows 7 Common File Dialog, allowing the user to select one or more files.
/// </summary>
/// 
public sealed class CommonOpenFileDialog : CommonFileDialog
{
    private NativeFileOpenDialog? _openDialogCoClass;

    /// <summary>
    /// Creates a new instance of this class.
    /// </summary>
    public CommonOpenFileDialog()
        : base()
    {
        // For Open file dialog, allow read only files.
        EnsureReadOnly = true;
    }

    /// <summary>
    /// Creates a new instance of this class with the specified name.
    /// </summary>
    /// <param name="name">The name of this dialog.</param>
    public CommonOpenFileDialog(string? name)
        : base(name)
    {
        // For Open file dialog, allow read only files.
        EnsureReadOnly = true;
    }

    #region Public API specific to Open

    /// <summary>
    /// Gets a collection of the selected file names.
    /// </summary>
    /// <remarks>This property should only be used when the
    /// <see cref="CommonOpenFileDialog.Multiselect"/>
    /// property is <b>true</b>.</remarks>
    public IEnumerable<string?> FileNames
    {
        get
        {
            CheckFileNamesAvailable();
            return FileNameCollection;
        }
    }

    /// <summary>
    /// Gets a collection of the selected items as ShellObject objects.
    /// </summary>
    /// <remarks>This property should only be used when the
    /// <see cref="CommonOpenFileDialog.Multiselect"/>
    /// property is <b>true</b>.</remarks>
    public ICollection<ShellObject?> FilesAsShellObject
    {
        get
        {
            // Check if we have selected files from the user.              
            CheckFileItemsAvailable();

            // temp collection to hold our shellobjects
            ICollection<ShellObject?> resultItems = new Collection<ShellObject?>();

            // Loop through our existing list of filenames, and try to create a concrete type of
            // ShellObject (e.g. ShellLibrary, FileSystemFolder, ShellFile, etc)
            foreach (IShellItem? si in Items)
            {
                resultItems.Add(ShellObjectFactory.Create(si));
            }

            return resultItems;
        }
    }


    private bool _multiselect;
    /// <summary>
    /// Gets or sets a value that determines whether the user can select more than one file.
    /// </summary>
    public bool Multiselect
    {
        get => _multiselect;
        set => _multiselect = value;
    }

    private bool _isFolderPicker;
    /// <summary>
    /// Gets or sets a value that determines whether the user can select folders or files.
    /// Default value is false.
    /// </summary>
    public bool IsFolderPicker
    {
        get => _isFolderPicker;
        set => _isFolderPicker = value;
    }

    private bool _allowNonFileSystem;
    /// <summary>
    /// Gets or sets a value that determines whether the user can select non-filesystem items, 
    /// such as <b>Library</b>, <b>Search Connectors</b>, or <b>Known Folders</b>.
    /// </summary>
    public bool AllowNonFileSystemItems
    {
        get => _allowNonFileSystem;
        set => _allowNonFileSystem = value;
    }
    #endregion

    internal override IFileDialog? GetNativeFileDialog()
    {
        Debug.Assert(_openDialogCoClass != null, "Must call Initialize() before fetching dialog interface");

        return _openDialogCoClass;
    }

    internal override void InitializeNativeFileDialog()
    {
        if (_openDialogCoClass == null)
        {
            _openDialogCoClass = new NativeFileOpenDialog();
        }
    }

    internal override void CleanUpNativeFileDialog()
    {
        if (_openDialogCoClass != null)
        {
            Marshal.ReleaseComObject(_openDialogCoClass);
        }
    }

    internal override void PopulateWithFileNames(Collection<string?> names)
    {
        IShellItemArray resultsArray;
        uint count;

        _openDialogCoClass!.GetResults(out resultsArray);
        resultsArray.GetCount(out count);
        names.Clear();
        for (int i = 0; i < count; i++)
        {
            names.Add(GetFileNameFromShellItem(GetShellItemAt(resultsArray, i)));
        }
    }

    internal override void PopulateWithIShellItems(Collection<IShellItem> items)
    {
        IShellItemArray resultsArray;
        uint count;

        _openDialogCoClass!.GetResults(out resultsArray);
        resultsArray.GetCount(out count);
        items.Clear();
        for (int i = 0; i < count; i++)
        {
            items.Add(GetShellItemAt(resultsArray, i)!);
        }
    }

    internal override ShellNativeMethods.FileOpenOptions GetDerivedOptionFlags(ShellNativeMethods.FileOpenOptions flags)
    {
        if (_multiselect)
        {
            flags |= ShellNativeMethods.FileOpenOptions.AllowMultiSelect;
        }
        if (_isFolderPicker)
        {
            flags |= ShellNativeMethods.FileOpenOptions.PickFolders;
        }

        if (!_allowNonFileSystem)
        {
            flags |= ShellNativeMethods.FileOpenOptions.ForceFilesystem;
        }
        else if (_allowNonFileSystem)
        {
            flags |= ShellNativeMethods.FileOpenOptions.AllNonStorageItems;
        }

        return flags;
    }
}