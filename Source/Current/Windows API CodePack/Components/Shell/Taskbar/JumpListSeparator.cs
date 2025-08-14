//Copyright (c) Microsoft Corporation.  All rights reserved.

using SystemProperties = Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties;
// ReSharper disable SuspiciousTypeConversion.Global

namespace Microsoft.WindowsAPICodePack.Taskbar;

/// <summary>
/// Represents a separator in the user task list. The JumpListSeparator control
/// can only be used in a user task list.
/// </summary>
public class JumpListSeparator : JumpListTask, IDisposable
{
    internal static PropertyKey _pKeyAppUserModelIsDestListSeparator = SystemProperties.System.AppUserModel.IsDestinationListSeparator;

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

            using (PropVariant propVariant = new(true))
            {
                HResult result = _nativePropertyStore.SetValue(ref _pKeyAppUserModelIsDestListSeparator, propVariant);
                if (!CoreErrorHelper.Succeeded(result))
                {
                    throw new ShellException(result);
                }
                _nativePropertyStore.Commit();
            }

            return _nativeShellLink; ;
        }
    }

    #region IDisposable Members

    /// <summary>
    /// Release the native and managed objects
    /// </summary>
    /// <param name="disposing">Indicates that this is being called from Dispose(), rather than the finalizer.</param>
    protected virtual void Dispose(bool disposing)
    {
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
    ~JumpListSeparator()
    {
        Dispose(false);
    }

    #endregion

}