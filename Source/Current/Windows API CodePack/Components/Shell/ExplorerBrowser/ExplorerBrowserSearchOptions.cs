using Color = System.Drawing.Color;

namespace Microsoft.WindowsAPICodePack.Controls;

/// <summary>
/// Groups all search-related properties for the ExplorerBrowser control.
/// This class can be used with ExpandableObjectConverter in a PropertyGrid.
/// </summary>
[TypeConverter(typeof(ExpandableObjectConverter))]
public class ExplorerBrowserSearchOptions : INotifyPropertyChanged
{
    #region Instance Fields

    private bool _autoRefreshSearchResults = true;
    private int _maxSearchResults = 0;
    private bool _showSearchProgress = true;
    private bool _cacheSearchResults = true;
    private SearchResultSortOrder _searchResultSortOrder = SearchResultSortOrder.Relevance;
    private SearchResultGrouping _groupSearchResultsByType = SearchResultGrouping.None;
    private bool _showFilePreviews = true;
    private int _searchResultThumbnailSize = 32;
    private bool _enableIncrementalSearch = false;
    private int _incrementalSearchDelay = 500;
    private bool _showSearchSuggestions = true;
    private int _maxSearchSuggestions = 10;
    private bool _highlightSearchTerms = true;
    private Color _searchTermHighlightColor = Color.Yellow;
    private bool _enableAdvancedSearchFilters = false;
    private bool _rememberSearchHistory = true;
    private int _maxSearchHistoryEntries = 50;
    private bool _enableSearchResultExport = false;
    private SearchResultExportFormat _defaultExportFormat = SearchResultExportFormat.CSV;
    private bool _showSearchStatistics = false;
    private bool _enableSearchResultSharing = false;
    private bool _showSearchResultContextMenu = true;
    private bool _enableSearchResultDragDrop = true;
    private bool _showSearchResultTooltips = true;
    private bool _persistSearchResultSelection = true;
    private bool _showSearchResultCount = true;
    private bool _enableVirtualScrolling = false;
    private int _virtualScrollingPageSize = 100;

    #endregion

    #region Public

    /// <summary>
    /// Gets or sets whether search results should automatically refresh when the underlying data changes.
    /// </summary>
    [Description("Whether search results should automatically refresh when the underlying data changes.")]
    [Category("Search Behavior")]
    [DefaultValue(true)]
    public bool AutoRefreshSearchResults
    {
        get => _autoRefreshSearchResults;
        set
        {
            if (_autoRefreshSearchResults != value)
            {
                _autoRefreshSearchResults = value;
                OnPropertyChanged(nameof(AutoRefreshSearchResults));
            }
        }
    }

