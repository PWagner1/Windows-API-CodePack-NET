# ExplorerBrowser Search Implementation

## Overview

The implementation provides:
- **Address Bar**: Shows current location and allows manual navigation
- **Search Box**: Performs file/folder searches within specified scopes
- **Scope Selector**: Choose where to search (This PC, Documents, Pictures, etc.)
- **Search Results**: Displayed directly in the ExplorerBrowser control

## Key Components

### 1. Search Implementation

The search functionality uses the `ShellSearchFolder` class to create search queries:

```csharp
// Create search condition for filename search
var searchCondition = SearchConditionFactory.CreateLeafCondition(
    "System.FileName", 
    searchQuery, 
    SearchConditionOperation.Contains
);

// Create search folder and navigate to results
using (var searchFolder = new ShellSearchFolder(searchCondition, selectedScope))
{
    explorerBrowser.Navigate(searchFolder);
}
```

### 2. ExplorerBrowser Configuration

Enable search folders support:

```csharp
explorerBrowser.ContentOptions.Flags |= ExplorerBrowserContentSectionOptions.UseSearchFolders;
```

### 3. Search Scopes

The demo includes common search scopes:
- This PC
- Desktop
- Documents
- Downloads
- Pictures
- Music
- Videos
- User Files

## How It Works

1. **Search Query Creation**: Uses `SearchConditionFactory.CreateLeafCondition()` to create search conditions
2. **Search Execution**: Creates a `ShellSearchFolder` with the search condition and scope
3. **Results Display**: Navigates the ExplorerBrowser to the search results
4. **Navigation**: Updates the address bar to show current location or search status

## Search Properties

The search supports various Windows Search properties. Common ones include:

- `System.FileName` - File name search
- `System.Title` - Document title
- `System.Author` - Document author
- `System.Keywords` - Keywords/tags
- `System.DateModified` - Modification date
- `System.Size` - File size

## ExplorerBrowser Search Properties

The `ExplorerBrowser` control now includes a comprehensive set of properties that allow developers to configure search behavior, appearance, and performance. All search-related properties are organized into expandable objects that work seamlessly with PropertyGrid controls.

### Property Organization with ExpandableObjectConverter

The search properties are now organized into two main groups:

#### SearchState Properties (Read-only)
- `CurrentSearchQuery` - Gets the current search query
- `CurrentSearchScope` - Gets the current search scope  
- `IsShowingSearchResults` - Gets whether currently showing search results

#### SearchOptions Properties (Configurable)
- **Search Behavior**: Auto-refresh, max results, progress indicators, caching
- **Display Options**: Sort order, grouping, file previews, thumbnail size
- **Advanced Features**: Incremental search, suggestions, term highlighting, filters
- **Performance Optimization**: Virtual scrolling, page sizing
- **User Experience**: Context menus, drag & drop, tooltips, selection persistence
- **Export & Sharing**: Export formats, statistics, sharing capabilities

### Benefits of the New Organization

1. **Better PropertyGrid Integration**: Properties display as expandable nodes
2. **Logical Grouping**: Related properties are organized by category
3. **Professional Appearance**: Clean, organized property display
4. **Easier Configuration**: Developers can quickly locate specific settings
5. **Backward Compatibility**: All existing properties remain available

### Demos

- **ExplorerBrowserPropertiesDemo**: Interactive demo with individual controls for each property
- **ExplorerBrowserPropertyGridDemo**: PropertyGrid demo showcasing the new ExpandableObjectConverter functionality

For a complete list of properties and usage examples, see the [ExplorerBrowser Properties Demo](../Source/Samples/Shell/ExplorerBrowserPropertiesDemo/README.md).

For information about using the new grouped properties in PropertyGrid controls, see the [ExplorerBrowser PropertyGrid Demo](../Source/Samples/Shell/ExplorerBrowserPropertyGridDemo/README.md).

## Advanced Search Examples

### Date-based Search
```csharp
var dateCondition = SearchConditionFactory.CreateLeafCondition(
    "System.DateModified",
    DateTime.Today.AddDays(-7),
    SearchConditionOperation.GreaterThan
);
```

### Size-based Search
```csharp
var sizeCondition = SearchConditionFactory.CreateLeafCondition(
    "System.Size",
    1024 * 1024, // 1MB
    SearchConditionOperation.GreaterThan
);
```

### Combined Search
```csharp
var combinedCondition = SearchConditionFactory.CreateAndCondition(
    filenameCondition,
    dateCondition
);
```

## Requirements

- Windows 10 or later (for full search functionality)
- .NET 6.0 or later
- Windows API Code Pack components
- Windows Search service enabled

## Building and Running

1. Ensure the Windows API Code Pack components are built
2. Build the demo project: `dotnet build`
3. Run the application: `dotnet run`

## Troubleshooting

### Search Not Working
- Verify Windows Search service is running
- Check that the search scope is indexed
- Ensure proper permissions on search locations

### Navigation Errors
- Verify paths exist and are accessible
- Check for special characters in paths
- Ensure proper Shell object creation

### Performance Issues
- Search results are loaded asynchronously
- Large search scopes may take time
- Consider limiting search scope for better performance

## Extending the Implementation

### Custom Search Properties
Add new search properties by extending the search condition creation:

```csharp
var customCondition = SearchConditionFactory.CreateLeafCondition(
    "Custom.Property.Name",
    searchValue,
    SearchConditionOperation.Equals
);
```

### Multiple Search Scopes
Combine multiple search scopes:

```csharp
var searchFolder = new ShellSearchFolder(
    searchCondition,
    KnownFolders.Documents,
    KnownFolders.Pictures,
    KnownFolders.Downloads
);
```

### Search Result Filtering
Filter search results after creation:

```csharp
var filteredResults = searchFolder
    .Where(item => item.Properties.System.Size.Value > 1024)
    .ToList();
```

## API Reference

Key classes used:
- `ExplorerBrowser` - Main browser control
- `ShellSearchFolder` - Search results container
- `SearchConditionFactory` - Search query builder
- `SearchCondition` - Individual search criteria
- `ShellContainer` - Search scope definition