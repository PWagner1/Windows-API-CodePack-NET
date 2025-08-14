//Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.WindowsAPICodePack.Shell;

/// <summary>
/// A file in the Shell Namespace
/// </summary>
public class ShellFile : ShellObject
{
    #region Internal Constructor

    [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    internal ShellFile(string? path)
    {
        // Get the absolute path
        string? absPath = ShellHelper.GetAbsolutePath(path);

        // Make sure this is valid
        if (!File.Exists(absPath))
        {
            throw new FileNotFoundException(
                string.Format(CultureInfo.InvariantCulture,
                    LocalizedMessages.FilePathNotExist, path));
        }

        ParsingName = absPath;
    }

    internal ShellFile(IShellItem2? shellItem)
    {
        nativeShellItem = shellItem;
    }

    #endregion

    #region Public Methods
    /// <summary>
    /// Constructs a new ShellFile object given a file path
    /// </summary>
    /// <param name="path">The file or folder path</param>
    /// <returns>ShellFile object created using given file path.</returns>
    static public ShellFile FromFilePath(string? path)
    {
        return new ShellFile(path);
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// The path for this file
    /// </summary>
    virtual public string? Path => ParsingName;

    #endregion
}