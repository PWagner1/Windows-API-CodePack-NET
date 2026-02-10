namespace Microsoft.WindowsAPICodePack.ShellExtensions;

/// <summary>
/// This interface exposes the <see cref="Load"/> function for initializing the 
/// Preview Handler with a <see cref="FileInfo"/>.
/// This interface can be used in conjunction with the other intialization interfaces,
/// but only 1 will be accessed according to the priorities preset by the Windows Shell:
/// <see cref="IPreviewFromStream"/>,
/// <see cref="IPreviewFromShellObject"/>,
/// <see cref="IPreviewFromFile"/>.
/// </summary>
public interface IPreviewFromFile
{
    /// <summary>
    /// Provides the <see cref="FileInfo"/> to the item from which a preview should be created.        
    /// </summary>
    /// <param name="info">File information to the previewed file.</param>
    void Load(FileInfo info);
}