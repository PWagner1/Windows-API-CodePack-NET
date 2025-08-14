namespace Microsoft.WindowsAPICodePack.Controls;

/// <summary>
/// Groups all search state properties for the ExplorerBrowser control.
/// This class can be used with ExpandableObjectConverter in a PropertyGrid.
/// </summary>
[TypeConverter(typeof(ExpandableObjectConverter))]
public class ExplorerBrowserSearchState : INotifyPropertyChanged
{
    private string? _currentSearchQuery;
    private ShellContainer? _currentSearchScope;
    private bool _isShowingSearchResults;

    /// <summary>
    /// Gets the current search query string.
    /// </summary>
    [Description("The current search query string.")]
    [Category("Search State")]
    [ReadOnly(true)]
    public string? CurrentSearchQuery
    {
        get => _currentSearchQuery;
        internal set
        {
            if (_currentSearchQuery != value)
            {
                _currentSearchQuery = value;
                OnPropertyChanged(nameof(CurrentSearchQuery));
            }
        }
    }

    /// <summary>
    /// Gets the current search scope (folder being searched).
    /// </summary>
    [Description("The current search scope (folder being searched).")]
    [Category("Search State")]
    [ReadOnly(true)]
    public ShellContainer? CurrentSearchScope
    {
        get => _currentSearchScope;
        internal set
        {
            if (_currentSearchScope != value)
            {
                _currentSearchScope = value;
                OnPropertyChanged(nameof(CurrentSearchScope));
            }
        }
    }

    /// <summary>
    /// Gets whether the ExplorerBrowser is currently showing search results.
    /// </summary>
    [Description("Whether the ExplorerBrowser is currently showing search results.")]
    [Category("Search State")]
    [ReadOnly(true)]
    public bool IsShowingSearchResults
    {
        get => _isShowingSearchResults;
        internal set
        {
            if (_isShowingSearchResults != value)
            {
                _isShowingSearchResults = value;
                OnPropertyChanged(nameof(IsShowingSearchResults));
            }
        }
    }

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
    /// Returns a string representation of the search state.
    /// </summary>
    /// <returns>A string describing the current search state.</returns>
    public override string ToString()
    {
        if (IsShowingSearchResults)
        {
            return $"Searching: {CurrentSearchQuery ?? "Unknown"} in {CurrentSearchScope?.Name ?? "Unknown"}";
        }
        return "No active search";
    }
}