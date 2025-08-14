# ExplorerBrowser Properties Demo

This sample application demonstrates the new properties that have been added to the `ExplorerBrowser` control to enhance search functionality and provide better control over the user experience.

## Overview

The `ExplorerBrowser` control now includes a comprehensive set of properties that allow developers to:

- Configure search behavior and appearance
- Control search result display options
- Enable/disable advanced features
- Customize the search experience
- Optimize performance for large result sets

## New Properties

### Search State Properties

| Property | Type | Description |
|----------|------|-------------|
| `CurrentSearchQuery` | `string?` | Gets the current search query. Returns null if no search is active. |
| `CurrentSearchScope` | `ShellContainer?` | Gets the current search scope. Returns null if no search is active. |
| `IsShowingSearchResults` | `bool` | Gets whether the current view is showing search results. |

### Search Behavior Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `AutoRefreshSearchResults` | `bool` | `true` | Whether search results should be automatically refreshed when the search scope changes. |
| `MaxSearchResults` | `int` | `0` | Maximum number of search results to display. Set to 0 for unlimited results. |
| `ShowSearchProgress` | `bool` | `true` | Whether to show search progress indicators during long searches. |
| `CacheSearchResults` | `bool` | `true` | Whether to cache search results for better performance. |

### Display Options

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `SearchResultSortOrder` | `SearchResultSortOrder` | `Relevance` | The search result sorting order. |
| `GroupSearchResultsByType` | `bool` | `false` | Whether to group search results by file type. |
| `ShowFilePreviews` | `bool` | `true` | Whether to show file previews in search results. |
| `SearchResultThumbnailSize` | `int` | `96` | Thumbnail size for search results (32-256 pixels). |

### Advanced Features

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `EnableIncrementalSearch` | `bool` | `false` | Whether to enable incremental search (search as you type). |
| `IncrementalSearchDelay` | `int` | `500` | Delay in milliseconds before performing incremental search. |
| `ShowSearchSuggestions` | `bool` | `true` | Whether to show search suggestions based on user input. |
| `MaxSearchSuggestions` | `int` | `10` | Maximum number of search suggestions to display. |

### Visual Enhancements

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `HighlightSearchTerms` | `bool` | `true` | Whether to highlight search terms in search results. |
| `SearchTermHighlightColor` | `Color` | `Yellow` | Color used to highlight search terms in search results. |
| `EnableAdvancedSearchFilters` | `bool` | `true` | Whether to enable advanced search filters (date, size, type, etc.). |

### History and Persistence

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `RememberSearchHistory` | `bool` | `true` | Whether to remember search history for quick access. |
| `MaxSearchHistoryEntries` | `int` | `50` | Maximum number of search history entries to remember. |

### Export and Sharing

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `EnableSearchResultExport` | `bool` | `false` | Whether to enable search result export functionality. |
| `DefaultExportFormat` | `SearchResultExportFormat` | `CSV` | Default export format for search results. |
| `ShowSearchStatistics` | `bool` | `true` | Whether to show search statistics (total results, search time, etc.). |
| `EnableSearchResultSharing` | `bool` | `false` | Whether to enable search result sharing functionality. |

### User Experience

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `ShowSearchResultContextMenu` | `bool` | `true` | Whether to show search result context menu with additional options. |
| `EnableSearchResultDragDrop` | `bool` | `true` | Whether to enable drag and drop of search results. |
| `ShowSearchResultTooltips` | `bool` | `true` | Whether to show search result tooltips with additional information. |
| `PersistSearchResultSelection` | `bool` | `false` | Whether to enable search result selection persistence across navigation. |
| `ShowSearchResultCount` | `bool` | `true` | Whether to show search result count in the status bar. |

### Performance Optimization

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `EnableVirtualScrolling` | `bool` | `true` | Whether to enable search result virtual scrolling for large result sets. |
| `VirtualScrollingPageSize` | `int` | `100` | Virtual scrolling page size for search results. |

## Enums

### SearchResultSortOrder

```csharp
public enum SearchResultSortOrder
{
    Relevance,              // Sort by relevance (default search ranking)
    NameAscending,          // Sort by file name in ascending order
    NameDescending,         // Sort by file name in descending order
    SizeAscending,          // Sort by file size in ascending order
    SizeDescending,         // Sort by file size in descending order
    DateModifiedAscending,  // Sort by date modified in ascending order
    DateModifiedDescending, // Sort by date modified in descending order
    TypeAscending,          // Sort by file type in ascending order
    TypeDescending,         // Sort by file type in descending order
    DateCreatedAscending,   // Sort by date created in ascending order
    DateCreatedDescending,  // Sort by date created in descending order
    AuthorAscending,        // Sort by author in ascending order
    AuthorDescending        // Sort by author in descending order
}
```

### SearchResultExportFormat

```csharp
public enum SearchResultExportFormat
{
    CSV,    // Export as comma-separated values
    TSV,    // Export as tab-separated values
    XML,    // Export as XML format
    JSON,   // Export as JSON format
    HTML,   // Export as HTML table format
    Text    // Export as plain text format
}
```

