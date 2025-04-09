namespace Microsoft.WindowsAPICodePack.ShellExtensions
{
    /// <summary>
    /// Defines the minimum thumbnail size for which thumbnails will be generated.
    /// </summary>
    public enum ThumbnailCutoffSize
    {
        /// <summary>
        /// Default size of 20x20
        /// </summary>
        Square20 = -1, //For 20x20, you do not add any key in the registry

        /// <summary>
        /// Size of 32x32
        /// </summary>
        Square32 = 0,

        /// <summary>
        /// Size of 16x16
        /// </summary>
        Square16 = 1,

        /// <summary>
        /// Size of 48x48
        /// </summary>
        Square48 = 2,

        /// <summary>
        /// Size of 16x16. An alternative to Square16.
        /// </summary>
        Square16B = 3
    }
}