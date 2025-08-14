namespace Microsoft.WindowsAPICodePack.ShellExtensions;

/// <summary>
/// This interface exposes the <see cref="Load"/> function for initializing the 
/// Preview Handler with a <typeparamref name="ShellObject"/>.
/// This interface can be used in conjunction with the other intialization interfaces,
/// but only 1 will be accessed according to the priorities preset by the Windows Shell:
/// <typeparamref name="IPreviewFromStream"/>
/// <typeparamref name="IPreviewFromShellObject"/>
/// <typeparamref name="IPreviewFromFile"/>
/// </summary>
public interface IPreviewFromShellObject
{
    /// <summary>
    /// Provides the <typeparamref name="ShellObject"/> from which a preview should be created.        
    /// </summary>
    /// <param name="shellObject">ShellObject for the previewed file, this ShellObject is only available in the scope of this method.</param>
    void Load(ShellObject? shellObject);
}