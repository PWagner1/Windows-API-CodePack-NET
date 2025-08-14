//Copyright (c) Microsoft Corporation.  All rights reserved.

using UserControl = System.Windows.Controls.UserControl;

// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
// ReSharper disable UseNameofForDependencyProperty

namespace Microsoft.WindowsAPICodePack.Controls.WindowsPresentationFoundation;

/// <summary>
/// Interaction logic for ExplorerBrowser.xaml
/// </summary>
public partial class ExplorerBrowser : UserControl, IDisposable
{
    /// <summary>
    /// The underlying WinForms control
    /// </summary>
    public WindowsForms.ExplorerBrowser ExplorerBrowserControl
    {
        get;
        set;
    }

    private readonly ObservableCollection<ShellObject> _selectedItems;
    private readonly ObservableCollection<ShellObject> _items;
    private readonly ObservableCollection<ShellObject> _navigationLog;
    private readonly DispatcherTimer _dtClrUpdater = new();

    private ShellObject? _initialNavigationTarget;
    private ExplorerBrowserViewMode? _initialViewMode;

    private readonly AutoResetEvent _itemsChanged = new AutoResetEvent(false);
    private readonly AutoResetEvent _selectionChanged = new AutoResetEvent(false);
    private int _selectionChangeWaitCount;

    /// <summary>
    /// Hosts the ExplorerBrowser WinForms wrapper in this control
    /// </summary>
    public ExplorerBrowser()
    {
        InitializeComponent();

        // the ExplorerBrowser WinForms control
        ExplorerBrowserControl = new WindowsForms.ExplorerBrowser();

        // back the dependency collection properties with instances
        SelectedItems = _selectedItems = new ObservableCollection<ShellObject>();
        Items = _items = new ObservableCollection<ShellObject>();
        NavigationLog = _navigationLog = new ObservableCollection<ShellObject>();

        // hook up events for collection synchronization
        ExplorerBrowserControl.ItemsChanged += ItemsChanged;
        ExplorerBrowserControl.SelectionChanged += SelectionChanged;
        ExplorerBrowserControl.ViewEnumerationComplete += ExplorerBrowserControl_ViewEnumerationComplete;
        ExplorerBrowserControl.ViewSelectedItemChanged += ExplorerBrowserControl_ViewSelectedItemChanged;
        ExplorerBrowserControl.NavigationLog.NavigationLogChanged += NavigationLogChanged;

        // host the control           
        WindowsFormsHost host = new();
        try
        {
            host.Child = ExplorerBrowserControl;
            root.Children.Clear();
            root.Children.Add(host);
        }
        catch
        {
            host.Dispose();
            throw;
        }

        Loaded += ExplorerBrowser_Loaded;
    }

    void ExplorerBrowserControl_ViewSelectedItemChanged(object? sender, EventArgs e)
    {
    }

    void ExplorerBrowserControl_ViewEnumerationComplete(object? sender, EventArgs e)
    {
        _itemsChanged.Set();
        _selectionChanged.Set();
    }

    /// <summary>
    /// To avoid the 'Dispatcher processing has been suspended' InvalidOperationException on Win7,
    /// the ExplorerBorwser native control is initialized after this control is fully loaded.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void ExplorerBrowser_Loaded(object? sender, RoutedEventArgs e)
    {
        // setup timer to update dependency properties from CLR properties of WinForms ExplorerBrowser object
        _dtClrUpdater.Tick += UpdateDependencyPropertiesFromClrpRoperties;
        _dtClrUpdater.Interval = new TimeSpan(100 * 10000); // 100ms
        _dtClrUpdater.Start();

        if (_initialNavigationTarget != null)
        {
            ExplorerBrowserControl.Navigate(_initialNavigationTarget);
            _initialNavigationTarget = null;
        }

        if (_initialViewMode != null)
        {
            ExplorerBrowserControl.ContentOptions.ViewMode = (ExplorerBrowserViewMode)_initialViewMode;
            _initialViewMode = null;
        }
    }

