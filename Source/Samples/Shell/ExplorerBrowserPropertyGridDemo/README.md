# ExplorerBrowser PropertyGrid Demo

This demo application showcases the new `ExpandableObjectConverter` properties in the `ExplorerBrowser` control, specifically the `SearchState` and `SearchOptions` properties.

## Overview

The `ExplorerBrowser` control now provides organized, expandable properties that can be easily viewed and edited in a `PropertyGrid` control. This makes it much easier for developers to:

- View and modify search-related settings
- Understand the current search state
- Configure search behavior through a user-friendly interface

## Key Features

### 1. SearchState Properties
The `SearchState` property group contains read-only properties that show the current search status:

- **IsShowingSearchResults**: Boolean indicating if search results are currently displayed
- **CurrentSearchQuery**: The active search query string
- **CurrentSearchScope**: The folder being searched

### 2. SearchOptions Properties
The `SearchOptions` property group contains configurable properties organized into categories:

#### Search Behavior
- AutoRefreshSearchResults
- MaxSearchResults
- ShowSearchProgress
- CacheSearchResults

#### Display Options
- SearchResultSortOrder
- GroupSearchResultsByType
- ShowFilePreviews
- SearchResultThumbnailSize

#### Advanced Features
- EnableIncrementalSearch
- IncrementalSearchDelay
- ShowSearchSuggestions
- MaxSearchSuggestions

#### Visual Enhancements
- HighlightSearchTerms
- SearchTermHighlightColor
- EnableAdvancedSearchFilters

#### History and Persistence
- RememberSearchHistory
- MaxSearchHistoryEntries

#### Export and Sharing
- EnableSearchResultExport
- DefaultExportFormat
- ShowSearchStatistics
- EnableSearchResultSharing

#### User Experience
- ShowSearchResultContextMenu
- EnableSearchResultDragDrop
- ShowSearchResultTooltips
- PersistSearchResultSelection
- ShowSearchResultCount

#### Performance Optimization
- EnableVirtualScrolling
- VirtualScrollingPageSize

## How to Use

### In a PropertyGrid
```csharp
// Create a PropertyGrid
PropertyGrid propertyGrid = new PropertyGrid();
propertyGrid.ToolbarVisible = true;
propertyGrid.HelpVisible = true;

// Set the ExplorerBrowser as the selected object
propertyGrid.SelectedObject = explorerBrowser;
```

### Accessing Properties Programmatically
```csharp
// Access search state
bool isSearching = explorerBrowser.SearchState.IsShowingSearchResults;
string currentQuery = explorerBrowser.SearchState.CurrentSearchQuery;

// Configure search options
explorerBrowser.SearchOptions.MaxSearchResults = 100;
explorerBrowser.SearchOptions.EnableIncrementalSearch = true;
explorerBrowser.SearchOptions.SearchResultSortOrder = SearchResultSortOrder.NameAscending;
```

### Property Categories
Properties are automatically categorized in the PropertyGrid:
- **Search**: Contains SearchState and SearchOptions
- **Navigation**: Navigation-related properties
- **Content**: Content display options
- **Events**: Event handlers

## Benefits of ExpandableObjectConverter

1. **Better Organization**: Related properties are grouped together
2. **Improved Readability**: Properties are categorized and easier to find
3. **Professional Appearance**: Properties look organized in PropertyGrid controls
4. **Easier Configuration**: Developers can quickly locate and modify specific settings
5. **Better Documentation**: Each property has descriptions and default values

## Backward Compatibility

All existing properties are still available but marked as `[Obsolete]` with helpful messages directing developers to use the new grouped properties. This ensures existing code continues to work while encouraging migration to the new structure.

## Requirements

- .NET 6.0 or later
- Windows Forms or WPF
- Windows API Code Pack .NET

## Running the Demo

1. Build the solution
2. Run the `ExplorerBrowserPropertyGridDemo` project
3. The left panel shows the ExplorerBrowser control
4. The right panel shows the PropertyGrid with expandable properties
5. Expand the "Search" category to see SearchState and SearchOptions
6. Modify properties to see how they affect the ExplorerBrowser behavior

## Integration Tips

- Use `SearchState` properties to monitor search status in your application
- Configure `SearchOptions` during application initialization
- Bind PropertyGrid to ExplorerBrowser for real-time property editing
- Consider creating custom PropertyGrid editors for complex property types
