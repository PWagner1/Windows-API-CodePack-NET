namespace Microsoft.WindowsAPICodePack.Shell
{
    [ComImport,
     Guid(ShellIIDGuid.IQueryParserManager),
     CoClass(typeof(QueryParserManagerCoClass))]
    internal interface INativeQueryParserManager : IQueryParserManager
    {
    }
}