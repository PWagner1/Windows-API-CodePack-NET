namespace Microsoft.WindowsAPICodePack.Shell;

[ComImport,
 Guid(ShellIIDGuid.IQueryParserManager),
 InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IQueryParserManager
{
    // Create a query parser loaded with the schema for a certain catalog localize to a certain language, and initialized with
    // standard defaults. One valid value for riid is IID_IQueryParser.
    [PreserveSig]
    HResult CreateLoadedParser([In, MarshalAs(UnmanagedType.LPWStr)] string pszCatalog, [In] ushort langidForKeywords, [In] ref Guid riid, [Out] out IQueryParser ppQueryParser);

    // In addition to setting AQS/NQS and automatic wildcard for the given query parser, this sets up standard named entity handlers and
    // sets the keyboard locale as locale for word breaking.
    [PreserveSig]
    HResult InitializeOptions([In] bool fUnderstandNQS, [In] bool fAutoWildCard, [In] IQueryParser pQueryParser);

    // Change one of the settings for the query parser manager, such as the name of the schema binary, or the location of the localized and unlocalized
    // schema binaries. By default, the settings point to the schema binaries used by Windows Shell.
    [PreserveSig]
    HResult SetOption([In] QueryParserManagerOption option, [In] PropVariant pOptionValue);

};