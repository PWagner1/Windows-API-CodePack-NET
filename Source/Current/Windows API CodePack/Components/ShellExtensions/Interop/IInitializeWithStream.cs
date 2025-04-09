namespace Microsoft.WindowsAPICodePack.ShellExtensions.Interop
{
    /// <summary>
    /// Provides means by which to initialize with a stream.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    [ComImport]
    [Guid("b824b49d-22ac-4161-ac8a-9916e8fa3f7f")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IInitializeWithStream
    {
        /// <summary>
        /// Initializes with a stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileMode"></param>
        void Initialize(IStream? stream, AccessModes fileMode);
    }
}