## Usage Examples

### Basic Property Configuration

```csharp
var explorerBrowser = new ExplorerBrowser();

// Configure search behavior
explorerBrowser.AutoRefreshSearchResults = true;
explorerBrowser.MaxSearchResults = 1000;
explorerBrowser.ShowSearchProgress = true;
explorerBrowser.CacheSearchResults = true;

// Configure display options
explorerBrowser.SearchResultSortOrder = SearchResultSortOrder.NameAscending;
explorerBrowser.GroupSearchResultsByType = true;
explorerBrowser.ShowFilePreviews = true;
explorerBrowser.SearchResultThumbnailSize = 128;

// Enable advanced features
explorerBrowser.EnableIncrementalSearch = true;
explorerBrowser.IncrementalSearchDelay = 300;
explorerBrowser.ShowSearchSuggestions = true;
explorerBrowser.MaxSearchSuggestions = 15;

// Configure visual enhancements
explorerBrowser.HighlightSearchTerms = true;
explorerBrowser.SearchTermHighlightColor = Color.LightBlue;
explorerBrowser.EnableAdvancedSearchFilters = true;
```

### Property Change Event Handling

```csharp
// Handle property changes
explorerBrowser.PropertyChanged += (sender, e) =>
{
    switch (e.PropertyName)
    {
        case nameof(ExplorerBrowser.CurrentSearchQuery):
            var query = explorerBrowser.CurrentSearchQuery;
            if (!string.IsNullOrEmpty(query))
            {
                Console.WriteLine($"Search query changed to: {query}");
            }
            break;
            
        case nameof(ExplorerBrowser.IsShowingSearchResults):
            if (explorerBrowser.IsShowingSearchResults)
            {
                Console.WriteLine("Now showing search results");
            }
            break;
    }
};
```

### Dynamic Property Updates

```csharp
// Update properties based on user preferences
private void UpdateSearchPreferences(SearchPreferences prefs)
{
    explorerBrowser.MaxSearchResults = prefs.MaxResults;
    explorerBrowser.SearchResultSortOrder = prefs.SortOrder;
    explorerBrowser.GroupSearchResultsByType = prefs.GroupByType;
    explorerBrowser.ShowFilePreviews = prefs.ShowPreviews;
    explorerBrowser.SearchResultThumbnailSize = prefs.ThumbnailSize;
    explorerBrowser.EnableIncrementalSearch = prefs.IncrementalSearch;
    explorerBrowser.IncrementalSearchDelay = prefs.SearchDelay;
    explorerBrowser.HighlightSearchTerms = prefs.HighlightTerms;
    explorerBrowser.SearchTermHighlightColor = prefs.HighlightColor;
}
```

## Requirements

- .NET 6.0 or later
- Windows 10 or later
- Windows API Code Pack 1.1 or later
- The `ExplorerBrowserContentOptions.Flags` must include `FolderOptions.UseSearchFolders`

## Performance Considerations

- **Virtual Scrolling**: Enable for large result sets (>1000 items) to improve performance
- **Caching**: Enable result caching for repeated searches in the same scope
- **Thumbnail Size**: Use smaller thumbnail sizes for better performance with many results
- **Max Results**: Limit results when exact count is not required

## Best Practices

1. **Set reasonable defaults**: Configure properties based on your application's needs
2. **Handle property changes**: Listen for property change events to update UI accordingly
3. **Validate property values**: Ensure numeric properties are within acceptable ranges
4. **Performance tuning**: Adjust performance-related properties based on user feedback
5. **Accessibility**: Consider users who may benefit from different property settings

## Migration from Previous Versions

If you're upgrading from a previous version of the Windows API Code Pack:

1. **No breaking changes**: Existing code will continue to work
2. **New properties are optional**: All new properties have sensible defaults
3. **Gradual adoption**: You can adopt new properties at your own pace
4. **Backward compatibility**: Old search methods still work as before

## Troubleshooting

### Common Issues

1. **Properties not taking effect**: Ensure the `ExplorerBrowser` control is fully initialized
2. **Performance issues**: Check if virtual scrolling and caching are enabled
3. **Visual glitches**: Verify thumbnail size and preview settings are appropriate
4. **Search not working**: Confirm `FolderOptions.UseSearchFolders` is enabled

### Debug Tips

- Use the demo application to test different property combinations
- Monitor property change events to track state changes
- Check the `IsShowingSearchResults` property to verify search state
- Use the `CurrentSearchQuery` and `CurrentSearchScope` properties for debugging

## Related Documentation

- [ExplorerBrowser Search Methods](../README.md#search-functionality)
- [ShellSearchFolder Class](../../../Common/ShellSearchFolder.cs)
- [SearchConditionFactory Class](../../../Common/SearchConditionFactory.cs)
- [Windows API Code Pack Overview](../../../../README.md)
