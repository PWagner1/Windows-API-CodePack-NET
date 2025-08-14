# ExplorerBrowser Integrated Search Functionality

## Overview

The search functionality has been **directly integrated** into the `ExplorerBrowser` control in the Windows API Code Pack. This provides a clean, native API for performing searches without needing external helper classes or complex setup.

## What's New

The following search methods are now available directly on the `ExplorerBrowser` class:

### 🔍 **Core Search Methods**

- **`Search(string searchQuery, ShellContainer searchScope)`** - Simple filename search
- **`Search(SearchCondition searchCondition, ShellContainer searchScope)`** - Custom search condition
- **`SearchWithQueryString(string queryString, ShellContainer searchScope)`** - Windows Search syntax
- **`SearchByDateRange(DateTime startDate, DateTime endDate, ShellContainer searchScope)`** - Date-based search
- **`SearchByFileSize(long minSizeBytes, ShellContainer searchScope)`** - Size-based search
- **`SearchByProperty(string propertyName, string propertyValue, SearchConditionOperation operation, ShellContainer searchScope)`** - Property-based search
- **`ClearSearch(ShellObject location)`** - Clear search and navigate to location
- **`IsShowingSearchResults`** - Check if currently showing search results

## Usage Examples

### Simple Filename Search
```csharp
// Search for files containing "report" in Documents
explorerBrowser.Search("report", KnownFolders.Documents);
```

### Windows Search Syntax
```csharp
// Search for pictures modified today
explorerBrowser.SearchWithQueryString("kind:picture AND datemodified:today", KnownFolders.Pictures);
```

### Date Range Search
```csharp
// Search for files modified in the last week
var lastWeek = DateTime.Today.AddDays(-7);
explorerBrowser.SearchByDateRange(lastWeek, DateTime.Today, KnownFolders.Downloads);
```

### File Size Search
```csharp
// Search for files larger than 10MB
explorerBrowser.SearchByFileSize(10 * 1024 * 1024, KnownFolders.ComputerFolder);
```

### Property-Based Search
```csharp
// Search for documents by author
explorerBrowser.SearchByProperty("System.Author", "John Doe", SearchConditionOperation.Contains, KnownFolders.Documents);
```

### Custom Search Condition
```csharp
// Create complex search conditions
var filenameCondition = SearchConditionFactory.CreateLeafCondition(
    "System.FileName", "report", SearchConditionOperation.Contains
);

var dateCondition = SearchConditionFactory.CreateLeafCondition(
    "System.DateModified", DateTime.Today.AddDays(-30), SearchConditionOperation.GreaterThan
);

var combinedCondition = SearchConditionFactory.CreateAndCondition(filenameCondition, dateCondition);

explorerBrowser.Search(combinedCondition, KnownFolders.Documents);
```

## Requirements

- **Windows 10+** for full search functionality
- **Windows Search service** must be enabled
- **Search folders support** must be enabled in ExplorerBrowser options

## Setup

Enable search folders support in your ExplorerBrowser:

```csharp
explorerBrowser.ContentOptions.Flags |= ExplorerBrowserContentSectionOptions.UseSearchFolders;
```

## Available Search Properties

Common Windows Search properties you can use:

- `System.FileName` - File name
- `System.Title` - Document title
- `System.Author` - Document author
- `System.Keywords` - Keywords/tags
- `System.DateModified` - Modification date
- `System.DateCreated` - Creation date
- `System.Size` - File size
- `System.FileExtension` - File extension
- `System.Kind` - File type (document, picture, music, etc.)

## Search Operations

Available search operations:

- `Contains` - Value contains the search term
- `Equals` - Value equals the search term exactly
- `StartsWith` - Value starts with the search term
- `EndsWith` - Value ends with the search term
- `GreaterThan` - Value is greater than the search term
- `LessThan` - Value is less than the search term
- `GreaterThanOrEqual` - Value is greater than or equal to the search term
- `LessThanOrEqual` - Value is less than or equal to the search term

## Windows Search Syntax

The `SearchWithQueryString` method supports native Windows Search syntax:

- `kind:picture` - Search by file type
- `datemodified:today` - Search by date
- `size:>10MB` - Search by size
- `author:"John Doe"` - Search by author
- `AND`, `OR`, `NOT` - Logical operators
- `()` - Grouping

Examples:
- `kind:picture AND datemodified:today`
- `size:>10MB AND kind:document`
- `author:"John Doe" OR author:"Jane Smith"`

## Error Handling

All search methods throw `CommonControlException` with appropriate error messages when search operations fail. Common causes:

- Windows Search service not running
- Search scope not indexed
- Invalid search conditions
- Insufficient permissions

## Performance Considerations

- Search results are loaded asynchronously
- Large search scopes may take time to complete
- Consider limiting search scope for better performance
- Search results are cached by Windows Search

## Integration with Existing Code

The new search methods are **fully backward compatible**. Existing code will continue to work unchanged. You can gradually migrate to the new integrated methods:

**Before (External Helper):**
```csharp
// Old way - external helper
var searchHelper = new SearchHelper(explorerBrowser);
searchHelper.SearchFiles("report", KnownFolders.Documents);
```

**After (Integrated):**
```csharp
// New way - direct method call
explorerBrowser.Search("report", KnownFolders.Documents);
```

## WPF Support

The search functionality is also available in the WPF version of ExplorerBrowser:

```csharp
// WPF ExplorerBrowser
var wpfExplorerBrowser = new Microsoft.WindowsAPICodePack.Controls.WindowsPresentationFoundation.ExplorerBrowser();
wpfExplorerBrowser.Search("report", KnownFolders.Documents);
```

## Benefits of Integration

1. **Cleaner API** - No need for external helper classes
2. **Better Performance** - Direct integration with the control
3. **Consistent Error Handling** - Uses the same exception types
4. **Easier Maintenance** - All search logic in one place
5. **Better IntelliSense** - Search methods appear directly on ExplorerBrowser
6. **Reduced Dependencies** - No need to reference additional search classes

## Migration Guide

If you have existing search code using external helpers:

1. **Replace helper calls** with direct ExplorerBrowser method calls
2. **Update error handling** to catch `CommonControlException`
3. **Remove external search dependencies** from your project
4. **Test search functionality** to ensure compatibility

## Troubleshooting

### Search Not Working
- Verify Windows Search service is running (`services.msc` → "Windows Search")
- Check that search scope is indexed
- Ensure `UseSearchFolders` flag is set
- Verify Windows 10+ for full functionality

### Performance Issues
- Limit search scope to specific folders
- Use more specific search terms
- Consider using date/size filters to narrow results
- Ensure Windows Search index is up to date

### Error Messages
- **"Search failed"** - Check Windows Search service and permissions
- **"Invalid search condition"** - Verify property names and values
- **"Search scope not accessible"** - Check folder permissions and indexing

## Future Enhancements

Potential future additions to the integrated search API:

- **Search result filtering** - Filter results after search
- **Search history** - Track and replay previous searches
- **Advanced search UI** - Built-in search interface
- **Search result export** - Export search results to various formats
- **Search templates** - Save and reuse common search patterns

## Support

For issues or questions about the integrated search functionality:

1. Check this README for common solutions
2. Verify Windows Search service is working
3. Test with simple search queries first
4. Check Windows Event Viewer for search-related errors
5. Ensure proper permissions on search locations
