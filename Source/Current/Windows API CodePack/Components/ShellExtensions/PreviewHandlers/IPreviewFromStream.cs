namespace Microsoft.WindowsAPICodePack.ShellExtensions;

/// <summary>
/// This interface exposes the <see cref="Load"/> function for initializing the 
/// Preview Handler with a <see cref="Stream"/>.
/// This interface can be used in conjunction with the other intialization interfaces,
/// but only 1 will be accessed according to the priorities preset by the Windows Shell:
/// <see cref="IPreviewFromStream"/>,
/// <see cref="IPreviewFromShellObject"/>,
/// <see cref="IPreviewFromFile"/>.
/// </summary>
public interface IPreviewFromStream
{
    /// <summary>
    /// Provides the <see cref="Stream"/> to the item from which a preview should be created.        
    /// </summary>
    /// <param name="stream">Stream to the previewed file, this stream is only available in the scope of this method.</param>
    void Load(Stream stream);
}