//Copyright (c) Microsoft Corporation.  All rights reserved.

// ReSharper disable AssignNullToNotNullAttribute
namespace MS.WindowsAPICodePack.Internal;

/// <summary>
/// This provides a connection point container compatible dispatch interface for
/// hooking into the ExplorerBrowser view.
/// </summary>    
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.AutoDual)]
[Obsolete(@"ClassInterfaceType.AutoDual is marked as obsolete.")]
public class ExplorerBrowserViewEvents : IDisposable
{
    #region implementation
    private uint _viewConnectionPointCookie;
    private object? _viewDispatch;

    [SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
    private IntPtr _nullPtr = IntPtr.Zero;

    private Guid _iidDShellFolderViewEvents = new(ExplorerBrowserIIDGuid.DShellFolderViewEvents);
    private Guid _iidIDispatch = new(ExplorerBrowserIIDGuid.IDispatch);
    private ExplorerBrowser? _parent;
    #endregion

    #region contstruction
    /// <summary>
    /// Default constructor for ExplorerBrowserViewEvents
    /// </summary>
    public ExplorerBrowserViewEvents() : this(null) { }

    internal ExplorerBrowserViewEvents(ExplorerBrowser? parent)
    {
        _parent = parent;
    }
    #endregion

    #region operations
    internal void ConnectToView(IShellView psv)
    {
        DisconnectFromView();

        HResult hr = psv.GetItemObject(
            ShellViewGetItemObject.Background,
            ref _iidIDispatch,
            out _viewDispatch);

        if (hr == HResult.Ok)
        {
            hr = ExplorerBrowserNativeMethods.ConnectToConnectionPoint(
                this,
                ref _iidDShellFolderViewEvents,
                true,
                _viewDispatch,
                ref _viewConnectionPointCookie,
                ref _nullPtr);

            if (hr != HResult.Ok)
            {
                if (_viewDispatch != null)
                {
                    Marshal.ReleaseComObject(_viewDispatch);
                }
            }
        }
    }

    internal void DisconnectFromView()
    {
        if (_viewDispatch != null)
        {
            ExplorerBrowserNativeMethods.ConnectToConnectionPoint(
                IntPtr.Zero,
                ref _iidDShellFolderViewEvents,
                false,
                _viewDispatch,
                ref _viewConnectionPointCookie,
                ref _nullPtr);

            Marshal.ReleaseComObject(_viewDispatch);
            _viewDispatch = null;
            _viewConnectionPointCookie = 0;
        }
    }
    #endregion

    #region IDispatch events
    // These need to be public to be accessible via AutoDual reflection

    /// <summary>
    /// The view selection has changed
    /// </summary>
    [DispId(ExplorerBrowserViewDispatchIds.SelectionChanged)]
    public void ViewSelectionChanged() => _parent?.FireSelectionChanged();

    /// <summary>
    /// The contents of the view have changed
    /// </summary>
    [DispId(ExplorerBrowserViewDispatchIds.ContentsChanged)]
    public void ViewContentsChanged() => _parent?.FireContentChanged();

    /// <summary>
    /// The enumeration of files in the view is complete
    /// </summary>
    [DispId(ExplorerBrowserViewDispatchIds.FileListEnumDone)]
    public void ViewFileListEnumDone() => _parent?.FireContentEnumerationComplete();

    /// <summary>
    /// The selected item in the view has changed (not the same as the selection has changed)
    /// </summary>
    [DispId(ExplorerBrowserViewDispatchIds.SelectedItemChanged)]
    public void ViewSelectedItemChanged() => _parent?.FireSelectedItemChanged();

    #endregion

    /// <summary>
    /// Finalizer for ExplorerBrowserViewEvents
    /// </summary>
    ~ExplorerBrowserViewEvents()
    {
        Dispose(false);
    }

    #region IDisposable Members

    /// <summary>
    /// Disconnects and disposes object.
    /// </summary>        
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disconnects and disposes object.
    /// </summary>
    /// <param name="disposed"></param>
    protected virtual void Dispose(bool disposed)
    {
        if (disposed)
        {
            DisconnectFromView();
        }
    }

    #endregion
}