    /// <summary>
    /// Map changes to the CLR flags to the dependency properties
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void UpdateDependencyPropertiesFromClrpRoperties(object? sender, EventArgs e)
    {
        AlignLeft = ExplorerBrowserControl.ContentOptions.AlignLeft;
        AutoArrange = ExplorerBrowserControl.ContentOptions.AutoArrange;
        CheckSelect = ExplorerBrowserControl.ContentOptions.CheckSelect;
        ExtendedTiles = ExplorerBrowserControl.ContentOptions.ExtendedTiles;
        FullRowSelect = ExplorerBrowserControl.ContentOptions.FullRowSelect;
        HideFileNames = ExplorerBrowserControl.ContentOptions.HideFileNames;
        NoBrowserViewState = ExplorerBrowserControl.ContentOptions.NoBrowserViewState;
        NoColumnHeader = ExplorerBrowserControl.ContentOptions.NoColumnHeader;
        NoHeaderInAllViews = ExplorerBrowserControl.ContentOptions.NoHeaderInAllViews;
        NoIcons = ExplorerBrowserControl.ContentOptions.NoIcons;
        NoSubfolders = ExplorerBrowserControl.ContentOptions.NoSubfolders;
        SingleClickActivate = ExplorerBrowserControl.ContentOptions.SingleClickActivate;
        SingleSelection = ExplorerBrowserControl.ContentOptions.SingleSelection;
        ThumbnailSize = ExplorerBrowserControl.ContentOptions.ThumbnailSize;
        ViewMode = ExplorerBrowserControl.ContentOptions.ViewMode;
        AlwaysNavigate = ExplorerBrowserControl.NavigationOptions.AlwaysNavigate;
        NavigateOnce = ExplorerBrowserControl.NavigationOptions.NavigateOnce;
        AdvancedQueryPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.AdvancedQuery;
        CommandsPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.Commands;
        CommandsOrganizePane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.CommandsOrganize;
        CommandsViewPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.CommandsView;
        DetailsPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.Details;
        NavigationPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.Navigation;
        PreviewPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.Preview;
        QueryPane = ExplorerBrowserControl.NavigationOptions.PaneVisibility.Query;
        NavigationLogIndex = ExplorerBrowserControl.NavigationLog.CurrentLocationIndex;

        if (_itemsChanged.WaitOne(1, false))
        {
            _items.Clear();
            if (ExplorerBrowserControl.Items != null)
            {
                foreach (ShellObject obj in ExplorerBrowserControl.Items)
                {
                    _items.Add(obj);
                }
            }
        }

        if (_selectionChanged.WaitOne(1, false))
        {
            _selectionChangeWaitCount = 4;
        }
        else if (_selectionChangeWaitCount > 0)
        {
            _selectionChangeWaitCount--;

            if (_selectionChangeWaitCount == 0)
            {
                _selectedItems.Clear();
                if (ExplorerBrowserControl.SelectedItems != null)
                {
                    foreach (ShellObject obj in ExplorerBrowserControl.SelectedItems)
                    {
                        _selectedItems.Add(obj);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Synchronize NavigationLog collection to dependency collection
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    void NavigationLogChanged(object? sender, NavigationLogEventArgs args)
    {
        _navigationLog.Clear();
        foreach (ShellObject? obj in ExplorerBrowserControl.NavigationLog.Locations)
        {
            if (obj != null)
            {
                _navigationLog.Add(obj);
            }
        }
    }

    /// <summary>
    /// Synchronize SelectedItems collection to dependency collection
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void SelectionChanged(object? sender, EventArgs e)
    {
        _selectionChanged.Set();
    }

    // Synchronize ItemsCollection to dependency collection
    void ItemsChanged(object? sender, EventArgs e)
    {
        _itemsChanged.Set();
    }

    /// <summary>
    /// The items in the ExplorerBrowser window
    /// </summary>
    public ObservableCollection<ShellObject> Items
    {
        get => (ObservableCollection<ShellObject>)GetValue(ItemsProperty);
        set => SetValue(ItemsPropertyKey, value);
    }

    private static readonly DependencyPropertyKey ItemsPropertyKey =
        DependencyProperty.RegisterReadOnly(
            "Items", typeof(ObservableCollection<ShellObject>),
            typeof(ExplorerBrowser),
            new PropertyMetadata(null));

    /// <summary>
    /// The items in the ExplorerBrowser window
    /// </summary>
    public static readonly DependencyProperty ItemsProperty = ItemsPropertyKey.DependencyProperty;

    /// <summary>
    /// The selected items in the ExplorerBrowser window
    /// </summary>
    public ObservableCollection<ShellObject> SelectedItems
    {
        get => (ObservableCollection<ShellObject>)GetValue(SelectedItemsProperty);
        internal set => SetValue(SelectedItemsPropertyKey, value);
    }

    private static readonly DependencyPropertyKey SelectedItemsPropertyKey =
        DependencyProperty.RegisterReadOnly(
            "SelectedItems", typeof(ObservableCollection<ShellObject>),
            typeof(ExplorerBrowser),
            new PropertyMetadata(null));

    /// <summary>
    /// The selected items in the ExplorerBrowser window
    /// </summary>
    public ObservableCollection<ShellObject>? NavigationLog
    {
        get => GetValue(NavigationLogProperty) as ObservableCollection<ShellObject>;
        internal set => SetValue(NavigationLogPropertyKey, value);
    }

    private static readonly DependencyPropertyKey NavigationLogPropertyKey =
        DependencyProperty.RegisterReadOnly(
            "NavigationLog", typeof(ObservableCollection<ShellObject>),
            typeof(ExplorerBrowser),
            new PropertyMetadata(null));

    /// <summary>
    /// The NavigationLog
    /// </summary>
    public static readonly DependencyProperty NavigationLogProperty = NavigationLogPropertyKey.DependencyProperty;

    /// <summary>
    /// The selected items in the ExplorerBrowser window
    /// </summary>
    public static readonly DependencyProperty SelectedItemsProperty = SelectedItemsPropertyKey.DependencyProperty;

    #region Search Properties

    // Search State Properties
    /// <summary>
    /// Gets the search state properties for the ExplorerBrowser.
    /// </summary>
    [Description("Search state properties for the ExplorerBrowser.")]
    [Category("Search")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public ExplorerBrowserSearchState SearchState => ExplorerBrowserControl?.SearchState ?? new ExplorerBrowserSearchState();

    // Search Options Properties
    /// <summary>
    /// Gets the search options properties for the ExplorerBrowser.
    /// </summary>
    [Description("Search options properties for the ExplorerBrowser.")]
    [Category("Search")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public ExplorerBrowserSearchOptions SearchOptions => ExplorerBrowserControl?.SearchOptions ?? new ExplorerBrowserSearchOptions();

    /// <summary>
    /// Gets whether the current view is showing search results.
    /// </summary>
    [Obsolete("Use SearchState.IsShowingSearchResults instead.")]
    public bool IsShowingexplorerSearchResults => ExplorerBrowserControl?.IsShowingSearchResults ?? false;

    /// <summary>
    /// Gets the current search query. Returns null if no search is active.
    /// </summary>
    [Obsolete("Use SearchState.CurrentSearchQuery instead.")]
    public string? CurrentSearchQuery => ExplorerBrowserControl?.CurrentSearchQuery;

    /// <summary>
    /// Gets the current search scope. Returns null if no search is active.
    /// </summary>
    [Obsolete("Use SearchState.CurrentSearchScope instead.")]
    public ShellContainer? CurrentSearchScope => ExplorerBrowserControl?.CurrentSearchScope;

    /// <summary>
    /// Gets or sets whether search results should be automatically refreshed when the search scope changes.
    /// </summary>
    [Obsolete("Use SearchOptions.AutoRefreshSearchResults instead.")]
    public bool AutoRefreshSearchResults
    {
        get => ExplorerBrowserControl?.AutoRefreshSearchResults ?? true;
        set { if (ExplorerBrowserControl != null)
            {
                ExplorerBrowserControl.AutoRefreshSearchResults = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the maximum number of search results to display. Set to 0 for unlimited results.
    /// </summary>
    [Obsolete("Use SearchOptions.MaxSearchResults instead.")]
    public int MaxSearchResults
    {
        get => ExplorerBrowserControl?.MaxSearchResults ?? 0;
        set { if (ExplorerBrowserControl != null)
            {
                ExplorerBrowserControl.MaxSearchResults = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to show search progress indicators during long searches.
    /// </summary>
    [Obsolete("Use SearchOptions.ShowSearchProgress instead.")]
    public bool ShowSearchProgress
    {
        get => ExplorerBrowserControl?.ShowSearchProgress ?? true;
        set { if (ExplorerBrowserControl != null)
            {
                ExplorerBrowserControl.ShowSearchProgress = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to cache search results for better performance.
    /// </summary>
    [Obsolete("Use SearchOptions.CacheSearchResults instead.")]
    public bool CacheSearchResults
    {
        get => ExplorerBrowserControl?.CacheSearchResults ?? true;
        set { if (ExplorerBrowserControl != null)
            {
                ExplorerBrowserControl.CacheSearchResults = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the search result sorting order.
    /// </summary>
    [Obsolete("Use SearchOptions.SearchResultSortOrder instead.")]
    public SearchResultSortOrder SearchResultSortOrder
    {
        get => ExplorerBrowserControl?.SearchResultSortOrder ?? SearchResultSortOrder.Relevance;
        set { if (ExplorerBrowserControl != null)
            {
                ExplorerBrowserControl.SearchResultSortOrder = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to group search results by file type.
    /// </summary>
    [Obsolete("Use SearchOptions.GroupSearchResultsByType instead.")]
    public bool GroupSearchResultsByType
    {
        get => ExplorerBrowserControl?.GroupSearchResultsByType == SearchResultGrouping.ByFileType;
        set
        {
            if (ExplorerBrowserControl != null)
            {
                ExplorerBrowserControl.GroupSearchResultsByType = value
                    ? SearchResultGrouping.ByFileType
                    : SearchResultGrouping.None;
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to show file previews in search results.
    /// </summary>
    [Obsolete("Use SearchOptions.ShowFilePreviews instead.")]
    public bool ShowFilePreviews
    {
        get => ExplorerBrowserControl?.ShowFilePreviews ?? true;
        set { if (ExplorerBrowserControl != null)
            {
                ExplorerBrowserControl.ShowFilePreviews = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the thumbnail size for search results. Only applies when ShowFilePreviews is true.
    /// </summary>
    [Obsolete("Use SearchOptions.SearchResultThumbnailSize instead.")]
    public int SearchResultThumbnailSize
    {
        get => ExplorerBrowserControl?.SearchResultThumbnailSize ?? 96;
        set { if (ExplorerBrowserControl != null)
            {
                ExplorerBrowserControl.SearchResultThumbnailSize = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to enable incremental search (search as you type).
    /// </summary>
    [Obsolete("Use SearchOptions.EnableIncrementalSearch instead.")]
    public bool EnableIncrementalSearch
    {
        get => ExplorerBrowserControl?.EnableIncrementalSearch ?? false;
        set { if (ExplorerBrowserControl != null)
            {
                ExplorerBrowserControl.EnableIncrementalSearch = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the delay in milliseconds before performing incremental search.
    /// </summary>
    [Obsolete("Use SearchOptions.IncrementalSearchDelay instead.")]
    public int IncrementalSearchDelay
    {
        get => ExplorerBrowserControl?.IncrementalSearchDelay ?? 500;
        set { if (ExplorerBrowserControl != null)
            {
                ExplorerBrowserControl.IncrementalSearchDelay = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to show search suggestions based on user input.
    /// </summary>
    [Obsolete("Use SearchOptions.ShowSearchSuggestions instead.")]
    public bool ShowSearchSuggestions
    {
        get => ExplorerBrowserControl?.ShowSearchSuggestions ?? true;
        set { if (ExplorerBrowserControl != null)
            {
                ExplorerBrowserControl.ShowSearchSuggestions = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the maximum number of search suggestions to display.
    /// </summary>
    [Obsolete("Use SearchOptions.MaxSearchSuggestions instead.")]
    public int MaxSearchSuggestions
    {
        get => ExplorerBrowserControl?.MaxSearchSuggestions ?? 10;
        set { if (ExplorerBrowserControl != null)
            {
                ExplorerBrowserControl.MaxSearchSuggestions = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to highlight search terms in search results.
    /// </summary>
    [Obsolete("Use SearchOptions.HighlightSearchTerms instead.")]
    public bool HighlightSearchTerms
    {
        get => ExplorerBrowserControl?.HighlightSearchTerms ?? true;
        set { if (ExplorerBrowserControl != null)
            {
                ExplorerBrowserControl.HighlightSearchTerms = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to enable advanced search filters (date, size, type, etc.).
    /// </summary>
    [Obsolete("Use SearchOptions.EnableAdvancedSearchFilters instead.")]
    public bool EnableAdvancedSearchFilters
    {
        get => ExplorerBrowserControl?.EnableAdvancedSearchFilters ?? true;
        set { if (ExplorerBrowserControl != null)
            {
                ExplorerBrowserControl.EnableAdvancedSearchFilters = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to remember search history for quick access.
    /// </summary>
    [Obsolete("Use SearchOptions.RememberSearchHistory instead.")]
    public bool RememberSearchHistory
    {
        get => ExplorerBrowserControl?.RememberSearchHistory ?? true;
        set { if (ExplorerBrowserControl != null)
            {
                ExplorerBrowserControl.RememberSearchHistory = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the maximum number of search history entries to remember.
    /// </summary>
    [Obsolete("Use SearchOptions.MaxSearchHistoryEntries instead.")]
    public int MaxSearchHistoryEntries
    {
        get => ExplorerBrowserControl?.MaxSearchHistoryEntries ?? 50;
        set { if (ExplorerBrowserControl != null)
            {
                ExplorerBrowserControl.MaxSearchHistoryEntries = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to enable search result export functionality.
    /// </summary>
    [Obsolete("Use SearchOptions.EnableSearchResultExport instead.")]
    public bool EnableSearchResultExport
    {
        get => ExplorerBrowserControl?.EnableSearchResultExport ?? false;
        set { if (ExplorerBrowserControl != null)
            {
                ExplorerBrowserControl.EnableSearchResultExport = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to show search statistics (total results, search time, etc.).
    /// </summary>
    [Obsolete("Use SearchOptions.ShowSearchStatistics instead.")]
    public bool ShowSearchStatistics
    {
        get => ExplorerBrowserControl?.ShowSearchStatistics ?? true;
        set { if (ExplorerBrowserControl != null)
            {
                ExplorerBrowserControl.ShowSearchStatistics = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to enable search result sharing functionality.
    /// </summary>
    [Obsolete("Use SearchOptions.EnableSearchResultSharing instead.")]
    public bool EnableSearchResultSharing
    {
        get => ExplorerBrowserControl?.EnableSearchResultSharing ?? false;
        set { if (ExplorerBrowserControl != null)
            {
                ExplorerBrowserControl.EnableSearchResultSharing = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to show search result context menu with additional options.
    /// </summary>
    [Obsolete("Use SearchOptions.ShowSearchResultContextMenu instead.")]
    public bool ShowSearchResultContextMenu
    {
        get => ExplorerBrowserControl?.ShowSearchResultContextMenu ?? true;
        set { if (ExplorerBrowserControl != null)
            {
                ExplorerBrowserControl.ShowSearchResultContextMenu = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to enable drag and drop of search results.
    /// </summary>
    [Obsolete("Use SearchOptions.EnableSearchResultDragDrop instead.")]
    public bool EnableSearchResultDragDrop
    {
        get => ExplorerBrowserControl?.EnableSearchResultDragDrop ?? true;
        set { if (ExplorerBrowserControl != null)
            {
                ExplorerBrowserControl.EnableSearchResultDragDrop = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to show search result tooltips with additional information.
    /// </summary>
    [Obsolete("Use SearchOptions.ShowSearchResultTooltips instead.")]
    public bool ShowSearchResultTooltips
    {
        get => ExplorerBrowserControl?.ShowSearchResultTooltips ?? true;
        set { if (ExplorerBrowserControl != null)
            {
                ExplorerBrowserControl.ShowSearchResultTooltips = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to enable search result selection persistence across navigation.
    /// </summary>
    [Obsolete("Use SearchOptions.PersistSearchResultSelection instead.")]
    public bool PersistSearchResultSelection
    {
        get => ExplorerBrowserControl?.PersistSearchResultSelection ?? false;
        set { if (ExplorerBrowserControl != null)
            {
                ExplorerBrowserControl.PersistSearchResultSelection = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to show search result count in the status bar.
    /// </summary>
    [Obsolete("Use SearchOptions.ShowSearchResultCount instead.")]
    public bool ShowSearchResultCount
    {
        get => ExplorerBrowserControl?.ShowSearchResultCount ?? true;
        set { if (ExplorerBrowserControl != null)
            {
                ExplorerBrowserControl.ShowSearchResultCount = value;
            }
        }
    }
    /// <summary>
    /// Gets or sets whether to enable search result virtual scrolling for large result sets.
    /// </summary>
    [Obsolete("Use SearchOptions.EnableVirtualScrolling instead.")]
    public bool EnableVirtualScrolling
    {
        get => ExplorerBrowserControl?.EnableVirtualScrolling ?? true;
        set { if (ExplorerBrowserControl != null)
            {
                ExplorerBrowserControl.EnableVirtualScrolling = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the virtual scrolling page size for search results.
    /// </summary>
    [Obsolete("Use SearchOptions.VirtualScrollingPageSize instead.")]
    public int VirtualScrollingPageSize
    {
        get => ExplorerBrowserControl?.VirtualScrollingPageSize ?? 100;
        set { if (ExplorerBrowserControl != null)
            {
                ExplorerBrowserControl.VirtualScrollingPageSize = value;
            }
        }
    }

    #endregion


    /// <summary>
    /// The location the explorer browser is navigating to
    /// </summary>
    public ShellObject NavigationTarget
    {
        get => (ShellObject)GetValue(NavigationTargetProperty);
        set => SetValue(NavigationTargetProperty, value);
    }

    /// <summary>
    /// The DependencyProperty for the NavigationTarget property
    /// </summary>
    public static readonly DependencyProperty NavigationTargetProperty =
        DependencyProperty.Register(
            @"NavigationTarget", typeof(ShellObject),
            typeof(ExplorerBrowser),
            new PropertyMetadata(null, NavigationTargetChanged));

    private static void NavigationTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ExplorerBrowser? instance = d as ExplorerBrowser;

        if (instance != null && instance.ExplorerBrowserControl.ExplorerBrowserControl != null)
        {
            instance.ExplorerBrowserControl.Navigate((ShellObject)e.NewValue);
        }
        else
        {
            instance!._initialNavigationTarget = e.NewValue as ShellObject;
        }
    }

    /// <summary>
    /// The view should be left-aligned. 
    /// </summary>
    public bool AlignLeft
    {
        get => (bool)GetValue(AlignLeftProperty);
        set => SetValue(AlignLeftProperty, value);
    }

    internal static DependencyProperty AlignLeftProperty =
        DependencyProperty.Register(
            "AlignLeft", typeof(bool),
            typeof(ExplorerBrowser),
            new FrameworkPropertyMetadata(false, OnAlignLeftChanged));

    private static void OnAlignLeftChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ExplorerBrowser? instance = d as ExplorerBrowser;
        if (instance?.ExplorerBrowserControl != null)
        {
            instance.ExplorerBrowserControl.ContentOptions.AlignLeft = (bool)e.NewValue;
        }
    }


    /// <summary>
    /// Automatically arrange the elements in the view. 
    /// </summary>
    public bool AutoArrange
    {
        get => (bool)GetValue(AutoArrangeProperty);
        set => SetValue(AutoArrangeProperty, value);
    }

    internal static DependencyProperty AutoArrangeProperty =
        DependencyProperty.Register(
            "AutoArrange", typeof(bool),
            typeof(ExplorerBrowser),
            new FrameworkPropertyMetadata(false, OnAutoArrangeChanged));

    private static void OnAutoArrangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ExplorerBrowser? instance = d as ExplorerBrowser;
        if (instance?.ExplorerBrowserControl != null)
        {
            instance.ExplorerBrowserControl.ContentOptions.AutoArrange = (bool)e.NewValue;
        }
    }

    /// <summary>
    /// Turns on check mode for the view
    /// </summary>
    public bool CheckSelect
    {
        get => (bool)GetValue(CheckSelectProperty);
        set => SetValue(CheckSelectProperty, value);
    }

    internal static DependencyProperty CheckSelectProperty =
        DependencyProperty.Register(
            "CheckSelect", typeof(bool),
            typeof(ExplorerBrowser),
            new FrameworkPropertyMetadata(false, OnCheckSelectChanged));

    private static void OnCheckSelectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ExplorerBrowser? instance = d as ExplorerBrowser;
        if (instance?.ExplorerBrowserControl != null)
        {
            instance.ExplorerBrowserControl.ContentOptions.CheckSelect = (bool)e.NewValue;
        }
    }

    /// <summary>
    /// When the view is in "tile view mode" the layout of a single item should be extended to the width of the view.
    /// </summary>
    public bool ExtendedTiles
    {
        get => (bool)GetValue(ExtendedTilesProperty);
        set => SetValue(ExtendedTilesProperty, value);
    }

    internal static DependencyProperty ExtendedTilesProperty =
        DependencyProperty.Register(
            "ExtendedTiles", typeof(bool),
            typeof(ExplorerBrowser),
            new FrameworkPropertyMetadata(false, OnExtendedTilesChanged));

    private static void OnExtendedTilesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ExplorerBrowser? instance = d as ExplorerBrowser;
        if (instance?.ExplorerBrowserControl != null)
        {
            instance.ExplorerBrowserControl.ContentOptions.ExtendedTiles = (bool)e.NewValue;
        }
    }

    /// <summary>
    /// When an item is selected, the item and all its sub-items are highlighted.
    /// </summary>
    public bool FullRowSelect
    {
        get => (bool)GetValue(FullRowSelectProperty);
        set => SetValue(FullRowSelectProperty, value);
    }

    internal static DependencyProperty FullRowSelectProperty =
        DependencyProperty.Register(
            "FullRowSelect", typeof(bool),
            typeof(ExplorerBrowser),
            new FrameworkPropertyMetadata(false, OnFullRowSelectChanged));

    private static void OnFullRowSelectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ExplorerBrowser? instance = d as ExplorerBrowser;
        if (instance?.ExplorerBrowserControl != null)
        {
            instance.ExplorerBrowserControl.ContentOptions.FullRowSelect = (bool)e.NewValue;
        }
    }

    /// <summary>
    /// The view should not display file names
    /// </summary>
    public bool HideFileNames
    {
        get => (bool)GetValue(HideFileNamesProperty);
        set => SetValue(HideFileNamesProperty, value);
    }

    internal static DependencyProperty HideFileNamesProperty =
        DependencyProperty.Register(
            "HideFileNames", typeof(bool),
            typeof(ExplorerBrowser),
            new FrameworkPropertyMetadata(false, OnHideFileNamesChanged));

    private static void OnHideFileNamesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ExplorerBrowser? instance = d as ExplorerBrowser;
        if (instance?.ExplorerBrowserControl != null)
        {
            instance.ExplorerBrowserControl.ContentOptions.HideFileNames = (bool)e.NewValue;
        }
    }

    /// <summary>
    /// The view should not save view state in the browser.
    /// </summary>
    public bool NoBrowserViewState
    {
        get => (bool)GetValue(NoBrowserViewStateProperty);
        set => SetValue(NoBrowserViewStateProperty, value);
    }

    internal static DependencyProperty NoBrowserViewStateProperty =
        DependencyProperty.Register(
            "NoBrowserViewState", typeof(bool),
            typeof(ExplorerBrowser),
            new FrameworkPropertyMetadata(false, OnNoBrowserViewStateChanged));

    private static void OnNoBrowserViewStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ExplorerBrowser? instance = d as ExplorerBrowser;
        if (instance?.ExplorerBrowserControl != null)
        {
            instance.ExplorerBrowserControl.ContentOptions.NoBrowserViewState = (bool)e.NewValue;
        }
    }

    /// <summary>
    /// Do not display a column header in the view in any view mode.
    /// </summary>
    public bool NoColumnHeader
    {
        get => (bool)GetValue(NoColumnHeaderProperty);
        set => SetValue(NoColumnHeaderProperty, value);
    }

    internal static DependencyProperty NoColumnHeaderProperty =
        DependencyProperty.Register(
            "NoColumnHeader", typeof(bool),
            typeof(ExplorerBrowser),
            new FrameworkPropertyMetadata(false, OnNoColumnHeaderChanged));

    private static void OnNoColumnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ExplorerBrowser? instance = d as ExplorerBrowser;
        if (instance?.ExplorerBrowserControl != null)
        {
            instance.ExplorerBrowserControl.ContentOptions.NoColumnHeader = (bool)e.NewValue;
        }
    }

    /// <summary>
    /// Only show the column header in details view mode.
    /// </summary>
    public bool NoHeaderInAllViews
    {
        get => (bool)GetValue(NoHeaderInAllViewsProperty);
        set => SetValue(NoHeaderInAllViewsProperty, value);
    }

    internal static DependencyProperty NoHeaderInAllViewsProperty =
        DependencyProperty.Register(
            "NoHeaderInAllViews", typeof(bool),
            typeof(ExplorerBrowser),
            new FrameworkPropertyMetadata(false, OnNoHeaderInAllViewsChanged));

    private static void OnNoHeaderInAllViewsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ExplorerBrowser? instance = d as ExplorerBrowser;
        if (instance?.ExplorerBrowserControl != null)
        {
            instance.ExplorerBrowserControl.ContentOptions.NoHeaderInAllViews = (bool)e.NewValue;
        }
    }

    /// <summary>
    /// The view should not display icons. 
    /// </summary>
    public bool NoIcons
    {
        get => (bool)GetValue(NoIconsProperty);
        set => SetValue(NoIconsProperty, value);
    }

    internal static DependencyProperty NoIconsProperty =
        DependencyProperty.Register(
            "NoIcons", typeof(bool),
            typeof(ExplorerBrowser),
            new FrameworkPropertyMetadata(false, OnNoIconsChanged));

    private static void OnNoIconsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ExplorerBrowser? instance = d as ExplorerBrowser;
        if (instance?.ExplorerBrowserControl != null)
        {
            instance.ExplorerBrowserControl.ContentOptions.NoIcons = (bool)e.NewValue;
        }
    }

    /// <summary>
    /// Do not show subfolders. 
    /// </summary>
    public bool NoSubfolders
    {
        get => (bool)GetValue(NoSubfoldersProperty);
        set => SetValue(NoSubfoldersProperty, value);
    }

    internal static DependencyProperty NoSubfoldersProperty =
        DependencyProperty.Register(
            "NoSubfolders", typeof(bool),
            typeof(ExplorerBrowser),
            new FrameworkPropertyMetadata(false, OnNoSubfoldersChanged));

    private static void OnNoSubfoldersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ExplorerBrowser? instance = d as ExplorerBrowser;
        if (instance?.ExplorerBrowserControl != null)
        {
            instance.ExplorerBrowserControl.ContentOptions.NoSubfolders = (bool)e.NewValue;
        }
    }

    /// <summary>
    /// Navigate with a single click
    /// </summary>
    public bool SingleClickActivate
    {
        get => (bool)GetValue(SingleClickActivateProperty);
        set => SetValue(SingleClickActivateProperty, value);
    }

    internal static DependencyProperty SingleClickActivateProperty =
        DependencyProperty.Register(
            "SingleClickActivate", typeof(bool),
            typeof(ExplorerBrowser),
            new FrameworkPropertyMetadata(false, OnSingleClickActivateChanged));

    private static void OnSingleClickActivateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ExplorerBrowser? instance = d as ExplorerBrowser;
        if (instance?.ExplorerBrowserControl != null)
        {
            instance.ExplorerBrowserControl.ContentOptions.SingleClickActivate = (bool)e.NewValue;
        }
    }

    /// <summary>
    /// Do not allow more than a single item to be selected.
    /// </summary>
    public bool SingleSelection
    {
        get => (bool)GetValue(SingleSelectionProperty);
        set => SetValue(SingleSelectionProperty, value);
    }

    internal static DependencyProperty SingleSelectionProperty =
        DependencyProperty.Register(
            "SingleSelection", typeof(bool),
            typeof(ExplorerBrowser),
            new FrameworkPropertyMetadata(false, OnSingleSelectionChanged));

    private static void OnSingleSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ExplorerBrowser? instance = d as ExplorerBrowser;
        if (instance?.ExplorerBrowserControl != null)
        {
            instance.ExplorerBrowserControl.ContentOptions.SingleSelection = (bool)e.NewValue;
        }
    }

    /// <summary>
    /// The size of the thumbnails in the explorer browser
    /// </summary>
    public int ThumbnailSize
    {
        get => (int)GetValue(ThumbnailSizeProperty);
        set => SetValue(ThumbnailSizeProperty, value);
    }

    internal static DependencyProperty ThumbnailSizeProperty =
        DependencyProperty.Register(
            "ThumbnailSize", typeof(int),
            typeof(ExplorerBrowser),
            new PropertyMetadata(32, OnThumbnailSizeChanged));

    private static void OnThumbnailSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ExplorerBrowser? instance = d as ExplorerBrowser;
        if (instance?.ExplorerBrowserControl != null)
        {
            instance.ExplorerBrowserControl.ContentOptions.ThumbnailSize = (int)e.NewValue;
        }
    }



    /// <summary>
    /// The various view modes of the explorer browser control
    /// </summary>
    public ExplorerBrowserViewMode ViewMode
    {
        get => (ExplorerBrowserViewMode)GetValue(ViewModeProperty);
        set => SetValue(ViewModeProperty, value);
    }

    internal static DependencyProperty ViewModeProperty =
        DependencyProperty.Register(
            "ViewMode", typeof(ExplorerBrowserViewMode),
            typeof(ExplorerBrowser),
            new PropertyMetadata(ExplorerBrowserViewMode.Auto, OnViewModeChanged));

    private static void OnViewModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ExplorerBrowser? instance = d as ExplorerBrowser;

        if (instance?.ExplorerBrowserControl != null)
        {
            if (instance.ExplorerBrowserControl.ExplorerBrowserControl == null)
            {
                instance._initialViewMode = (ExplorerBrowserViewMode)e.NewValue;
            }
            else
            {
                instance.ExplorerBrowserControl.ContentOptions.ViewMode = (ExplorerBrowserViewMode)e.NewValue;
            }
        }
    }


    /// <summary>
    /// Always navigate, even if you are attempting to navigate to the current folder.
    /// </summary>
    public bool AlwaysNavigate
    {
        get => (bool)GetValue(AlwaysNavigateProperty);
        set => SetValue(AlwaysNavigateProperty, value);
    }

    internal static DependencyProperty AlwaysNavigateProperty =
        DependencyProperty.Register(
            "AlwaysNavigate", typeof(bool),
            typeof(ExplorerBrowser),
            new FrameworkPropertyMetadata(false, OnAlwaysNavigateChanged));

    private static void OnAlwaysNavigateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ExplorerBrowser? instance = d as ExplorerBrowser;
        if (instance?.ExplorerBrowserControl != null)
        {
            instance.ExplorerBrowserControl.NavigationOptions.AlwaysNavigate = (bool)e.NewValue;
        }
    }

    /// <summary>
    /// Do not navigate further than the initial navigation.
    /// </summary>
    public bool NavigateOnce
    {
        get => (bool)GetValue(NavigateOnceProperty);
        set => SetValue(NavigateOnceProperty, value);
    }

    internal static DependencyProperty NavigateOnceProperty =
        DependencyProperty.Register(
            "NavigateOnce", typeof(bool),
            typeof(ExplorerBrowser),
            new FrameworkPropertyMetadata(false, OnNavigateOnceChanged));

    private static void OnNavigateOnceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ExplorerBrowser? instance = d as ExplorerBrowser;
        if (instance?.ExplorerBrowserControl != null)
        {
            instance.ExplorerBrowserControl.NavigationOptions.NavigateOnce = (bool)e.NewValue;
        }
    }

    /// <summary>
    /// Show/Hide the AdvancedQuery pane on subsequent navigation
    /// </summary>
    public PaneVisibilityState AdvancedQueryPane
    {
        get => (PaneVisibilityState)GetValue(AdvancedQueryPaneProperty);
        set => SetValue(AdvancedQueryPaneProperty, value);
    }

    internal static DependencyProperty AdvancedQueryPaneProperty =
        DependencyProperty.Register(
            "AdvancedQueryPane", typeof(PaneVisibilityState),
            typeof(ExplorerBrowser),
            new PropertyMetadata(PaneVisibilityState.DoNotCare, OnAdvancedQueryPaneChanged));

    private static void OnAdvancedQueryPaneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ExplorerBrowser? instance = d as ExplorerBrowser;
        if (instance?.ExplorerBrowserControl != null)
        {
            instance.ExplorerBrowserControl.NavigationOptions.PaneVisibility.AdvancedQuery = (PaneVisibilityState)e.NewValue;
        }
    }

    /// <summary>
    /// Show/Hide the Commands pane on subsequent navigation
    /// </summary>
    public PaneVisibilityState CommandsPane
    {
        get => (PaneVisibilityState)GetValue(CommandsPaneProperty);
        set => SetValue(CommandsPaneProperty, value);
    }

    internal static DependencyProperty CommandsPaneProperty =
        DependencyProperty.Register(
            "CommandsPane", typeof(PaneVisibilityState),
            typeof(ExplorerBrowser),
            new PropertyMetadata(PaneVisibilityState.DoNotCare, OnCommandsPaneChanged));

    private static void OnCommandsPaneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ExplorerBrowser? instance = d as ExplorerBrowser;
        if (instance?.ExplorerBrowserControl != null)
        {
            instance.ExplorerBrowserControl.NavigationOptions.PaneVisibility.Commands =
                (PaneVisibilityState)e.NewValue;
        }
    }

    /// <summary>
    /// Show/Hide the Organize menu in the Commands pane on subsequent navigation
    /// </summary>
    public PaneVisibilityState CommandsOrganizePane
    {
        get => (PaneVisibilityState)GetValue(CommandsOrganizePaneProperty);
        set => SetValue(CommandsOrganizePaneProperty, value);
    }

    internal static DependencyProperty CommandsOrganizePaneProperty =
        DependencyProperty.Register(
            "CommandsOrganizePane", typeof(PaneVisibilityState),
            typeof(ExplorerBrowser),
            new PropertyMetadata(PaneVisibilityState.DoNotCare, OnCommandsOrganizePaneChanged));

    private static void OnCommandsOrganizePaneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ExplorerBrowser? instance = d as ExplorerBrowser;
        if (instance?.ExplorerBrowserControl != null)
        {
            instance.ExplorerBrowserControl.NavigationOptions.PaneVisibility.CommandsOrganize =
                (PaneVisibilityState)e.NewValue;
        }
    }

    /// <summary>
    /// Show/Hide the View menu in the Commands pane on subsequent navigation
    /// </summary>
    public PaneVisibilityState CommandsViewPane
    {
        get => (PaneVisibilityState)GetValue(CommandsViewPaneProperty);
        set => SetValue(CommandsViewPaneProperty, value);
    }

    internal static DependencyProperty CommandsViewPaneProperty =
        DependencyProperty.Register(
            "CommandsViewPane", typeof(PaneVisibilityState),
            typeof(ExplorerBrowser),
            new PropertyMetadata(PaneVisibilityState.DoNotCare, OnCommandsViewPaneChanged));

    private static void OnCommandsViewPaneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ExplorerBrowser? instance = d as ExplorerBrowser;
        if (instance?.ExplorerBrowserControl != null)
        {
            instance.ExplorerBrowserControl.NavigationOptions.PaneVisibility.CommandsView = (PaneVisibilityState)e.NewValue;
        }
    }

    /// <summary>
    /// Show/Hide the Details pane on subsequent navigation
    /// </summary>
    public PaneVisibilityState DetailsPane
    {
        get => (PaneVisibilityState)GetValue(DetailsPaneProperty);
        set => SetValue(DetailsPaneProperty, value);
    }

    internal static DependencyProperty DetailsPaneProperty =
        DependencyProperty.Register(
            "DetailsPane", typeof(PaneVisibilityState),
            typeof(ExplorerBrowser),
            new PropertyMetadata(PaneVisibilityState.DoNotCare, OnDetailsPaneChanged));

    private static void OnDetailsPaneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ExplorerBrowser? instance = d as ExplorerBrowser;
        if (instance?.ExplorerBrowserControl != null)
        {
            instance.ExplorerBrowserControl.NavigationOptions.PaneVisibility.Details = (PaneVisibilityState)e.NewValue;
        }
    }

    /// <summary>
    /// Show/Hide the Navigation pane on subsequent navigation
    /// </summary>
    public PaneVisibilityState NavigationPane
    {
        get => (PaneVisibilityState)GetValue(NavigationPaneProperty);
        set => SetValue(NavigationPaneProperty, value);
    }

    internal static DependencyProperty NavigationPaneProperty =
        DependencyProperty.Register(
            "NavigationPane", typeof(PaneVisibilityState),
            typeof(ExplorerBrowser),
            new PropertyMetadata(PaneVisibilityState.DoNotCare, OnNavigationPaneChanged));

    private static void OnNavigationPaneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ExplorerBrowser? instance = d as ExplorerBrowser;
        if (instance?.ExplorerBrowserControl != null)
        {
            instance.ExplorerBrowserControl.NavigationOptions.PaneVisibility.Navigation = (PaneVisibilityState)e.NewValue;
        }
    }

    /// <summary>
    /// Show/Hide the Preview pane on subsequent navigation
    /// </summary>
    public PaneVisibilityState PreviewPane
    {
        get => (PaneVisibilityState)GetValue(PreviewPaneProperty);
        set => SetValue(PreviewPaneProperty, value);
    }

    internal static DependencyProperty PreviewPaneProperty =
        DependencyProperty.Register(
            "PreviewPane", typeof(PaneVisibilityState),
            typeof(ExplorerBrowser),
            new PropertyMetadata(PaneVisibilityState.DoNotCare, OnPreviewPaneChanged));

    private static void OnPreviewPaneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ExplorerBrowser? instance = d as ExplorerBrowser;
        if (instance?.ExplorerBrowserControl != null)
        {
            instance.ExplorerBrowserControl.NavigationOptions.PaneVisibility.Preview = (PaneVisibilityState)e.NewValue;
        }
    }

    /// <summary>
    /// Show/Hide the Query pane on subsequent navigation
    /// </summary>
    public PaneVisibilityState QueryPane
    {
        get => (PaneVisibilityState)GetValue(QueryPaneProperty);
        set => SetValue(QueryPaneProperty, value);
    }

    internal static DependencyProperty QueryPaneProperty =
        DependencyProperty.Register(
            "QueryPane", typeof(PaneVisibilityState),
            typeof(ExplorerBrowser),
            new PropertyMetadata(PaneVisibilityState.DoNotCare, OnQueryPaneChanged));

    private static void OnQueryPaneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ExplorerBrowser? instance = d as ExplorerBrowser;
        if (instance?.ExplorerBrowserControl != null)
        {
            instance.ExplorerBrowserControl.NavigationOptions.PaneVisibility.Query = (PaneVisibilityState)e.NewValue;
        }
    }


    /// <summary>
    /// Navigation log index
    /// </summary>
    public int NavigationLogIndex
    {
        get => (int)GetValue(NavigationLogIndexProperty);
        set => SetValue(NavigationLogIndexProperty, value);
    }

    internal static DependencyProperty NavigationLogIndexProperty =
        DependencyProperty.Register(
            "NavigationLogIndex", typeof(int),
            typeof(ExplorerBrowser),
            new PropertyMetadata(0, OnNavigationLogIndexChanged));

    private static void OnNavigationLogIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ExplorerBrowser? instance = d as ExplorerBrowser;
        if (instance?.ExplorerBrowserControl != null)
        {
            instance.ExplorerBrowserControl.NavigationLog.NavigateLog((int)e.NewValue);
        }
    }


    #region Search Methods

    /// <summary>
    /// Performs a search for files and folders within the specified scope and navigates to the results.
    /// </summary>
    /// <param name="searchQuery">The search query string to search for in filenames.</param>
    /// <param name="searchScope">The scope where to perform the search (e.g., KnownFolders.Documents).</param>
    public void Search(string searchQuery, ShellContainer searchScope)
    {
        ExplorerBrowserControl?.Search(searchQuery, searchScope);
    }

    /// <summary>
    /// Performs a search using a custom search condition and navigates to the results.
    /// </summary>
    /// <param name="searchCondition">The custom search condition to use.</param>
    /// <param name="searchScope">The scope where to perform the search.</param>
    public void Search(SearchCondition searchCondition, ShellContainer searchScope)
    {
        ExplorerBrowserControl?.Search(searchCondition, searchScope);
    }

    /// <summary>
    /// Performs a search using Windows Search syntax and navigates to the results.
    /// </summary>
    /// <param name="queryString">The search query in Windows Search format (e.g., "kind:picture AND datemodified:today").</param>
    /// <param name="searchScope">The scope where to perform the search.</param>
    /// <param name="cultureInfo">The culture info for parsing the query. If null, uses CurrentCulture.</param>
    public void SearchWithQueryString(string queryString, ShellContainer searchScope, System.Globalization.CultureInfo? cultureInfo = null)
    {
        ExplorerBrowserControl?.SearchWithQueryString(queryString, searchScope, cultureInfo);
    }

    /// <summary>
    /// Performs a search for files modified within a date range and navigates to the results.
    /// </summary>
    /// <param name="startDate">The start date for the search range.</param>
    /// <param name="endDate">The end date for the search range.</param>
    /// <param name="searchScope">The scope where to perform the search.</param>
    public void SearchByDateRange(DateTime startDate, DateTime endDate, ShellContainer searchScope)
    {
        ExplorerBrowserControl?.SearchByDateRange(startDate, endDate, searchScope);
    }

    /// <summary>
    /// Performs a search for files larger than a specified size and navigates to the results.
    /// </summary>
    /// <param name="minSizeBytes">The minimum file size in bytes.</param>
    /// <param name="searchScope">The scope where to perform the search.</param>
    public void SearchByFileSize(long minSizeBytes, ShellContainer searchScope)
    {
        ExplorerBrowserControl?.SearchByFileSize(minSizeBytes, searchScope);
    }

    /// <summary>
    /// Performs a search for files by a specific property and navigates to the results.
    /// </summary>
    /// <param name="propertyName">The name of the property to search (e.g., "System.Author", "System.Title").</param>
    /// <param name="propertyValue">The value to search for in the property.</param>
    /// <param name="operation">The search operation to perform.</param>
    /// <param name="searchScope">The scope where to perform the search.</param>
    public void SearchByProperty(string propertyName, string propertyValue, SearchConditionOperation operation, ShellContainer searchScope)
    {
        ExplorerBrowserControl?.SearchByProperty(propertyName, propertyValue, operation, searchScope);
    }

    /// <summary>
    /// Clears the current search and navigates to the specified location.
    /// </summary>
    /// <param name="location">The location to navigate to after clearing the search.</param>
    public void ClearSearch(ShellObject location)
    {
        ExplorerBrowserControl?.ClearSearch(location);
    }

    /// <summary>
    /// Gets whether the current view is showing search results.
    /// </summary>
    public bool IsShowingSearchResults => ExplorerBrowserControl?.IsShowingSearchResults ?? false;

    #endregion

    #region IDisposable Members

    /// <summary>
    /// Disposes the class
    /// </summary>        
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the browser.
    /// </summary>
    /// <param name="disposed"></param>
    protected virtual void Dispose(bool disposed)
    {
        if (disposed)
        {
            if (_itemsChanged != null)
            {
                _itemsChanged.Close();
            }

            if (_selectionChanged != null)
            {
                _selectionChanged.Close();
            }
        }
    }

    #endregion
}