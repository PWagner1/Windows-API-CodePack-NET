namespace Microsoft.WindowsAPICodePack.Shell;

[ComImport,
 Guid(ShellIIDGuid.ISearchFolderItemFactory),
 CoClass(typeof(SearchFolderItemFactoryCoClass))]
internal interface INativeSearchFolderItemFactory : ISearchFolderItemFactory
{
}