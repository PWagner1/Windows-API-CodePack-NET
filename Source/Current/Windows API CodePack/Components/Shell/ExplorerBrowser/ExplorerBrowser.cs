//Copyright (c) Microsoft Corporation.  All rights reserved.

using Application = System.Windows.Forms.Application;
using Brushes = System.Drawing.Brushes;
using Color = System.Drawing.Color;
using LinearGradientBrush = System.Drawing.Drawing2D.LinearGradientBrush;
using MethodInvoker = System.Windows.Forms.MethodInvoker;

namespace Microsoft.WindowsAPICodePack.Controls.WindowsForms;

/// <summary>
/// This class is a wrapper around the Windows Explorer Browser control.
/// </summary>
public sealed class ExplorerBrowser :
    UserControl,
    IServiceProvider,
    IExplorerPaneVisibility,
    IExplorerBrowserEvents,
    ICommDlgBrowser3,
    IMessageFilter
{
    #region Instance Fields

    private int? _totalItemsCache;
    private bool _enumerationCompleteFired;

    #endregion

    #region Properties
    /// <summary>
    /// Options that control how the ExplorerBrowser navigates
    /// </summary>
    public ExplorerBrowserNavigationOptions NavigationOptions { get; private set; }

    /// <summary>
    /// Options that control how the content of the ExplorerBorwser looks
    /// </summary>
    public ExplorerBrowserContentOptions ContentOptions { get; private set; }

    private IShellItemArray? _shellItemsArray;
    private ShellObjectCollection? _itemsCollection;
    /// <summary>
    /// The set of ShellObjects in the Explorer Browser
    /// </summary>
    public ShellObjectCollection? Items
    {
        get
        {
            if (_shellItemsArray != null)
            {
                Marshal.ReleaseComObject(_shellItemsArray);
            }

            if (_itemsCollection != null)
            {
                _itemsCollection.Dispose();
                _itemsCollection = null;
            }

            _shellItemsArray = GetItemsArray();
            _itemsCollection = new ShellObjectCollection(_shellItemsArray, true);

            return _itemsCollection;
        }
    }

    private IShellItemArray? _selectedShellItemsArray;
    private ShellObjectCollection? _selectedItemsCollection;
    /// <summary>
    /// The set of selected ShellObjects in the Explorer Browser
    /// </summary>
    public ShellObjectCollection? SelectedItems
    {
        get
        {
            if (_selectedShellItemsArray != null)
            {
                Marshal.ReleaseComObject(_selectedShellItemsArray);
            }

            if (_selectedItemsCollection != null)
            {
                _selectedItemsCollection.Dispose();
                _selectedItemsCollection = null;
            }

            _selectedShellItemsArray = GetSelectedItemsArray();
            _selectedItemsCollection = new ShellObjectCollection(_selectedShellItemsArray, true);

            return _selectedItemsCollection;
        }
    }

    /// <summary>
    /// Contains the navigation history of the ExplorerBrowser
    /// </summary>
    public ExplorerBrowserNavigationLog NavigationLog { get; private set; }

    /// <summary>
    /// The name of the property bag used to persist changes to the ExplorerBrowser's view state.
    /// </summary>
    public string PropertyBagName
    {
        get => _propertyBagName;
        set
        {
            _propertyBagName = value;
            if (ExplorerBrowserControl != null)
            {
                ExplorerBrowserControl.SetPropertyBag(_propertyBagName);
            }
        }
    }

    // Search State Properties
    private readonly ExplorerBrowserSearchState _searchState = new();

    /// <summary>
    /// Gets the search state properties for the ExplorerBrowser.
    /// </summary>
    [Description("Search state properties for the ExplorerBrowser.")]
    [Category("Search")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public ExplorerBrowserSearchState SearchState => _searchState;

    /// <summary>
    /// Gets whether the current view is showing search results.
    /// </summary>
    [Obsolete("Use SearchState.IsShowingSearchResults instead.")]
    public bool IsShowingSearchResults => Items?.FirstOrDefault() is ShellSearchFolder;

    /// <summary>
    /// Updates the search state based on current items.
    /// </summary>
    private void UpdateSearchState()
    {
        SearchState.IsShowingSearchResults = Items?.FirstOrDefault() is ShellSearchFolder;
    }

    /// <summary>
    /// Gets or sets the current search query. Returns null if no search is active.
    /// </summary>
    [Obsolete("Use SearchState.CurrentSearchQuery instead.")]
    public string? CurrentSearchQuery => SearchState.CurrentSearchQuery;

    /// <summary>
    /// Gets or sets the current search scope. Returns null if no search is active.
    /// </summary>
    [Obsolete("Use SearchState.CurrentSearchScope instead.")]
    public ShellContainer? CurrentSearchScope => SearchState.CurrentSearchScope;

    // Search Options Properties
    private readonly ExplorerBrowserSearchOptions _searchOptions = new();

    /// <summary>
    /// Gets the search options properties for the ExplorerBrowser.
    /// </summary>
    [Description("Search options properties for the ExplorerBrowser.")]
    [Category("Search")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public ExplorerBrowserSearchOptions SearchOptions => _searchOptions;

    // Legacy properties for backward compatibility
    [Obsolete("Use SearchOptions.AutoRefreshSearchResults instead.")]
    public bool AutoRefreshSearchResults { get => SearchOptions.AutoRefreshSearchResults; set => SearchOptions.AutoRefreshSearchResults = value; }

    [Obsolete("Use SearchOptions.MaxSearchResults instead.")]
    public int MaxSearchResults { get => SearchOptions.MaxSearchResults; set => SearchOptions.MaxSearchResults = value; }

    [Obsolete("Use SearchOptions.ShowSearchProgress instead.")]
    public bool ShowSearchProgress { get => SearchOptions.ShowSearchProgress; set => SearchOptions.ShowSearchProgress = value; }

    [Obsolete("Use SearchOptions.CacheSearchResults instead.")]
    public bool CacheSearchResults { get => SearchOptions.CacheSearchResults; set => SearchOptions.CacheSearchResults = value; }

    [Obsolete("Use SearchOptions.SearchResultSortOrder instead.")]
    public SearchResultSortOrder SearchResultSortOrder { get => SearchOptions.SearchResultSortOrder; set => SearchOptions.SearchResultSortOrder = value; }

    [Obsolete("Use SearchOptions.GroupSearchResultsByType instead.")]
    public SearchResultGrouping GroupSearchResultsByType
    {
        get => SearchOptions.GroupSearchResultsByType;
        set => SearchOptions.GroupSearchResultsByType = value;
    }

    [Obsolete("Use SearchOptions.ShowFilePreviews instead.")]
    public bool ShowFilePreviews { get => SearchOptions.ShowFilePreviews; set => SearchOptions.ShowFilePreviews = value; }

    [Obsolete("Use SearchOptions.SearchResultThumbnailSize instead.")]
    public int SearchResultThumbnailSize { get => SearchOptions.SearchResultThumbnailSize; set => SearchOptions.SearchResultThumbnailSize = value; }

    [Obsolete("Use SearchOptions.EnableIncrementalSearch instead.")]
    public bool EnableIncrementalSearch { get => SearchOptions.EnableIncrementalSearch; set => SearchOptions.EnableIncrementalSearch = value; }

    [Obsolete("Use SearchOptions.IncrementalSearchDelay instead.")]
    public int IncrementalSearchDelay { get => SearchOptions.IncrementalSearchDelay; set => SearchOptions.IncrementalSearchDelay = value; }

    [Obsolete("Use SearchOptions.ShowSearchSuggestions instead.")]
    public bool ShowSearchSuggestions { get => SearchOptions.ShowSearchSuggestions; set => SearchOptions.ShowSearchSuggestions = value; }

    [Obsolete("Use SearchOptions.MaxSearchSuggestions instead.")]
    public int MaxSearchSuggestions { get => SearchOptions.MaxSearchSuggestions; set => SearchOptions.MaxSearchSuggestions = value; }

    [Obsolete("Use SearchOptions.HighlightSearchTerms instead.")]
    public bool HighlightSearchTerms { get => SearchOptions.HighlightSearchTerms; set => SearchOptions.HighlightSearchTerms = value; }

    [Obsolete("Use SearchOptions.SearchTermHighlightColor instead.")]
    public Color SearchTermHighlightColor { get => SearchOptions.SearchTermHighlightColor; set => SearchOptions.SearchTermHighlightColor = value; }

    [Obsolete("Use SearchOptions.EnableAdvancedSearchFilters instead.")]
    public bool EnableAdvancedSearchFilters { get => SearchOptions.EnableAdvancedSearchFilters; set => SearchOptions.EnableAdvancedSearchFilters = value; }

    [Obsolete("Use SearchOptions.RememberSearchHistory instead.")]
    public bool RememberSearchHistory { get => SearchOptions.RememberSearchHistory; set => SearchOptions.RememberSearchHistory = value; }

    [Obsolete("Use SearchOptions.MaxSearchHistoryEntries instead.")]
    public int MaxSearchHistoryEntries { get => SearchOptions.MaxSearchHistoryEntries; set => SearchOptions.MaxSearchHistoryEntries = value; }

    [Obsolete("Use SearchOptions.EnableSearchResultExport instead.")]
    public bool EnableSearchResultExport { get => SearchOptions.EnableSearchResultExport; set => SearchOptions.EnableSearchResultExport = value; }

    [Obsolete("Use SearchOptions.DefaultExportFormat instead.")]
    public SearchResultExportFormat DefaultExportFormat { get => SearchOptions.DefaultExportFormat; set => SearchOptions.DefaultExportFormat = value; }

    [Obsolete("Use SearchOptions.ShowSearchStatistics instead.")]
    public bool ShowSearchStatistics { get => SearchOptions.ShowSearchStatistics; set => SearchOptions.ShowSearchStatistics = value; }

    [Obsolete("Use SearchOptions.EnableSearchResultSharing instead.")]
    public bool EnableSearchResultSharing { get => SearchOptions.EnableSearchResultSharing; set => SearchOptions.EnableSearchResultSharing = value; }

    [Obsolete("Use SearchOptions.ShowSearchResultContextMenu instead.")]
    public bool ShowSearchResultContextMenu { get => SearchOptions.ShowSearchResultContextMenu; set => SearchOptions.ShowSearchResultContextMenu = value; }

    [Obsolete("Use SearchOptions.EnableSearchResultDragDrop instead.")]
    public bool EnableSearchResultDragDrop { get => SearchOptions.EnableSearchResultDragDrop; set => SearchOptions.EnableSearchResultDragDrop = value; }

    [Obsolete("Use SearchOptions.ShowSearchResultTooltips instead.")]
    public bool ShowSearchResultTooltips { get => SearchOptions.ShowSearchResultTooltips; set => SearchOptions.ShowSearchResultTooltips = value; }

    [Obsolete("Use SearchOptions.PersistSearchResultSelection instead.")]
    public bool PersistSearchResultSelection { get => SearchOptions.PersistSearchResultSelection; set => SearchOptions.PersistSearchResultSelection = value; }

    [Obsolete("Use SearchOptions.ShowSearchResultCount instead.")]
    public bool ShowSearchResultCount { get => SearchOptions.ShowSearchResultCount; set => SearchOptions.ShowSearchResultCount = value; }

    [Obsolete("Use SearchOptions.EnableVirtualScrolling instead.")]
    public bool EnableVirtualScrolling { get => SearchOptions.EnableVirtualScrolling; set => SearchOptions.EnableVirtualScrolling = value; }

    [Obsolete("Use SearchOptions.VirtualScrollingPageSize instead.")]
    public int VirtualScrollingPageSize { get => SearchOptions.VirtualScrollingPageSize; set => SearchOptions.VirtualScrollingPageSize = value; }

    #endregion

    #region Private Methods

    /// <summary>
    /// Applies sorting to search results based on the SearchResultSortOrder property.
    /// </summary>
    /// <param name="searchCondition">The base search condition to apply sorting to.</param>
    private void ApplySearchResultSorting(SearchCondition searchCondition)
    {
        // This is a placeholder for future implementation
        // The actual sorting would be applied when creating the ShellSearchFolder
        // or through additional search parameters
    }

    /// <summary>
    /// Applies grouping to search results based on the GroupSearchResultsByType property.
    /// </summary>
    private void ApplySearchResultGrouping()
    {
        // This is a placeholder for future implementation
        // The actual grouping would be applied through folder view settings
    }

    /// <summary>
    /// Applies filtering to search results based on the current filter settings.
    /// </summary>
    private void ApplySearchResultFiltering()
    {
        // This is a placeholder for future implementation
        // The actual filtering would be applied through additional search conditions
    }

    #endregion

    #region operations
    /// <summary>
    /// Clears the Explorer Browser of existing content, fills it with
    /// content from the specified container, and adds a new point to the Travel Log.
    /// </summary>
    /// <param name="shellObject">The shell container to navigate to.</param>
    /// <exception cref="System.Runtime.InteropServices.COMException">Will throw if navigation fails for any other reason.</exception>
    public void Navigate(ShellObject? shellObject)
    {
        if (shellObject == null)
        {
            throw new ArgumentNullException(nameof(shellObject));
        }

        if (ExplorerBrowserControl == null)
        {
            _antecreationNavigationTarget = shellObject;
        }
        else
        {
            HResult hr = ExplorerBrowserControl.BrowseToObject(shellObject.NativeShellItem, 0);
            if (hr != HResult.Ok)
            {
                if ((hr == HResult.ResourceInUse || hr == HResult.Canceled) && NavigationFailed != null)
                {
                    NavigationFailedEventArgs args = new();
                    args.FailedLocation = shellObject;
                    NavigationFailed(this, args);
                }
                else
                {
                    throw new CommonControlException(LocalizedMessages.ExplorerBrowserBrowseToObjectFailed, hr);
                }
            }
        }
    }

    /// <summary>
    /// Navigates within the navigation log. This does not change the set of 
    /// locations in the navigation log.
    /// </summary>
    /// <param name="direction">Forward of Backward</param>
    /// <returns>True if the navigation succeeded, false if it failed for any reason.</returns>
    public bool NavigateLogLocation(NavigationLogDirection direction)
    {
        return NavigationLog.NavigateLog(direction);
    }

    /// <summary>
    /// Navigate within the navigation log. This does not change the set of 
    /// locations in the navigation log.
    /// </summary>
    /// <param name="navigationLogIndex">An index into the navigation logs Locations collection.</param>
    /// <returns>True if the navigation succeeded, false if it failed for any reason.</returns>
    public bool NavigateLogLocation(int navigationLogIndex)
    {
        return NavigationLog.NavigateLog(navigationLogIndex);
    }

    /// <summary>
    /// Performs a search for files and folders within the specified scope and navigates to the results.
    /// </summary>
    /// <param name="searchQuery">The search query string to search for in filenames.</param>
    /// <param name="searchScope">The scope where to perform the search (e.g., KnownFolders.Documents).</param>
    /// <exception cref="System.ArgumentException">Thrown when searchQuery is null or empty.</exception>
    /// <exception cref="System.ArgumentNullException">Thrown when searchScope is null.</exception>
    /// <exception cref="System.Runtime.InteropServices.COMException">Thrown when search fails for any other reason.</exception>
    public void Search(string searchQuery, ShellContainer searchScope)
    {
        if (string.IsNullOrWhiteSpace(searchQuery))
        {
            throw new ArgumentException("Search query must not be empty.", nameof(searchQuery));
        }

        if (searchScope == null)
        {
            throw new ArgumentNullException(nameof(searchScope));
        }

        try
        {
            // Update current search state
            SearchState.CurrentSearchQuery = searchQuery;
            SearchState.CurrentSearchScope = searchScope;

            // Create search condition for filename search
            var searchCondition = SearchConditionFactory.CreateLeafCondition(
                "System.FileName",
                searchQuery,
                SearchConditionOperation.ValueContains
            );

            // Apply search result sorting if specified
            if (SearchResultSortOrder != SearchResultSortOrder.Relevance)
            {
                // Apply sorting based on the selected order
                ApplySearchResultSorting(searchCondition);
            }

            // Create search folder and navigate to results
            using (var searchFolder = new ShellSearchFolder(searchCondition, searchScope))
            {
                Navigate(searchFolder);
            }
        }
        catch (Exception ex)
        {
            throw new CommonControlException($"Search failed: {ex.Message}", HResult.Fail);
        }
    }

    /// <summary>
    /// Performs a search using a custom search condition and navigates to the results.
    /// </summary>
    /// <param name="searchCondition">The custom search condition to use.</param>
    /// <param name="searchScope">The scope where to perform the search.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when searchCondition or searchScope is null.</exception>
    /// <exception cref="System.Runtime.InteropServices.COMException">Thrown when search fails for any other reason.</exception>
    public void Search(SearchCondition searchCondition, ShellContainer searchScope)
    {
        if (searchCondition == null)
        {
            throw new ArgumentNullException(nameof(searchCondition));
        }

        if (searchScope == null)
        {
            throw new ArgumentNullException(nameof(searchScope));
        }

        try
        {
            // Update current search state
            SearchState.CurrentSearchQuery = searchCondition.ToString();
            SearchState.CurrentSearchScope = searchScope;

            using (var searchFolder = new ShellSearchFolder(searchCondition, searchScope))
            {
                Navigate(searchFolder);
            }
        }
        catch (Exception ex)
        {
            throw new CommonControlException($"Search failed: {ex.Message}", HResult.Fail);
        }
    }

    /// <summary>
    /// Performs a search using Windows Search syntax and navigates to the results.
    /// </summary>
    /// <param name="queryString">The search query in Windows Search format (e.g., "kind:picture AND datemodified:today").</param>
    /// <param name="searchScope">The scope where to perform the search.</param>
    /// <param name="cultureInfo">The culture info for parsing the query. If null, uses CurrentCulture.</param>
    /// <exception cref="System.ArgumentException">Thrown when queryString is null or empty.</exception>
    /// <exception cref="System.ArgumentNullException">Thrown when searchScope is null.</exception>
    /// <exception cref="System.Runtime.InteropServices.COMException">Thrown when search fails for any other reason.</exception>
    public void SearchWithQueryString(string queryString, ShellContainer searchScope, System.Globalization.CultureInfo? cultureInfo = null)
    {
        if (string.IsNullOrWhiteSpace(queryString))
        {
            throw new ArgumentException("Query string must not be empty.", nameof(queryString));
        }

        if (searchScope == null)
        {
            throw new ArgumentNullException(nameof(searchScope));
        }

        try
        {
            // Parse structured query (supports Windows Search syntax)
            var searchCondition = SearchConditionFactory.ParseStructuredQuery(
                queryString,
                cultureInfo ?? System.Globalization.CultureInfo.CurrentCulture
            );

            // Update current search state
            SearchState.CurrentSearchQuery = queryString;
            SearchState.CurrentSearchScope = searchScope;

            using (var searchFolder = new ShellSearchFolder(searchCondition, searchScope))
            {
                Navigate(searchFolder);
            }
        }
        catch (Exception ex)
        {
            throw new CommonControlException($"Search with query string failed: {ex.Message}", HResult.Fail);
        }
    }

    /// <summary>
    /// Performs a search for files modified within a date range and navigates to the results.
    /// </summary>
    /// <param name="startDate">The start date for the search range.</param>
    /// <param name="endDate">The end date for the search range.</param>
    /// <param name="searchScope">The scope where to perform the search.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when searchScope is null.</exception>
    /// <exception cref="System.Runtime.InteropServices.COMException">Thrown when search fails for any other reason.</exception>
    public void SearchByDateRange(DateTime startDate, DateTime endDate, ShellContainer searchScope)
    {
        if (searchScope == null)
        {
            throw new ArgumentNullException(nameof(searchScope));
        }

        try
        {
            // Create date range search condition
            var startCondition = SearchConditionFactory.CreateLeafCondition(
                "System.DateModified",
                startDate,
                SearchConditionOperation.GreaterThanOrEqual
            );

            var endCondition = SearchConditionFactory.CreateLeafCondition(
                "System.DateModified",
                endDate,
                SearchConditionOperation.LessThanOrEqual
            );

            // Combine conditions with AND logic
            var combinedCondition = SearchConditionFactory.CreateAndOrCondition(
                SearchConditionType.And, true, startCondition, endCondition
            );

            Search(combinedCondition, searchScope);
        }
        catch (Exception ex)
        {
            throw new CommonControlException($"Date range search failed: {ex.Message}", HResult.Fail);
        }
    }

    /// <summary>
    /// Performs a search for files larger than a specified size and navigates to the results.
    /// </summary>
    /// <param name="minSizeBytes">The minimum file size in bytes.</param>
    /// <param name="searchScope">The scope where to perform the search.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when searchScope is null.</exception>
    /// <exception cref="System.Runtime.InteropServices.COMException">Thrown when search fails for any other reason.</exception>
    public void SearchByFileSize(long minSizeBytes, ShellContainer searchScope)
    {
        if (searchScope == null)
        {
            throw new ArgumentNullException(nameof(searchScope));
        }

        try
        {
            var sizeCondition = SearchConditionFactory.CreateLeafCondition(
                "System.Size",
                minSizeBytes,
                SearchConditionOperation.GreaterThan
            );

            Search(sizeCondition, searchScope);
        }
        catch (Exception ex)
        {
            throw new CommonControlException($"File size search failed: {ex.Message}", HResult.Fail);
        }
    }

    /// <summary>
    /// Performs a search for files by a specific property and navigates to the results.
    /// </summary>
    /// <param name="propertyName">The name of the property to search (e.g., "System.Author", "System.Title").</param>
    /// <param name="propertyValue">The value to search for in the property.</param>
    /// <param name="operation">The search operation to perform.</param>
    /// <param name="searchScope">The scope where to perform the search.</param>
    /// <exception cref="System.ArgumentException">Thrown when propertyName or propertyValue is null or empty.</exception>
    /// <exception cref="System.ArgumentNullException">Thrown when searchScope is null.</exception>
    /// <exception cref="System.Runtime.InteropServices.COMException">Thrown when search fails for any other reason.</exception>
    public void SearchByProperty(string propertyName, string propertyValue, SearchConditionOperation operation, ShellContainer searchScope)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ArgumentException("Property name must not be empty.", nameof(propertyName));
        }

        if (string.IsNullOrWhiteSpace(propertyValue))
        {
            throw new ArgumentException("Property value must not be empty.", nameof(propertyValue));
        }

        if (searchScope == null)
        {
            throw new ArgumentNullException(nameof(searchScope));
        }

        try
        {
            var propertyCondition = SearchConditionFactory.CreateLeafCondition(
                propertyName,
                propertyValue,
                operation
            );

            Search(propertyCondition, searchScope);
        }
        catch (Exception ex)
        {
            throw new CommonControlException($"Property search failed: {ex.Message}", HResult.Fail);
        }
    }

    /// <summary>
    /// Clears the current search and navigates to the specified location.
    /// </summary>
    /// <param name="location">The location to navigate to after clearing the search.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when location is null.</exception>
    public void ClearSearch(ShellObject location)
    {
        if (location == null)
        {
            throw new ArgumentNullException(nameof(location));
        }

        // Clear search state
        SearchState.CurrentSearchQuery = null;
        SearchState.CurrentSearchScope = null;

        Navigate(location);
    }

    #endregion

    #region events

    /// <summary>
    /// Fires when the SelectedItems collection changes. 
    /// </summary>
    public event EventHandler SelectionChanged;

    /// <summary>
    /// Fires when the Items colection changes. 
    /// </summary>
    public event EventHandler ItemsChanged;

    /// <summary>
    /// Fires when a navigation has been initiated, but is not yet complete.
    /// </summary>
    public event EventHandler<NavigationPendingEventArgs> NavigationPending;

    /// <summary>
    /// Fires when a navigation has been 'completed': no NavigationPending listener 
    /// has cancelled, and the ExplorerBorwser has created a new view. The view 
    /// will be populated with new items asynchronously, and ItemsChanged will be 
    /// fired to reflect this some time later.
    /// </summary>
    public event EventHandler<NavigationCompleteEventArgs> NavigationComplete;

    /// <summary>
    /// Fires when either a NavigationPending listener cancels the navigation, or
    /// if the operating system determines that navigation is not possible.
    /// </summary>
    public event EventHandler<NavigationFailedEventArgs> NavigationFailed;

    /// <summary>
    /// Fires when the ExplorerBorwser view has finished enumerating files.
    /// </summary>
    public event EventHandler ViewEnumerationComplete;

    /// <summary>
    /// Fires when the item selected in the view has changed (i.e., a rename ).
    /// This is not the same as SelectionChanged.
    /// </summary>
    public event EventHandler ViewSelectedItemChanged;

    #endregion

    #region implementation

    #region construction
    internal ExplorerBrowserClass? ExplorerBrowserControl;

    // for the IExplorerBrowserEvents Advise call
    internal uint EventsCookie;

    // name of the property bag that contains the view state options of the browser
    string _propertyBagName = typeof(ExplorerBrowser).FullName;

    /// <summary>
    /// Initializes the ExplorerBorwser WinForms wrapper.
    /// </summary>
    public ExplorerBrowser()
        : base()
    {
        NavigationOptions = new ExplorerBrowserNavigationOptions(this);
        ContentOptions = new ExplorerBrowserContentOptions(this);
        NavigationLog = new ExplorerBrowserNavigationLog(this);
    }

    #endregion

    #region message handlers

    /// <summary>
    /// Displays a placeholder for the explorer browser in design mode
    /// </summary>
    /// <param name="e">Contains information about the paint event.</param>
    protected override void OnPaint(PaintEventArgs? e)
    {
        if (DesignMode && e != null)
        {
            using (LinearGradientBrush linGrBrush = new(
                       ClientRectangle,
                       Color.Aqua,
                       Color.CadetBlue,
                       LinearGradientMode.ForwardDiagonal))
            {
                e.Graphics.FillRectangle(linGrBrush, ClientRectangle);
            }

            using (Font font = new("Garamond", 30))
            {
                using (StringFormat sf = new())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    e.Graphics.DrawString(
                        "ExplorerBrowserControl",
                        font,
                        Brushes.White,
                        ClientRectangle,
                        sf);
                }
            }
        }

        base.OnPaint(e);
    }

    ShellObject? _antecreationNavigationTarget;
    // Replace the obsolete ExplorerBrowserViewEvents with a custom implementation
    private ExplorerBrowserViewEvents? _viewEvents;

    // Updated initialization in OnCreateControl
    protected override void OnCreateControl()
    {
        base.OnCreateControl();

        if (!DesignMode)
        {
            ExplorerBrowserControl = new ExplorerBrowserClass();

            // hooks up IExplorerPaneVisibility and ICommDlgBrowser event notifications
            ExplorerBrowserNativeMethods.IUnknown_SetSite(ExplorerBrowserControl, this);

            // hooks up IExplorerBrowserEvents event notification
            ExplorerBrowserControl.Advise(
                Marshal.GetComInterfaceForObject(this, typeof(IExplorerBrowserEvents)),
                out EventsCookie);

            // Replace the obsolete ExplorerBrowserViewEvents initialization
            _viewEvents = new ExplorerBrowserViewEvents(this);

            NativeRect rect = new();
            rect.Top = ClientRectangle.Top;
            rect.Left = ClientRectangle.Left;
            rect.Right = ClientRectangle.Right;
            rect.Bottom = ClientRectangle.Bottom;

            ExplorerBrowserControl.Initialize(Handle, ref rect, null);

            // Force an initial show frames so that IExplorerPaneVisibility works the first time it is set.
            ExplorerBrowserControl.SetOptions(ExplorerBrowserOptions.ShowFrames);

            // Remove the window border manually
            RemoveWindowBorder();

            ExplorerBrowserControl.SetPropertyBag(_propertyBagName);

            if (_antecreationNavigationTarget != null)
            {
                BeginInvoke(new MethodInvoker(() =>
                {
                    Navigate(_antecreationNavigationTarget);
                    _antecreationNavigationTarget = null;
                }));
            }
        }

        Application.AddMessageFilter(this);
    }

    /// <summary>
    /// Sizes the native control to match the WinForms control wrapper.
    /// </summary>
    /// <param name="e">Contains information about the size changed event.</param>
    protected override void OnSizeChanged(EventArgs e)
    {
        if (ExplorerBrowserControl != null)
        {
            NativeRect rect = new();
            rect.Top = ClientRectangle.Top;
            rect.Left = ClientRectangle.Left;
            rect.Right = ClientRectangle.Right;
            rect.Bottom = ClientRectangle.Bottom;

            IntPtr ptr = IntPtr.Zero;
            ExplorerBrowserControl.SetRect(ref ptr, rect);
        }

        base.OnSizeChanged(e);
    }

    /// <summary>
    /// Cleans up the explorer browser events+object when the window is being taken down.
    /// </summary>
    /// <param name="e">An EventArgs that contains event data.</param>
    protected override void OnHandleDestroyed(EventArgs e)
    {
        if (ExplorerBrowserControl != null)
        {
            // unhook events
            _viewEvents.DisconnectFromView();
            ExplorerBrowserControl.Unadvise(EventsCookie);
            ExplorerBrowserNativeMethods.IUnknown_SetSite(ExplorerBrowserControl, null);

            // destroy the explorer browser control
            ExplorerBrowserControl.Destroy();

            // release com reference to it
            Marshal.ReleaseComObject(ExplorerBrowserControl);
            ExplorerBrowserControl = null;
        }

        base.OnHandleDestroyed(e);
    }
    #endregion

    #region object interfaces

    #region IServiceProvider
    /// <summary>
    /// 
    /// </summary>
    /// <param name="guidService">calling service</param>
    /// <param name="riid">requested interface guid</param>
    /// <param name="ppvObject">caller-allocated memory for interface pointer</param>
    /// <returns></returns>
    HResult IServiceProvider.QueryService(
        ref Guid guidService, ref Guid riid, out IntPtr ppvObject)
    {
        HResult hr = HResult.Ok;

        if (guidService.CompareTo(new Guid(ExplorerBrowserIIDGuid.IExplorerPaneVisibility)) == 0)
        {
            // Responding to this SID allows us to control the visibility of the 
            // explorer browser panes
            ppvObject =
                Marshal.GetComInterfaceForObject(this, typeof(IExplorerPaneVisibility));
            hr = HResult.Ok;
        }
        else if (guidService.CompareTo(new Guid(ExplorerBrowserIIDGuid.ICommDlgBrowser)) == 0)
        {
            if (riid.CompareTo(new Guid(ExplorerBrowserIIDGuid.ICommDlgBrowser)) == 0)
            {
                ppvObject = Marshal.GetComInterfaceForObject(this, typeof(ICommDlgBrowser3));
                hr = HResult.Ok;
            }
            // The below lines are commented out to decline requests for the ICommDlgBrowser2 interface.
            // This interface is incorrectly marshaled back to unmanaged, and causes an exception.
            // There is a bug for this, I have not figured the underlying cause.
            // Remove this comment and uncomment the following code to enable the ICommDlgBrowser2 interface
            //else if (riid.CompareTo(new Guid(ExplorerBrowserIIDGuid.ICommDlgBrowser2)) == 0)
            //{
            //    ppvObject = Marshal.GetComInterfaceForObject(this, typeof(ICommDlgBrowser3));
            //    hr = HResult.Ok;                    
            //}
            else if (riid.CompareTo(new Guid(ExplorerBrowserIIDGuid.ICommDlgBrowser3)) == 0)
            {
                ppvObject = Marshal.GetComInterfaceForObject(this, typeof(ICommDlgBrowser3));
                hr = HResult.Ok;
            }
            else
            {
                ppvObject = IntPtr.Zero;
                hr = HResult.NoInterface;
            }
        }
        else
        {
            IntPtr nullObj = IntPtr.Zero;
            ppvObject = nullObj;
            hr = HResult.NoInterface;
        }

        return hr;
    }
    #endregion

    #region IExplorerPaneVisibility
    /// <summary>
    /// Controls the visibility of the explorer borwser panes
    /// </summary>
    /// <param name="explorerPane">a guid identifying the pane</param>
    /// <param name="peps">the pane state desired</param>
    /// <returns></returns>
    HResult IExplorerPaneVisibility.GetPaneState(ref Guid explorerPane, out ExplorerPaneState peps)
    {
        switch (explorerPane.ToString())
        {
            case ExplorerBrowserViewPanes.AdvancedQuery:
                peps = VisibilityToPaneState(NavigationOptions.PaneVisibility.AdvancedQuery);
                break;
            case ExplorerBrowserViewPanes.Commands:
                peps = VisibilityToPaneState(NavigationOptions.PaneVisibility.Commands);
                break;
            case ExplorerBrowserViewPanes.CommandsOrganize:
                peps = VisibilityToPaneState(NavigationOptions.PaneVisibility.CommandsOrganize);
                break;
            case ExplorerBrowserViewPanes.CommandsView:
                peps = VisibilityToPaneState(NavigationOptions.PaneVisibility.CommandsView);
                break;
            case ExplorerBrowserViewPanes.Details:
                peps = VisibilityToPaneState(NavigationOptions.PaneVisibility.Details);
                break;
            case ExplorerBrowserViewPanes.Navigation:
                peps = VisibilityToPaneState(NavigationOptions.PaneVisibility.Navigation);
                break;
            case ExplorerBrowserViewPanes.Preview:
                peps = VisibilityToPaneState(NavigationOptions.PaneVisibility.Preview);
                break;
            case ExplorerBrowserViewPanes.Query:
                peps = VisibilityToPaneState(NavigationOptions.PaneVisibility.Query);
                break;
            default:
#if LOG_UNKNOWN_PANES
                    System.Diagnostics.Debugger.Log( 4, "ExplorerBrowser", "unknown pane view state. id=" + explorerPane.ToString( ) );
#endif
                peps = VisibilityToPaneState(PaneVisibilityState.Show);
                break;
        }

        return HResult.Ok;
    }

    private static ExplorerPaneState VisibilityToPaneState(PaneVisibilityState visibility)
    {
        switch (visibility)
        {
            case PaneVisibilityState.DoNotCare:
                return ExplorerPaneState.DoNotCare;

            case PaneVisibilityState.Hide:
                return ExplorerPaneState.DefaultOff | ExplorerPaneState.Force;

            case PaneVisibilityState.Show:
                return ExplorerPaneState.DefaultOn | ExplorerPaneState.Force;

            default:
                throw new ArgumentException("unexpected PaneVisibilityState");
        }
    }

    #endregion

    #region IExplorerBrowserEvents
    HResult IExplorerBrowserEvents.OnNavigationPending(IntPtr pidlFolder)
    {
        bool canceled = false;

        if (NavigationPending != null)
        {
            NavigationPendingEventArgs args = new();

            // For some special items (like network machines), ShellObject.FromIDList
            // might return null
            args.PendingLocation = ShellObjectFactory.Create(pidlFolder);

            if (args.PendingLocation != null)
            {
                foreach (Delegate del in NavigationPending.GetInvocationList())
                {
                    del.DynamicInvoke(new object[] { this, args });
                    if (args.Cancel)
                    {
                        canceled = true;
                    }
                }
            }
        }

        return canceled ? HResult.Canceled : HResult.Ok;
    }

    HResult IExplorerBrowserEvents.OnViewCreated(object psv)
    {
        _viewEvents.ConnectToView((IShellView)psv);

        return HResult.Ok;
    }

    HResult IExplorerBrowserEvents.OnNavigationComplete(IntPtr pidlFolder)
    {
        // view mode may change 
        ContentOptions.FolderSettings.ViewMode = GetCurrentViewMode();

        if (NavigationComplete != null)
        {
            NavigationCompleteEventArgs args = new();
            args.NewLocation = ShellObjectFactory.Create(pidlFolder);
            NavigationComplete(this, args);
        }
        return HResult.Ok;
    }

    HResult IExplorerBrowserEvents.OnNavigationFailed(IntPtr pidlFolder)
    {
        if (NavigationFailed != null)
        {
            NavigationFailedEventArgs args = new();
            args.FailedLocation = ShellObjectFactory.Create(pidlFolder);
            NavigationFailed(this, args);
        }
        return HResult.Ok;
    }
    #endregion

    #region ICommDlgBrowser
    HResult ICommDlgBrowser3.OnDefaultCommand(IntPtr ppshv)
    {
        return HResult.False;
        //return HResult.Ok;
    }

    HResult ICommDlgBrowser3.OnStateChange(IntPtr ppshv, CommDlgBrowserStateChange uChange)
    {
        if (uChange == CommDlgBrowserStateChange.SelectionChange)
        {
            FireSelectionChanged();
        }

        return HResult.Ok;
    }

    HResult ICommDlgBrowser3.IncludeObject(IntPtr ppshv, IntPtr pidl)
    {
        // items in the view have changed, so the collections need updating
        FireContentChanged();

        return HResult.Ok;
    }

    #endregion

    #region ICommDlgBrowser2 Members

    // The below methods can be called into, but marshalling the response causes an exception to be
    // thrown from unmanaged code.  At this time, I decline calls requesting the ICommDlgBrowser2
    // interface.  This is logged as a bug, but moved to less of a priority, as it only affects being
    // able to change the default action text for remapping the default action.

    HResult ICommDlgBrowser3.GetDefaultMenuText(IShellView shellView, IntPtr text, int cchMax)
    {
        return HResult.False;
        //return HResult.Ok;
        //OK if new
        //False if default
        //other if error
    }

    HResult ICommDlgBrowser3.GetViewFlags(out uint pdwFlags)
    {
        //var flags = CommDlgBrowser2ViewFlags.NoSelectVerb;
        //Marshal.WriteInt32(pdwFlags, 0);
        pdwFlags = (uint)CommDlgBrowser2ViewFlags.ShowAllFiles;
        return HResult.Ok;
    }

    HResult ICommDlgBrowser3.Notify(IntPtr pshv, CommDlgBrowserNotifyType notifyType)
    {
        return HResult.Ok;
    }

    #endregion

    #region ICommDlgBrowser3 Members

    HResult ICommDlgBrowser3.GetCurrentFilter(StringBuilder pszFileSpec, int cchFileSpec)
    {
        // If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        return HResult.Ok;
    }

    HResult ICommDlgBrowser3.OnColumnClicked(IShellView ppshv, int iColumn)
    {
        // If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        return HResult.Ok;
    }

    HResult ICommDlgBrowser3.OnPreViewCreated(IShellView ppshv)
    {
        // If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code
        return HResult.Ok;
    }

    #endregion

    #region IMessageFilter Members

    bool IMessageFilter.PreFilterMessage(ref System.Windows.Forms.Message m)
    {
        HResult hr = HResult.False;
        if (ExplorerBrowserControl != null)
        {
            // translate keyboard input
            hr = ((IInputObject)ExplorerBrowserControl).TranslateAcceleratorIO(ref m);
        }
        return (hr == HResult.Ok);
    }

    #endregion

    #endregion

    #region utilities

    /// <summary>
    /// Returns the current view mode of the browser
    /// </summary>
    /// <returns></returns>
    internal FolderViewMode GetCurrentViewMode()
    {
        IFolderView2? ifv2 = GetFolderView2();
        uint viewMode = 0;
        if (ifv2 != null)
        {
            try
            {
                HResult hr = ifv2.GetCurrentViewMode(out viewMode);
                if (hr != HResult.Ok) { throw new ShellException(hr); }
            }
            finally
            {
                Marshal.ReleaseComObject(ifv2);
                ifv2 = null;
            }
        }
        return (FolderViewMode)viewMode;
    }

    /// <summary>
    /// Gets the IFolderView2 interface from the explorer browser.
    /// </summary>
    /// <returns></returns>
    internal IFolderView2? GetFolderView2()
    {
        Guid iid = new(ExplorerBrowserIIDGuid.IFolderView2);
        IntPtr view = IntPtr.Zero;
        if (ExplorerBrowserControl != null)
        {
            HResult hr = ExplorerBrowserControl.GetCurrentView(ref iid, out view);
            switch (hr)
            {
                case HResult.Ok:
                    break;

                case HResult.NoInterface:
                case HResult.Fail:
#if LOG_KNOWN_COM_ERRORS
                        Debugger.Log( 2, "ExplorerBrowser", "Unable to obtain view. Error=" + e.ToString( ) );
#endif
                    return null;

                default:
                    throw new CommonControlException(LocalizedMessages.ExplorerBrowserFailedToGetView, hr);
            }

            return (IFolderView2)Marshal.GetObjectForIUnknown(view);
        }
        return null;
    }

    /// <summary>
    /// Gets the selected items in the explorer browser as an IShellItemArray
    /// </summary>
    /// <returns></returns>
    internal IShellItemArray? GetSelectedItemsArray()
    {
        IShellItemArray? iArray = null;
        IFolderView2? iFv2 = GetFolderView2();
        if (iFv2 != null)
        {
            try
            {
                Guid iidShellItemArray = new(ShellIIDGuid.IShellItemArray);
                object? oArray = null;
                HResult hr = iFv2.Items((uint)ShellViewGetItemObject.Selection, ref iidShellItemArray, out oArray);
                iArray = oArray as IShellItemArray;
                if (hr != HResult.Ok &&
                    hr != HResult.ElementNotFound &&
                    hr != HResult.Fail)
                {
                    throw new CommonControlException(LocalizedMessages.ExplorerBrowserUnexpectedError, hr);
                }
            }
            finally
            {
                Marshal.ReleaseComObject(iFv2);
                iFv2 = null;
            }
        }

        return iArray;
    }

    /// <summary>
    /// Find the native control handle, remove its border style, then ask for a redraw.
    /// </summary>
    internal void RemoveWindowBorder()
    {
        // There is an option (EBO_NOBORDER) to avoid showing a border on the native ExplorerBrowser control
        // so we wouldn't have to remove it afterwards, but:
        // 1. It's not implemented by the Windows API Code Pack
        // 2. The flag doesn't seem to work anyway (tested on 7 and 8.1)
        // For reference: EXPLORER_BROWSER_OPTIONS https://msdn.microsoft.com/en-us/library/windows/desktop/bb762501(v=vs.85).aspx

        IntPtr hwnd = WindowNativeMethods.FindWindowEx(Handle, IntPtr.Zero, "ExplorerBrowserControl", IntPtr.Zero);
        int explorerBrowserStyle = WindowNativeMethods.GetWindowLong(hwnd, (int)WindowLongFlags.GWL_STYLE);
        WindowNativeMethods.SetWindowLong(
            hwnd,
            (int)WindowLongFlags.GWL_STYLE,
            explorerBrowserStyle & ~(int)WindowStyles.Caption & ~(int)WindowStyles.Border);
        WindowNativeMethods.SetWindowPos(
            hwnd,
            IntPtr.Zero,
            0, 0, 0, 0,
            SetWindowPosFlags.SWP_FRAMECHANGED | SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_NOSIZE);

    }

    internal int GetItemsCount()
    {
        int itemsCount = 0;

        IFolderView2? iFv2 = GetFolderView2();
        if (iFv2 != null)
        {
            try
            {
                iFv2.ItemCount((uint)ShellViewGetItemObject.AllView, out itemsCount);
            }
            finally
            {
                Marshal.ReleaseComObject(iFv2);
            }
        }

        return itemsCount;
    }

    internal int GetSelectedItemsCount()
    {
        int itemsCount = 0;

        IFolderView2? iFv2 = GetFolderView2();
        if (iFv2 != null)
        {
            try
            {
                HResult hr = iFv2.ItemCount((uint)ShellViewGetItemObject.Selection, out itemsCount);

                if (hr != HResult.Ok &&
                    hr != HResult.ElementNotFound &&
                    hr != HResult.Fail)
                {
                    throw new CommonControlException(LocalizedMessages.ExplorerBrowserSelectedItemCount, hr);
                }
            }
            finally
            {
                Marshal.ReleaseComObject(iFv2);
                iFv2 = null;
            }
        }

        return itemsCount;
    }

    /// <summary>
    /// Gets the items in the ExplorerBrowser as an IShellItemArray
    /// </summary>
    /// <returns></returns>
    internal IShellItemArray? GetItemsArray()
    {
        IShellItemArray? iArray = null;
        IFolderView2? iFv2 = GetFolderView2();
        if (iFv2 != null)
        {
            try
            {
                Guid iidShellItemArray = new(ShellIIDGuid.IShellItemArray);
                object? oArray = null;
                HResult hr = iFv2.Items((uint)ShellViewGetItemObject.AllView, ref iidShellItemArray, out oArray);
                if (hr != HResult.Ok &&
                    hr != HResult.Fail &&
                    hr != HResult.ElementNotFound &&
                    hr != HResult.InvalidArguments)
                {
                    throw new CommonControlException(LocalizedMessages.ExplorerBrowserViewItems, hr);
                }

                iArray = oArray as IShellItemArray;
            }
            finally
            {
                Marshal.ReleaseComObject(iFv2);
                iFv2 = null;
            }
        }
        return iArray;
    }

    #endregion

    #region view event forwarding

    /// <summary>
    /// Checks if all items in the view have been fully enumerated.
    /// </summary>
    private bool IsEnumerationComplete()
    {
        try
        {
            if (_totalItemsCache == null)
            {
                _totalItemsCache = GetTotalItemCount();
                System.Diagnostics.Debug.WriteLine($"Total items determined: {_totalItemsCache}");
            }

            var enumeratedItems = GetItemsCount();
            System.Diagnostics.Debug.WriteLine($"Enumerated items: {enumeratedItems}/{_totalItemsCache}");

            return _totalItemsCache != null && enumeratedItems == _totalItemsCache;
        }
        catch
        {
            // Log or handle the exception as needed
            return false; // Fail-safe assumption
        }
    }

    /// <summary>
    /// Fires the ViewEnumerationComplete event only if all items are fully enumerated.
    /// </summary>
    internal void FireContentEnumerationCompleteIfDone()
    {
        if (IsEnumerationComplete() && !_enumerationCompleteFired)
        {
            _enumerationCompleteFired = true;
            FireContentEnumerationComplete();
        }
    }

    /// <summary>
    /// Retrieves the total number of items in the view.
    /// </summary>
    private int? GetTotalItemCount()
    {
        var ifV2 = GetFolderView2();
        if (ifV2 == null)
        {
            return null;
        }

        try
        {
            ifV2.ItemCount((uint)ShellViewGetItemObject.AllView, out var totalItems);
            return totalItems;
        }
        finally
        {
            Marshal.ReleaseComObject(ifV2);
        }
    }

    /// <summary>
    /// Resets the enumeration state, useful for starting new navigation or refreshing content.
    /// </summary>
    internal void ResetEnumerationState()
    {
        _totalItemsCache = null;
        _enumerationCompleteFired = false;
    }

    internal void FireContentEnumerationComplete()
    {
        if (ViewEnumerationComplete != null)
        {
            ViewEnumerationComplete.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Updates the existing FireContentChanged method to ensure enumeration completeness.
    /// </summary>
    internal void FireContentChanged()
    {
        ItemsChanged?.Invoke(this, EventArgs.Empty);

        // Update search state when items change
        UpdateSearchState();

        // Add enumeration check after content change
        FireContentEnumerationCompleteIfDone();
    }


    internal void FireSelectionChanged()
    {
        if (SelectionChanged != null)
        {
            SelectionChanged(this, EventArgs.Empty);
        }
    }

    internal void FireSelectedItemChanged()
    {
        if (ViewSelectedItemChanged != null)
        {
            ViewSelectedItemChanged.Invoke(this, EventArgs.Empty);
        }
    }
    #endregion

    #endregion

}