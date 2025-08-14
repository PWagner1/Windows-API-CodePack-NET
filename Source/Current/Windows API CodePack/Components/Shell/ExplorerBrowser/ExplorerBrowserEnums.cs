//Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.WindowsAPICodePack.Controls;

/// <summary>
/// Defines the sorting order for search results in the ExplorerBrowser.
/// </summary>
public enum SearchResultSortOrder
{
    /// <summary>
    /// Sort by relevance (default search ranking).
    /// </summary>
    Relevance,

    /// <summary>
    /// Sort by file name in ascending order.
    /// </summary>
    NameAscending,

    /// <summary>
    /// Sort by file name in descending order.
    /// </summary>
    NameDescending,

    /// <summary>
    /// Sort by file size in ascending order.
    /// </summary>
    SizeAscending,

    /// <summary>
    /// Sort by file size in descending order.
    /// </summary>
    SizeDescending,

    /// <summary>
    /// Sort by date modified in ascending order.
    /// </summary>
    DateModifiedAscending,

    /// <summary>
    /// Sort by date modified in descending order.
    /// </summary>
    DateModifiedDescending,

    /// <summary>
    /// Sort by file type in ascending order.
    /// </summary>
    TypeAscending,

    /// <summary>
    /// Sort by file type in descending order.
    /// </summary>
    TypeDescending,

    /// <summary>
    /// Sort by date created in ascending order.
    /// </summary>
    DateCreatedAscending,

    /// <summary>
    /// Sort by date created in descending order.
    /// </summary>
    DateCreatedDescending,

    /// <summary>
    /// Sort by author in ascending order.
    /// </summary>
    AuthorAscending,

    /// <summary>
    /// Sort by author in descending order.
    /// </summary>
    AuthorDescending
}

/// <summary>
/// Defines the export format for search results.
/// </summary>
public enum SearchResultExportFormat
{
    /// <summary>
    /// Export as comma-separated values (CSV).
    /// </summary>
    CSV,

    /// <summary>
    /// Export as tab-separated values (TSV).
    /// </summary>
    TSV,

    /// <summary>
    /// Export as XML format.
    /// </summary>
    XML,

    /// <summary>
    /// Export as JSON format.
    /// </summary>
    JSON,

    /// <summary>
    /// Export as HTML table format.
    /// </summary>
    HTML,

    /// <summary>
    /// Export as plain text format.
    /// </summary>
    Text
}

/// <summary>
/// Defines the search result grouping options.
/// </summary>
public enum SearchResultGrouping
{
    /// <summary>
    /// No grouping applied.
    /// </summary>
    None,

    /// <summary>
    /// Group by file type.
    /// </summary>
    ByFileType,

    /// <summary>
    /// Group by date modified.
    /// </summary>
    ByDateModified,

    /// <summary>
    /// Group by file size.
    /// </summary>
    ByFileSize,

    /// <summary>
    /// Group by author.
    /// </summary>
    ByAuthor,

    /// <summary>
    /// Group by folder location.
    /// </summary>
    ByLocation,

    /// <summary>
    /// Group by search relevance.
    /// </summary>
    ByRelevance
}

/// <summary>
/// Defines the search result display mode.
/// </summary>
public enum SearchResultDisplayMode
{
    /// <summary>
    /// Display as list view.
    /// </summary>
    List,

    /// <summary>
    /// Display as details view.
    /// </summary>
    Details,

    /// <summary>
    /// Display as tiles view.
    /// </summary>
    Tiles,

    /// <summary>
    /// Display as content view.
    /// </summary>
    Content,

    /// <summary>
    /// Display as small icons view.
    /// </summary>
    SmallIcons,

    /// <summary>
    /// Display as medium icons view.
    /// </summary>
    MediumIcons,

    /// <summary>
    /// Display as large icons view.
    /// </summary>
    LargeIcons,

    /// <summary>
    /// Display as extra large icons view.
    /// </summary>
    ExtraLargeIcons
}

/// <summary>
/// Defines the search result filtering options.
/// </summary>
public enum SearchResultFilter
{
    /// <summary>
    /// No filtering applied.
    /// </summary>
    None,

    /// <summary>
    /// Filter to show only files.
    /// </summary>
    FilesOnly,

    /// <summary>
    /// Filter to show only folders.
    /// </summary>
    FoldersOnly,

    /// <summary>
    /// Filter to show only images.
    /// </summary>
    ImagesOnly,

    /// <summary>
    /// Filter to show only documents.
    /// </summary>
    DocumentsOnly,

    /// <summary>
    /// Filter to show only media files.
    /// </summary>
    MediaOnly,

    /// <summary>
    /// Filter to show only compressed files.
    /// </summary>
    CompressedOnly,

    /// <summary>
    /// Filter to show only executable files.
    /// </summary>
    ExecutablesOnly,

    /// <summary>
    /// Filter to show only hidden files.
    /// </summary>
    HiddenOnly,

    /// <summary>
    /// Filter to show only system files.
    /// </summary>
    SystemOnly
}