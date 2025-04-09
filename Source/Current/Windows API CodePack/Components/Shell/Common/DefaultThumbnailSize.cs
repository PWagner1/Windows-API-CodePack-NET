namespace Microsoft.WindowsAPICodePack.Shell
{
    /// <summary>
    /// Defines the read-only properties for default shell thumbnail sizes.
    /// </summary>
    public static class DefaultThumbnailSize
    {
        /// <summary>
        /// Gets the small size property for a 32x32 pixel Shell Thumbnail.
        /// </summary>
        public static readonly System.Windows.Size Small = new(32, 32);

        /// <summary>
        /// Gets the medium size property for a 96x96 pixel Shell Thumbnail.
        /// </summary>
        public static readonly System.Windows.Size Medium = new(96, 96);

        /// <summary>
        /// Gets the large size property for a 256x256 pixel Shell Thumbnail.
        /// </summary>
        public static readonly System.Windows.Size Large = new(256, 256);

        /// <summary>
        /// Gets the extra-large size property for a 1024x1024 pixel Shell Thumbnail.
        /// </summary>
        public static readonly System.Windows.Size ExtraLarge = new(1024, 1024);

        /// <summary>
        /// Maximum size for the Shell Thumbnail, 1024x1024 pixels.
        /// </summary>
        public static readonly System.Windows.Size Maximum = new(1024, 1024);
    }
}