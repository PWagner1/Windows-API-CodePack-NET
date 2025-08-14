using Microsoft.WindowsAPICodePack.Controls.WindowsForms;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace IntegratedSearchExample
{
    /// <summary>
    /// Example demonstrating the new integrated search functionality in ExplorerBrowser
    /// </summary>
    public class IntegratedSearchExample
    {
        private ExplorerBrowser explorerBrowser;

        public IntegratedSearchExample()
        {
            // Initialize ExplorerBrowser
            explorerBrowser = new ExplorerBrowser();
            
            // Enable search folders support (required for search functionality)
            explorerBrowser.ContentOptions.Flags |= ExplorerBrowserContentSectionOptions.UseSearchFolders;
        }

        /// <summary>
        /// Example 1: Simple filename search
        /// </summary>
        public void SearchForFiles()
        {
            // Search for files containing "report" in Documents folder
            explorerBrowser.Search("report", KnownFolders.Documents);
        }

        /// <summary>
        /// Example 2: Search using Windows Search syntax
        /// </summary>
        public void SearchWithWindowsSyntax()
        {
            // Search for pictures modified today
            explorerBrowser.SearchWithQueryString("kind:picture AND datemodified:today", KnownFolders.Pictures);
        }

        /// <summary>
        /// Example 3: Date range search
        /// </summary>
        public void SearchByDateRange()
        {
            var today = DateTime.Today;
            var lastWeek = today.AddDays(-7);
            
            // Search for files modified in the last week
            explorerBrowser.SearchByDateRange(lastWeek, today, KnownFolders.Downloads);
        }

        /// <summary>
        /// Example 4: File size search
        /// </summary>
        public void SearchByFileSize()
        {
            // Search for files larger than 10MB
            explorerBrowser.SearchByFileSize(10 * 1024 * 1024, KnownFolders.ComputerFolder);
        }

        /// <summary>
        /// Example 5: Property-based search
        /// </summary>
        public void SearchByProperty()
        {
            // Search for documents by author
            explorerBrowser.SearchByProperty("System.Author", "John Doe", SearchConditionOperation.Contains, KnownFolders.Documents);
        }

        /// <summary>
        /// Example 6: Custom search condition
        /// </summary>
        public void SearchWithCustomCondition()
        {
            // Create a complex search condition
            var filenameCondition = SearchConditionFactory.CreateLeafCondition(
                "System.FileName", 
                "report", 
                SearchConditionOperation.Contains
            );

            var dateCondition = SearchConditionFactory.CreateLeafCondition(
                "System.DateModified",
                DateTime.Today.AddDays(-30),
                SearchConditionOperation.GreaterThan
            );

            var combinedCondition = SearchConditionFactory.CreateAndCondition(
                filenameCondition,
                dateCondition
            );

            // Use the custom condition
            explorerBrowser.Search(combinedCondition, KnownFolders.Documents);
        }

        /// <summary>
        /// Example 7: Clear search and return to normal view
        /// </summary>
        public void ClearSearch()
        {
            // Clear search and go to Desktop
            explorerBrowser.ClearSearch(KnownFolders.Desktop);
        }

        /// <summary>
        /// Example 8: Check if currently showing search results
        /// </summary>
        public void CheckSearchStatus()
        {
            if (explorerBrowser.IsShowingSearchResults)
            {
                Console.WriteLine("Currently showing search results");
            }
            else
            {
                Console.WriteLine("Currently showing normal folder view");
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
    /// Example usage of the integrated search functionality
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            using (var example = new IntegratedSearchExample())
            {
                Console.WriteLine("ExplorerBrowser Integrated Search Examples");
                Console.WriteLine("==========================================");

                // Example 1: Simple search
                Console.WriteLine("\n1. Searching for files containing 'report' in Documents...");
                example.SearchForFiles();

                // Example 2: Windows Search syntax
                Console.WriteLine("\n2. Searching for pictures modified today...");
                example.SearchWithWindowsSyntax();

                // Example 3: Date range search
                Console.WriteLine("\n3. Searching for files modified in the last week...");
                example.SearchByDateRange();

                // Example 4: File size search
                Console.WriteLine("\n4. Searching for files larger than 10MB...");
                example.SearchByFileSize();

                // Example 5: Property search
                Console.WriteLine("\n5. Searching for documents by author...");
                example.SearchByProperty();

                // Example 6: Custom condition
                Console.WriteLine("\n6. Searching with custom condition...");
                example.SearchWithCustomCondition();

                // Example 7: Check search status
                Console.WriteLine("\n7. Checking search status...");
                example.CheckSearchStatus();

                // Example 8: Clear search
                Console.WriteLine("\n8. Clearing search...");
                example.ClearSearch();

                Console.WriteLine("\nAll examples completed!");
            }
        }
    }
}
