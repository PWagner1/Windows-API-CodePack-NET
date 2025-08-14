using Microsoft.WindowsAPICodePack.Controls.WindowsForms;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace SimpleSearchExample
{
    /// <summary>
    /// Simple example showing how to implement search with ExplorerBrowser
    /// </summary>
    public class SimpleSearchExample
    {
        private ExplorerBrowser explorerBrowser;

        public SimpleSearchExample()
        {
            // Initialize ExplorerBrowser
            explorerBrowser = new ExplorerBrowser();
            
            // Enable search folders support (required for search functionality)
            explorerBrowser.ContentOptions.Flags |= ExplorerBrowserContentSectionOptions.UseSearchFolders;
        }

        /// <summary>
        /// Perform a simple filename search
        /// </summary>
        /// <param name="searchTerm">Text to search for in filenames</param>
        /// <param name="searchScope">Where to search (e.g., KnownFolders.Documents)</param>
        public void SearchFiles(string searchTerm, ShellContainer searchScope)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return;

            try
            {
                // Create search condition for filename search
                var searchCondition = SearchConditionFactory.CreateLeafCondition(
                    "System.FileName",           // Search in filename property
                    searchTerm,                  // Search term
                    SearchConditionOperation.Contains  // Contains operation
                );

                // Create search folder with the condition and scope
                using (var searchFolder = new ShellSearchFolder(searchCondition, searchScope))
                {
                    // Navigate ExplorerBrowser to search results
                    explorerBrowser.Navigate(searchFolder);
                }
            }
            catch (Exception ex)
            {
                // Handle search errors
                Console.WriteLine($"Search failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Search for files modified within a date range
        /// </summary>
        /// <param name="startDate">Start date for search</param>
        /// <param name="endDate">End date for search</param>
        /// <param name="searchScope">Where to search</param>
        public void SearchByDateRange(DateTime startDate, DateTime endDate, ShellContainer searchScope)
        {
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
                var combinedCondition = SearchConditionFactory.CreateAndCondition(
                    startCondition,
                    endCondition
                );

                // Execute search
                using (var searchFolder = new ShellSearchFolder(combinedCondition, searchScope))
                {
                    explorerBrowser.Navigate(searchFolder);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Date search failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Search for files larger than a specified size
        /// </summary>
        /// <param name="minSizeBytes">Minimum file size in bytes</param>
        /// <param name="searchScope">Where to search</param>
        public void SearchByFileSize(long minSizeBytes, ShellContainer searchScope)
        {
            try
            {
                var sizeCondition = SearchConditionFactory.CreateLeafCondition(
                    "System.Size",
                    minSizeBytes,
                    SearchConditionOperation.GreaterThan
                );

                using (var searchFolder = new ShellSearchFolder(sizeCondition, searchScope))
                {
                    explorerBrowser.Navigate(searchFolder);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Size search failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Search using a custom query string (Windows Search syntax)
        /// </summary>
        /// <param name="queryString">Search query in Windows Search format</param>
        /// <param name="searchScope">Where to search</param>
        public void SearchWithQueryString(string queryString, ShellContainer searchScope)
        {
            try
            {
                // Parse structured query (supports Windows Search syntax)
                var searchCondition = SearchConditionFactory.ParseStructuredQuery(
                    queryString,
                    System.Globalization.CultureInfo.CurrentCulture
                );

                using (var searchFolder = new ShellSearchFolder(searchCondition, searchScope))
                {
                    explorerBrowser.Navigate(searchFolder);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Query string search failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Clear search and return to normal folder view
        /// </summary>
        /// <param name="folder">Folder to navigate to</param>
        public void ClearSearch(ShellContainer folder)
        {
            try
            {
                explorerBrowser.Navigate(folder);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Navigation failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Get the ExplorerBrowser control for use in UI
        /// </summary>
        public ExplorerBrowser GetExplorerBrowser()
        {
            return explorerBrowser;
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            explorerBrowser?.Dispose();
        }
    }

    /// <summary>
    /// Example usage of the search functionality
    /// </summary>
    public class SearchExampleUsage
    {
        public static void Main()
        {
            using (var searchExample = new SimpleSearchExample())
            {
                // Example 1: Search for files containing "report" in Documents
                searchExample.SearchFiles("report", KnownFolders.Documents);

                // Example 2: Search for files modified today in Downloads
                var today = DateTime.Today;
                searchExample.SearchByDateRange(today, today.AddDays(1), KnownFolders.Downloads);

                // Example 3: Search for large files (>10MB) in This PC
                searchExample.SearchByFileSize(10 * 1024 * 1024, KnownFolders.ComputerFolder);

                // Example 4: Use Windows Search syntax
                searchExample.SearchWithQueryString("kind:picture AND datemodified:today", KnownFolders.Pictures);

                // Example 5: Clear search and go to Desktop
                searchExample.ClearSearch(KnownFolders.Desktop);
            }
        }
    }
}
