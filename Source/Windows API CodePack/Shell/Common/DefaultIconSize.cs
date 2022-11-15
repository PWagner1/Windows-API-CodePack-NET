// Copyright (c) Microsoft Corporation.  All rights reserved.


namespace Microsoft.WindowsAPICodePack.Shell
{
    /// <summary>
    /// Defines the read-only properties for default shell icon sizes.
    /// </summary>
    public static class DefaultIconSize
    {
        /// <summary>
        /// The small size property for a 16x16 pixel Shell Icon.
        /// </summary>
        public static readonly System.Windows.Size Small = new(16, 16);

        /// <summary>
        /// The medium size property for a 32x32 pixel Shell Icon.
        /// </summary>
        public static readonly System.Windows.Size Medium = new(32, 32);

        /// <summary>
        /// The large size property for a 48x48 pixel Shell Icon.
        /// </summary>
        public static readonly System.Windows.Size Large = new(48, 48);

        /// <summary>
        /// The extra-large size property for a 256x256 pixel Shell Icon.
        /// </summary>
        public static readonly System.Windows.Size ExtraLarge = new(256, 256);

        /// <summary>
        /// The maximum size for a Shell Icon, 256x256 pixels.
        /// </summary>
        public static readonly System.Windows.Size Maximum = new(256, 256);

    }
}
