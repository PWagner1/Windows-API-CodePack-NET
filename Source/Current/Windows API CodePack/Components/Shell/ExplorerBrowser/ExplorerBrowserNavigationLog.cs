//Copyright (c) Microsoft Corporation.  All rights reserved.

// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
namespace Microsoft.WindowsAPICodePack.Controls;

/// <summary>
/// The navigation log is a history of the locations visited by the explorer browser. 
/// </summary>
public class ExplorerBrowserNavigationLog
{
    #region operations
    /// <summary>
    /// Clears the contents of the navigation log.
    /// </summary>
    public void ClearLog()
    {
        // nothing to do
        if (_locations.Count == 0) { return; }

        bool oldCanNavigateBackward = CanNavigateBackward;
        bool oldCanNavigateForward = CanNavigateForward;

        _locations.Clear();
        _currentLocationIndex = -1;

        NavigationLogEventArgs args = new()
        {
            LocationsChanged = true,
            CanNavigateBackwardChanged = (oldCanNavigateBackward != CanNavigateBackward),
            CanNavigateForwardChanged = (oldCanNavigateForward != CanNavigateForward)
        };
        if (NavigationLogChanged != null)
        {
            NavigationLogChanged(this, args);
        }
    }
    #endregion

    #region properties
    /// <summary>
    /// Indicates the presence of locations in the log that can be 
    /// reached by calling Navigate(Forward)
    /// </summary>
    public bool CanNavigateForward => (CurrentLocationIndex < (_locations.Count - 1));

    /// <summary>
    /// Indicates the presence of locations in the log that can be 
    /// reached by calling Navigate(Backward)
    /// </summary>
    public bool CanNavigateBackward => (CurrentLocationIndex > 0);

    /// <summary>
    /// The navigation log
    /// </summary>
    public IEnumerable<ShellObject?> Locations
    {
        get { foreach (var obj in _locations) { yield return obj; } }
    }
    private readonly List<ShellObject?> _locations = new();

    /// <summary>
    /// An index into the Locations collection. The ShellObject pointed to 
    /// by this index is the current location of the ExplorerBrowser.
    /// </summary>
    public int CurrentLocationIndex => _currentLocationIndex;


    /// <summary>
    /// Gets the shell object in the Locations collection pointed to
    /// by CurrentLocationIndex.
    /// </summary>
    public ShellObject? CurrentLocation
    {
        get
        {
            if (_currentLocationIndex < 0) { return null; }

            return _locations[_currentLocationIndex];
        }
    }
    #endregion

    #region events
    /// <summary>
    /// Fires when the navigation log changes or 
    /// the current navigation position changes
    /// </summary>
    public event EventHandler<NavigationLogEventArgs>? NavigationLogChanged;
    #endregion

    #region implementation

    private readonly ExplorerBrowser? _parent;

    /// <summary>
    /// The pending navigation log action. null if the user is not navigating 
    /// via the navigation log.
    /// </summary>
    private PendingNavigation? _pendingNavigation;

    /// <summary>
    /// The index into the Locations collection. -1 if the Locations collection 
    /// is empty.
    /// </summary>
    private int _currentLocationIndex = -1;

    internal ExplorerBrowserNavigationLog(ExplorerBrowser? parent)
    {
        // Hook navigation events from the parent to distinguish between
        // navigation log induced navigation, and other navigations.
        _parent = parent ?? throw new ArgumentException(LocalizedMessages.NavigationLogNullParent, nameof(parent));
        _parent.NavigationComplete += OnNavigationComplete;
        _parent.NavigationFailed += OnNavigationFailed;
    }

    private void OnNavigationFailed(object? sender, NavigationFailedEventArgs args) => _pendingNavigation = null;

    private void OnNavigationComplete(object? sender, NavigationCompleteEventArgs args)
    {
        NavigationLogEventArgs eventArgs = new();
        bool oldCanNavigateBackward = CanNavigateBackward;
        bool oldCanNavigateForward = CanNavigateForward;

        if ((_pendingNavigation != null))
        {
            // navigation log traversal in progress

            // determine if new location is the same as the traversal request
            int result = 0;
            _pendingNavigation.Location?.NativeShellItem?.Compare(
                args.NewLocation?.NativeShellItem, SICHINTF.SICHINT_ALLFIELDS, out result);
            bool shellItemsEqual = (result == 0);
            if (shellItemsEqual == false)
            {
                // new location is different than traversal request, 
                // behave is if it never happened!
                // remove history following currentLocationIndex, append new item
                if (_currentLocationIndex < (_locations.Count - 1))
                {
                    _locations.RemoveRange(_currentLocationIndex + 1, _locations.Count - (_currentLocationIndex + 1));
                }
                _locations.Add(args.NewLocation);
                _currentLocationIndex = (_locations.Count - 1);
                eventArgs.LocationsChanged = true;
            }
            else
            {
                // log traversal successful, update index
                _currentLocationIndex = _pendingNavigation.Index;
                eventArgs.LocationsChanged = false;
            }
            _pendingNavigation = null;
        }
        else
        {
            // remove history following currentLocationIndex, append new item
            if (_currentLocationIndex < (_locations.Count - 1))
            {
                _locations.RemoveRange(_currentLocationIndex + 1, _locations.Count - (_currentLocationIndex + 1));
            }
            _locations.Add(args.NewLocation);
            _currentLocationIndex = (_locations.Count - 1);
            eventArgs.LocationsChanged = true;
        }

        // update event args
        eventArgs.CanNavigateBackwardChanged = (oldCanNavigateBackward != CanNavigateBackward);
        eventArgs.CanNavigateForwardChanged = (oldCanNavigateForward != CanNavigateForward);

        if (NavigationLogChanged != null)
        {
            NavigationLogChanged(this, eventArgs);
        }
    }

    internal bool NavigateLog(NavigationLogDirection direction)
    {
        // determine proper index to navigate to
        int locationIndex;
        if (direction == NavigationLogDirection.Backward && CanNavigateBackward)
        {
            locationIndex = (_currentLocationIndex - 1);
        }
        else if (direction == NavigationLogDirection.Forward && CanNavigateForward)
        {
            locationIndex = (_currentLocationIndex + 1);
        }
        else
        {
            return false;
        }

        // initiate traversal request
        ShellObject? location = _locations[locationIndex];
        _pendingNavigation = new PendingNavigation(location, locationIndex);
        _parent?.Navigate(location);
        return true;
    }

    internal bool NavigateLog(int index)
    {
        // can't go anywhere
        if (index >= _locations.Count || index < 0) { return false; }

        // no need to re navigate to the same location
        if (index == _currentLocationIndex) { return false; }

        // initiate traversal request
        ShellObject? location = _locations[index];
        _pendingNavigation = new PendingNavigation(location, index);
        _parent?.Navigate(location);
        return true;
    }

    #endregion
}