    /// <summary>
    /// Gets or sets the maximum number of search results to display (0 = unlimited).
    /// </summary>
    [Description("The maximum number of search results to display (0 = unlimited).")]
    [Category("Search Behavior")]
    [DefaultValue(0)]
    public int MaxSearchResults
    {
        get => _maxSearchResults;
        set
        {
            if (_maxSearchResults != value)
            {
                _maxSearchResults = value;
                OnPropertyChanged(nameof(MaxSearchResults));
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to show search progress indicators during search operations.
    /// </summary>
    [Description("Whether to show search progress indicators during search operations.")]
    [Category("Search Behavior")]
    [DefaultValue(true)]
    public bool ShowSearchProgress
    {
        get => _showSearchProgress;
        set
        {
            if (_showSearchProgress != value)
            {
                _showSearchProgress = value;
                OnPropertyChanged(nameof(ShowSearchProgress));
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to cache search results for better performance.
    /// </summary>
    [Description("Whether to cache search results for better performance.")]
    [Category("Search Behavior")]
    [DefaultValue(true)]
    public bool CacheSearchResults
    {
        get => _cacheSearchResults;
        set
        {
            if (_cacheSearchResults != value)
            {
                _cacheSearchResults = value;
                OnPropertyChanged(nameof(CacheSearchResults));
            }
        }
    }

    /// <summary>
    /// Gets or sets the sort order for search results.
    /// </summary>
    [Description("The sort order for search results.")]
    [Category("Display Options")]
    [DefaultValue(SearchResultSortOrder.Relevance)]
    public SearchResultSortOrder SearchResultSortOrder
    {
        get => _searchResultSortOrder;
        set
        {
            if (_searchResultSortOrder != value)
            {
                _searchResultSortOrder = value;
                OnPropertyChanged(nameof(SearchResultSortOrder));
            }
        }
    }

    /// <summary>
    /// Gets or sets how search results should be grouped.
    /// </summary>
    [Description("How search results should be grouped.")]
    [Category("Display Options")]
    [DefaultValue(SearchResultGrouping.None)]
    public SearchResultGrouping GroupSearchResultsByType
    {
        get => _groupSearchResultsByType;
        set
        {
            if (_groupSearchResultsByType != value)
            {
                _groupSearchResultsByType = value;
                OnPropertyChanged(nameof(GroupSearchResultsByType));
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to show file previews for search results.
    /// </summary>
    [Description("Whether to show file previews for search results.")]
    [Category("Display Options")]
    [DefaultValue(true)]
    public bool ShowFilePreviews
    {
        get => _showFilePreviews;
        set
        {
            if (_showFilePreviews != value)
            {
                _showFilePreviews = value;
                OnPropertyChanged(nameof(ShowFilePreviews));
            }
        }
    }

    /// <summary>
    /// Gets or sets the thumbnail size for search result items.
    /// </summary>
    [Description("The thumbnail size for search result items.")]
    [Category("Display Options")]
    [DefaultValue(32)]
    public int SearchResultThumbnailSize
    {
        get => _searchResultThumbnailSize;
        set
        {
            if (_searchResultThumbnailSize != value)
            {
                _searchResultThumbnailSize = value;
                OnPropertyChanged(nameof(SearchResultThumbnailSize));
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to enable incremental search as the user types.
    /// </summary>
    [Description("Whether to enable incremental search as the user types.")]
    [Category("Advanced Features")]
    [DefaultValue(false)]
    public bool EnableIncrementalSearch
    {
        get => _enableIncrementalSearch;
        set
        {
            if (_enableIncrementalSearch != value)
            {
                _enableIncrementalSearch = value;
                OnPropertyChanged(nameof(EnableIncrementalSearch));
            }
        }
    }

    /// <summary>
    /// Gets or sets the delay in milliseconds before performing incremental search.
    /// </summary>
    [Description("The delay in milliseconds before performing incremental search.")]
    [Category("Advanced Features")]
    [DefaultValue(500)]
    public int IncrementalSearchDelay
    {
        get => _incrementalSearchDelay;
        set
        {
            if (_incrementalSearchDelay != value)
            {
                _incrementalSearchDelay = value;
                OnPropertyChanged(nameof(IncrementalSearchDelay));
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to show search suggestions based on user input.
    /// </summary>
    [Description("Whether to show search suggestions based on user input.")]
    [Category("Advanced Features")]
    [DefaultValue(true)]
    public bool ShowSearchSuggestions
    {
        get => _showSearchSuggestions;
        set
        {
            if (_showSearchSuggestions != value)
            {
                _showSearchSuggestions = value;
                OnPropertyChanged(nameof(ShowSearchSuggestions));
            }
        }
    }

    /// <summary>
    /// Gets or sets the maximum number of search suggestions to display.
    /// </summary>
    [Description("The maximum number of search suggestions to display.")]
    [Category("Advanced Features")]
    [DefaultValue(10)]
    public int MaxSearchSuggestions
    {
        get => _maxSearchSuggestions;
        set
        {
            if (_maxSearchSuggestions != value)
            {
                _maxSearchSuggestions = value;
                OnPropertyChanged(nameof(MaxSearchSuggestions));
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to highlight search terms in search results.
    /// </summary>
    [Description("Whether to highlight search terms in search results.")]
    [Category("Visual Enhancements")]
    [DefaultValue(true)]
    public bool HighlightSearchTerms
    {
        get => _highlightSearchTerms;
        set
        {
            if (_highlightSearchTerms != value)
            {
                _highlightSearchTerms = value;
                OnPropertyChanged(nameof(HighlightSearchTerms));
            }
        }
    }

    /// <summary>
    /// Gets or sets the color used to highlight search terms.
    /// </summary>
    [Description("The color used to highlight search terms.")]
    [Category("Visual Enhancements")]
    [DefaultValue(typeof(Color), "Yellow")]
    public Color SearchTermHighlightColor
    {
        get => _searchTermHighlightColor;
        set
        {
            if (_searchTermHighlightColor != value)
            {
                _searchTermHighlightColor = value;
                OnPropertyChanged(nameof(SearchTermHighlightColor));
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to enable advanced search filters.
    /// </summary>
    [Description("Whether to enable advanced search filters.")]
    [Category("Advanced Features")]
    [DefaultValue(false)]
    public bool EnableAdvancedSearchFilters
    {
        get => _enableAdvancedSearchFilters;
        set
        {
            if (_enableAdvancedSearchFilters != value)
            {
                _enableAdvancedSearchFilters = value;
                OnPropertyChanged(nameof(EnableAdvancedSearchFilters));
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to remember search history.
    /// </summary>
    [Description("Whether to remember search history.")]
    [Category("History and Persistence")]
    [DefaultValue(true)]
    public bool RememberSearchHistory
    {
        get => _rememberSearchHistory;
        set
        {
            if (_rememberSearchHistory != value)
            {
                _rememberSearchHistory = value;
                OnPropertyChanged(nameof(RememberSearchHistory));
            }
        }
    }

    /// <summary>
    /// Gets or sets the maximum number of search history entries to remember.
    /// </summary>
    [Description("The maximum number of search history entries to remember.")]
    [Category("History and Persistence")]
    [DefaultValue(50)]
    public int MaxSearchHistoryEntries
    {
        get => _maxSearchHistoryEntries;
        set
        {
            if (_maxSearchHistoryEntries != value)
            {
                _maxSearchHistoryEntries = value;
                OnPropertyChanged(nameof(MaxSearchHistoryEntries));
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to enable exporting search results.
    /// </summary>
    [Description("Whether to enable exporting search results.")]
    [Category("Export and Sharing")]
    [DefaultValue(false)]
    public bool EnableSearchResultExport
    {
        get => _enableSearchResultExport;
        set
        {
            if (_enableSearchResultExport != value)
            {
                _enableSearchResultExport = value;
                OnPropertyChanged(nameof(EnableSearchResultExport));
            }
        }
    }

    /// <summary>
    /// Gets or sets the default export format for search results.
    /// </summary>
    [Description("The default export format for search results.")]
    [Category("Export and Sharing")]
    [DefaultValue(SearchResultExportFormat.CSV)]
    public SearchResultExportFormat DefaultExportFormat
    {
        get => _defaultExportFormat;
        set
        {
            if (_defaultExportFormat != value)
            {
                _defaultExportFormat = value;
                OnPropertyChanged(nameof(DefaultExportFormat));
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to show search statistics.
    /// </summary>
    [Description("Whether to show search statistics.")]
    [Category("Export and Sharing")]
    [DefaultValue(false)]
    public bool ShowSearchStatistics
    {
        get => _showSearchStatistics;
        set
        {
            if (_showSearchStatistics != value)
            {
                _showSearchStatistics = value;
                OnPropertyChanged(nameof(ShowSearchStatistics));
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to enable sharing search results.
    /// </summary>
    [Description("Whether to enable sharing search results.")]
    [Category("Export and Sharing")]
    [DefaultValue(false)]
    public bool EnableSearchResultSharing
    {
        get => _enableSearchResultSharing;
        set
        {
            if (_enableSearchResultSharing != value)
            {
                _enableSearchResultSharing = value;
                OnPropertyChanged(nameof(EnableSearchResultSharing));
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to show context menu for search results.
    /// </summary>
    [Description("Whether to show context menu for search results.")]
    [Category("User Experience")]
    [DefaultValue(true)]
    public bool ShowSearchResultContextMenu
    {
        get => _showSearchResultContextMenu;
        set
        {
            if (_showSearchResultContextMenu != value)
            {
                _showSearchResultContextMenu = value;
                OnPropertyChanged(nameof(ShowSearchResultContextMenu));
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to enable drag and drop for search results.
    /// </summary>
    [Description("Whether to enable drag and drop for search results.")]
    [Category("User Experience")]
    [DefaultValue(true)]
    public bool EnableSearchResultDragDrop
    {
        get => _enableSearchResultDragDrop;
        set
        {
            if (_enableSearchResultDragDrop != value)
            {
                _enableSearchResultDragDrop = value;
                OnPropertyChanged(nameof(EnableSearchResultDragDrop));
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to show tooltips for search results.
    /// </summary>
    [Description("Whether to show tooltips for search results.")]
    [Category("User Experience")]
    [DefaultValue(true)]
    public bool ShowSearchResultTooltips
    {
        get => _showSearchResultTooltips;
        set
        {
            if (_showSearchResultTooltips != value)
            {
                _showSearchResultTooltips = value;
                OnPropertyChanged(nameof(ShowSearchResultTooltips));
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to persist search result selection across operations.
    /// </summary>
    [Description("Whether to persist search result selection across operations.")]
    [Category("User Experience")]
    [DefaultValue(true)]
    public bool PersistSearchResultSelection
    {
        get => _persistSearchResultSelection;
        set
        {
            if (_persistSearchResultSelection != value)
            {
                _persistSearchResultSelection = value;
                OnPropertyChanged(nameof(PersistSearchResultSelection));
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to show the count of search results.
    /// </summary>
    [Description("Whether to show the count of search results.")]
    [Category("User Experience")]
    [DefaultValue(true)]
    public bool ShowSearchResultCount
    {
        get => _showSearchResultCount;
        set
        {
            if (_showSearchResultCount != value)
            {
                _showSearchResultCount = value;
                OnPropertyChanged(nameof(ShowSearchResultCount));
            }
        }
    }

    /// <summary>
    /// Gets or sets whether to enable virtual scrolling for large search result sets.
    /// </summary>
    [Description("Whether to enable virtual scrolling for large search result sets.")]
    [Category("Performance Optimization")]
    [DefaultValue(false)]
    public bool EnableVirtualScrolling
    {
        get => _enableVirtualScrolling;
        set
        {
            if (_enableVirtualScrolling != value)
            {
                _enableVirtualScrolling = value;
                OnPropertyChanged(nameof(EnableVirtualScrolling));
            }
        }
    }

    /// <summary>
    /// Gets or sets the page size for virtual scrolling.
    /// </summary>
    [Description("The page size for virtual scrolling.")]
    [Category("Performance Optimization")]
    [DefaultValue(100)]
    public int VirtualScrollingPageSize
    {
        get => _virtualScrollingPageSize;
        set
        {
            if (_virtualScrollingPageSize != value)
            {
                _virtualScrollingPageSize = value;
                OnPropertyChanged(nameof(VirtualScrollingPageSize));
            }
        }
    }

    #endregion

    /// <summary>
    /// Event raised when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises the PropertyChanged event.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Returns a string representation of the search options.
    /// </summary>
    /// <returns>A string describing the current search options.</returns>
    public override string ToString()
    {
        return $"Search Options (Max Results: {MaxSearchResults}, Sort: {SearchResultSortOrder}, Group: {GroupSearchResultsByType})";
    }
}