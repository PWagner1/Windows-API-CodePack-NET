using IConditionFactory = Microsoft.WindowsAPICodePack.Shell.IConditionFactory;
using ISearchFolderItemFactory = Microsoft.WindowsAPICodePack.Shell.Interop.ISearchFolderItemFactory;

namespace Microsoft.WindowsAPICodePack.Controls.WindowsForms;

/// <summary>
/// Extension methods for the <see cref="ExplorerBrowser"/> WinForms control.
/// </summary>
public static class ExplorerBrowserExtensions
{
    /// <summary>
    /// Navigates the specified <see cref="ExplorerBrowser"/> to a simple filename search within the given known folder.
    /// </summary>
    /// <param name="browser">The explorer browser to navigate.</param>
    /// <param name="searchQuery">The search text to use for matching file names.</param>
    /// <param name="searchScope">The known folder to search within.</param>
    public static void NavigateToSearch(this ExplorerBrowser browser, string searchQuery, IKnownFolder searchScope)
    {
        if (string.IsNullOrWhiteSpace(searchQuery))
        {
            throw new ArgumentException(@"Search query must not be empty.", nameof(searchQuery));
        }

        if (searchScope == null)
        {
            throw new ArgumentNullException(nameof(searchScope));
        }

        var searchFolderFactory = (ISearchFolderItemFactory)new SearchFolderItemFactory();

        IShellItemArray scopeArray = ShellItemArrayHelper.FromKnownFolder(searchScope);
        searchFolderFactory.SetScope(scopeArray);

        var conditionFactory = (IConditionFactory)new ConditionFactory();

        var propVar = new PROPVARIANT();
        try
        {
            PropVariantHelper.InitPropVariantFromString(searchQuery, out propVar);

            /*conditionFactory.MakeLeaf(
                "System.FileName",
                CONDITION_OPERATION.COP_VALUE_CONTAINS, // More useful for search
                propVar,
                null,
                null,
                null,
                null,
                false,
                out ICondition condition);

            searchFolderFactory.SetCondition(condition);*/
        }
        finally
        {
            propVar.Clear(); // Clean up native memory
        }

        Guid shellItemGuid = new Guid(ShellIIDGuid.IShellItem);
        searchFolderFactory.GetShellItem(0, ref shellItemGuid, out IShellItem shellItem);

        //browser.Navigate(shellItem);
    }
}